//------------------------------------------------------------------------------
// The contents of this file are subject to the nopCommerce Public License Version 1.0 ("License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at  http://www.nopCommerce.com/License.aspx. 
// 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. 
// See the License for the specific language governing rights and limitations under the License.
// 
// The Original Code is nopCommerce.
// The Initial Developer of the Original Code is NopSolutions.
// All Rights Reserved.
// 
// Contributor(s): _______. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Services.Protocols;
using System.Xml;
using Nop.Shipping.FedEx.RateServiceWebReference;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Measures;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.Common;

namespace NopSolutions.NopCommerce.Shipping.Methods.FedEx
{
    /// <summary>
    /// FedEx computation method
    /// </summary>
    public class FedExComputationMethod : IShippingRateComputationMethod
    {
        #region Const

        private const int MAXPACKAGEWEIGHT = 150;
        #endregion

        #region Utilities

        private RateRequest CreateRateRequest(ShipmentPackage ShipmentPackage)
        {
            // Build the RateRequest
            RateRequest request = new RateRequest();
            //
            request.WebAuthenticationDetail = new WebAuthenticationDetail();
            request.WebAuthenticationDetail.UserCredential = new WebAuthenticationCredential();
            request.WebAuthenticationDetail.UserCredential.Key = SettingManager.GetSettingValue("ShippingRateComputationMethod.FedEx.Key"); // Replace "XXX" with the Key
            request.WebAuthenticationDetail.UserCredential.Password = SettingManager.GetSettingValue("ShippingRateComputationMethod.FedEx.Password"); // Replace "XXX" with the Password
            //
            request.ClientDetail = new ClientDetail();
            request.ClientDetail.AccountNumber = SettingManager.GetSettingValue("ShippingRateComputationMethod.FedEx.AccountNumber"); // Replace "XXX" with client's account number
            request.ClientDetail.MeterNumber = SettingManager.GetSettingValue("ShippingRateComputationMethod.FedEx.MeterNumber"); // Replace "XXX" with client's meter number
            //
            request.TransactionDetail = new TransactionDetail();
            request.TransactionDetail.CustomerTransactionId = "***Rate Available Services v7 Request - nopCommerce***"; // This is a reference field for the customer.  Any value can be used and will be provided in the response.

            request.Version = new VersionId(); // WSDL version information, value is automatically set from wsdl            

            request.ReturnTransitAndCommit = true;
            request.ReturnTransitAndCommitSpecified = true;
            request.CarrierCodes = new CarrierCodeType[2];
            // Insert the Carriers you would like to see the rates for
            request.CarrierCodes[0] = CarrierCodeType.FDXE;
            request.CarrierCodes[1] = CarrierCodeType.FDXG;

            decimal orderSubTotal = ShoppingCartManager.GetShoppingCartSubTotal(ShipmentPackage.Items, ShipmentPackage.Customer);
            SetShipmentDetails(request, ShipmentPackage, orderSubTotal);
            SetOrigin(request);
            SetDestination(request, ShipmentPackage);
            SetPayment(request, ShipmentPackage);
            SetIndividualPackageLineItems(request, ShipmentPackage, orderSubTotal);

            return request;
        }

        private void SetShipmentDetails(RateRequest request, ShipmentPackage ShipmentPackage, decimal orderSubTotal)
        {
            request.RequestedShipment = new RequestedShipment();
            request.RequestedShipment.DropoffType = DropoffType.REGULAR_PICKUP; //Drop off types are BUSINESS_SERVICE_CENTER, DROP_BOX, REGULAR_PICKUP, REQUEST_COURIER, STATION
            request.RequestedShipment.TotalInsuredValue = new Money();
            request.RequestedShipment.TotalInsuredValue.Amount = orderSubTotal;
            request.RequestedShipment.TotalInsuredValue.Currency = CurrencyManager.PrimaryStoreCurrency.CurrencyCode.ToString();
            request.RequestedShipment.ShipTimestamp = DateTime.Now; // Shipping date and time
            request.RequestedShipment.ShipTimestampSpecified = true;
            request.RequestedShipment.RateRequestTypes = new RateRequestType[2];
            request.RequestedShipment.RateRequestTypes[0] = RateRequestType.ACCOUNT;
            request.RequestedShipment.RateRequestTypes[1] = RateRequestType.LIST;
            request.RequestedShipment.PackageDetail = RequestedPackageDetailType.INDIVIDUAL_PACKAGES;
            request.RequestedShipment.PackageDetailSpecified = true;
        }

        private void SetPayment(RateRequest request, ShipmentPackage ShipmentPackage)
        {
            request.RequestedShipment.ShippingChargesPayment = new Payment(); // Payment Information
            request.RequestedShipment.ShippingChargesPayment.PaymentType = PaymentType.SENDER; // Payment options are RECIPIENT, SENDER, THIRD_PARTY
            request.RequestedShipment.ShippingChargesPayment.PaymentTypeSpecified = true;
            request.RequestedShipment.ShippingChargesPayment.Payor = new Payor();
            request.RequestedShipment.ShippingChargesPayment.Payor.AccountNumber = SettingManager.GetSettingValue("ShippingRateComputationMethod.FedEx.AccountNumber"); // Replace "XXX" with client's account number
        }

        private void SetDestination(RateRequest request, ShipmentPackage ShipmentPackage)
        {
            request.RequestedShipment.Recipient = new Party();
            request.RequestedShipment.Recipient.Address = new Address();
            request.RequestedShipment.Recipient.Address.StreetLines = new string[1] { "Recipient Address Line 1" };
            request.RequestedShipment.Recipient.Address.City = ShipmentPackage.ShippingAddress.City;
            if (ShipmentPackage.ShippingAddress.StateProvince != null)
            {
                request.RequestedShipment.Recipient.Address.StateOrProvinceCode = ShipmentPackage.ShippingAddress.StateProvince.Abbreviation;
            }
            else
            {
                request.RequestedShipment.Recipient.Address.StateOrProvinceCode = string.Empty;
            }
            request.RequestedShipment.Recipient.Address.PostalCode = ShipmentPackage.ShippingAddress.ZipPostalCode;
            request.RequestedShipment.Recipient.Address.CountryCode = ShipmentPackage.ShippingAddress.Country.TwoLetterISOCode;
        }

        private void SetOrigin(RateRequest request)
        {
            request.RequestedShipment.Shipper = new Party();
            request.RequestedShipment.Shipper.Address = new Address();
            request.RequestedShipment.Shipper.Address.StreetLines = new string[1] { SettingManager.GetSettingValue("ShippingRateComputationMethod.FedEx.ShippingOrigin.Street") };
            request.RequestedShipment.Shipper.Address.City = SettingManager.GetSettingValue("ShippingRateComputationMethod.FedEx.ShippingOrigin.City");
            request.RequestedShipment.Shipper.Address.StateOrProvinceCode = SettingManager.GetSettingValue("ShippingRateComputationMethod.FedEx.ShippingOrigin.StateOrProvinceCode");
            request.RequestedShipment.Shipper.Address.PostalCode = SettingManager.GetSettingValue("ShippingRateComputationMethod.FedEx.ShippingOrigin.PostalCode");
            request.RequestedShipment.Shipper.Address.CountryCode = SettingManager.GetSettingValue("ShippingRateComputationMethod.FedEx.ShippingOrigin.CountryCode");
        }

        private void SetIndividualPackageLineItems(RateRequest request, ShipmentPackage ShipmentPackage, decimal orderSubTotal)
        {
            // ------------------------------------------
            // Passing individual pieces rate request
            // ------------------------------------------


            int length = Convert.ToInt32(Math.Ceiling(ShipmentPackage.GetTotalLength()));
            int height = Convert.ToInt32(Math.Ceiling(ShipmentPackage.GetTotalHeight()));
            int width = Convert.ToInt32(Math.Ceiling(ShipmentPackage.GetTotalWidth()));
            int weight = Convert.ToInt32(Math.Ceiling(ShippingManager.GetShoppingCartTotalWeigth(ShipmentPackage.Items)));
            if ((!IsPackageTooHeavy(weight)) && (!IsPackageTooLarge(length, height, width)))
            {
                request.RequestedShipment.PackageCount = "1";

                request.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[1];
                request.RequestedShipment.RequestedPackageLineItems[0] = new RequestedPackageLineItem();
                request.RequestedShipment.RequestedPackageLineItems[0].SequenceNumber = "1"; // package sequence number            
                request.RequestedShipment.RequestedPackageLineItems[0].Weight = new Weight(); // package weight
                request.RequestedShipment.RequestedPackageLineItems[0].Weight.Units = WeightUnits.LB;
                request.RequestedShipment.RequestedPackageLineItems[0].Weight.Value = weight;
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions = new Dimensions(); // package dimensions

                //it's better to don't pass dims now
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Length = "0";
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Width = "0";
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Height = "0";
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Units = LinearUnits.IN;
                request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue = new Money(); // insured value
                request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.Amount = orderSubTotal;
                request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.Currency = CurrencyManager.PrimaryStoreCurrency.CurrencyCode.ToString();

            }
            else
            {
                int totalPackages = 1;
                int totalPackagesDims = 1;
                int totalPackagesWeights = 1;
                if (IsPackageTooHeavy(weight))
                {
                    totalPackagesWeights = Convert.ToInt32(Math.Ceiling((decimal)weight / (decimal)MAXPACKAGEWEIGHT));
                }
                if (IsPackageTooLarge(length, height, width))
                {
                    totalPackagesDims = TotalPackageSize(length, height, width) / 108;
                }
                totalPackages = totalPackagesDims > totalPackagesWeights ? totalPackagesDims : totalPackagesWeights;
                if (totalPackages == 0)
                    totalPackages = 1;

                int weight2 = weight / totalPackages;
                int height2 = height / totalPackages;
                int width2 = width / totalPackages;
                decimal orderSubTotal2 = orderSubTotal / totalPackages;
               
                request.RequestedShipment.PackageCount = totalPackages.ToString();
                request.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[totalPackages];

                for (int i = 0; i < totalPackages; i++)
                {
                    request.RequestedShipment.RequestedPackageLineItems[i] = new RequestedPackageLineItem();
                    request.RequestedShipment.RequestedPackageLineItems[i].SequenceNumber = (i + 1).ToString(); // package sequence number            
                    request.RequestedShipment.RequestedPackageLineItems[i].Weight = new Weight(); // package weight
                    request.RequestedShipment.RequestedPackageLineItems[i].Weight.Units = WeightUnits.LB;
                    request.RequestedShipment.RequestedPackageLineItems[i].Weight.Value = (decimal)weight2;
                    request.RequestedShipment.RequestedPackageLineItems[i].Dimensions = new Dimensions(); // package dimensions
                    
                    //it's better to don't pass dims now
                    request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Length = "0";
                    request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Width = "0";
                    request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Height = "0";
                    request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Units = LinearUnits.IN;
                    request.RequestedShipment.RequestedPackageLineItems[i].InsuredValue = new Money(); // insured value
                    request.RequestedShipment.RequestedPackageLineItems[i].InsuredValue.Amount = orderSubTotal2;
                    request.RequestedShipment.RequestedPackageLineItems[i].InsuredValue.Currency = CurrencyManager.PrimaryStoreCurrency.CurrencyCode.ToString();
                }
            }
        }

        private ShippingOptionCollection ParseResponse(RateReply reply)
        {
            ShippingOptionCollection result = new ShippingOptionCollection();

            Debug.WriteLine("RateReply details:");
            Debug.WriteLine("**********************************************************");
            foreach (RateReplyDetail rateDetail in reply.RateReplyDetails)
            {
                Debug.WriteLine("ServiceType: " + rateDetail.ServiceType);
                for (int i = 0; i < rateDetail.RatedShipmentDetails.Length; i++)
                {
                    RatedShipmentDetail shipmentDetail = rateDetail.RatedShipmentDetails[i];
                    Debug.WriteLine("RateType : " + shipmentDetail.ShipmentRateDetail.RateType);
                    Debug.WriteLine("Total Billing Weight : " + shipmentDetail.ShipmentRateDetail.TotalBillingWeight.Value);
                    Debug.WriteLine("Total Base Charge : " + shipmentDetail.ShipmentRateDetail.TotalBaseCharge.Amount);
                    Debug.WriteLine("Total Discount : " + shipmentDetail.ShipmentRateDetail.TotalFreightDiscounts.Amount);
                    Debug.WriteLine("Total Surcharges : " + shipmentDetail.ShipmentRateDetail.TotalSurcharges.Amount);
                    Debug.WriteLine("Net Charge : " + shipmentDetail.ShipmentRateDetail.TotalNetCharge.Amount);
                    Debug.WriteLine("*********");

                    //take first one
                    if (i == 0)
                    {
                        ShippingOption shippingOption = new ShippingOption();
                        string userFriendlyServiceType = GetUserFriendlyEnum(rateDetail.ServiceType.ToString());
                        shippingOption.Name = userFriendlyServiceType;
                        shippingOption.Rate = shipmentDetail.ShipmentRateDetail.TotalNetCharge.Amount;
                        result.Add(shippingOption);
                    }
                }
                Debug.WriteLine("**********************************************************");
            }

            return result;
        }

        private string GetUserFriendlyEnum(string s)
        {
            string result = string.Empty;
            char[] letters = s.ToCharArray();
            foreach (char c in letters)
            {
                if (c.ToString() == "_")
                    result += " ";
                else
                    result += c.ToString();
            }
            return result;
        }

        private bool IsPackageTooLarge(int length, int height, int width)
        {
            int total = TotalPackageSize(length, height, width);
            if (total > 165)
                return true;
            else
                return false;
        }

        private int TotalPackageSize(int length, int height, int width)
        {
            int girth = height + height + width + width;
            int total = girth + length;
            return total;
        }

        private bool IsPackageTooHeavy(int weight)
        {
            if (weight > MAXPACKAGEWEIGHT)
                return true;
            else
                return false;
        }

        #endregion

        #region Methods
        /// <summary>
        ///  Gets available shipping options
        /// </summary>
        /// <param name="ShipmentPackage">Shipment package</param>
        /// <param name="Error">Error</param>
        /// <returns>Shipping options</returns>
        public ShippingOptionCollection GetShippingOptions(ShipmentPackage ShipmentPackage, ref string Error)
        {
            ShippingOptionCollection shippingOptions = new ShippingOptionCollection();
            if (ShipmentPackage == null)
                throw new ArgumentNullException("ShipmentPackage");
            if (ShipmentPackage.Items == null)
                throw new NopException("No shipment items");
            if (ShipmentPackage.ShippingAddress == null)
            {
                Error = "Shipping address is not set";
                return shippingOptions;
            }
            if (ShipmentPackage.ShippingAddress.Country == null)
            {
                Error = "Shipping country is not set";
                return shippingOptions;
            }

            MeasureWeight baseWeightIn = MeasureManager.BaseWeightIn;
            if (baseWeightIn.SystemKeyword != "lb")
                throw new NopException("USPS shipping service. Base weight should be set to lb(s)");

            MeasureDimension baseDimensionIn = MeasureManager.BaseDimensionIn;
            if (baseDimensionIn.SystemKeyword != "inches")
                throw new NopException("USPS shipping service. Base dimension should be set to inch(es)");



            RateRequest request = CreateRateRequest(ShipmentPackage);
            RateService service = new RateService(); // Initialize the service
            service.Url = SettingManager.GetSettingValue("ShippingRateComputationMethod.FedEx.URL");
            try
            {
                // This is the call to the web service passing in a RateRequest and returning a RateReply
                RateReply reply = service.getRates(request); // Service call
                //
                if (reply.HighestSeverity == NotificationSeverityType.SUCCESS || reply.HighestSeverity == NotificationSeverityType.NOTE || reply.HighestSeverity == NotificationSeverityType.WARNING) // check if the call was successful
                {
                    if (reply != null && reply.RateReplyDetails != null)
                    {
                        shippingOptions = ParseResponse(reply);
                    }
                    else
                    {
                        Error = "Could not get reply from shipping server";
                    }
                }
                else
                {
                    Debug.WriteLine(reply.Notifications[0].Message);
                    Error = reply.Notifications[0].Message;
                }
            }
            catch (SoapException e)
            {
                Debug.WriteLine(e.Detail.InnerText);
                Error = e.Detail.InnerText;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Error = e.Message;
            }



            if (String.IsNullOrEmpty(Error) && shippingOptions.Count == 0)
                Error = "Shipping options could not be loaded";
            return shippingOptions;
        }

        /// <summary>
        /// Gets fixed shipping rate (if shipping rate computation method allows it and the rate can be calculated before checkout).
        /// </summary>
        /// <param name="ShipmentPackage">Shipment package</param>
        /// <returns>Fixed shipping rate; or null if shipping rate could not be calculated before checkout</returns>
        public decimal? GetFixedRate(ShipmentPackage ShipmentPackage)
        {
            return null;
        }
        #endregion
    }
}
