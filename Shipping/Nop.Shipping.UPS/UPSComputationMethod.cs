﻿//------------------------------------------------------------------------------
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
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Measures;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.Common;

namespace NopSolutions.NopCommerce.Shipping.Methods.UPS
{
    /// <summary>
    /// UPS computation method
    /// </summary>
    public class UPSComputationMethod : IShippingRateComputationMethod
    {
        #region Const

        private const int MAXPACKAGEWEIGHT = 150;
        #endregion

        #region Utilities
        private string CreateRequest(string AccessKey, string Username, string Password,
            ShipmentPackage ShipmentPackage, UPSCustomerClassification customerClassification,
            UPSPickupType pickupType, UPSPackagingType packagingType)
        {
            int length = Convert.ToInt32(Math.Ceiling(ShipmentPackage.GetTotalLength()));
            int height = Convert.ToInt32(Math.Ceiling(ShipmentPackage.GetTotalHeight()));
            int width = Convert.ToInt32(Math.Ceiling(ShipmentPackage.GetTotalWidth()));
            int weight = Convert.ToInt32(Math.Ceiling(ShippingManager.GetShoppingCartTotalWeigth(ShipmentPackage.Items)));
            string zipPostalCodeFrom = ShipmentPackage.ZipPostalCodeFrom;
            string zipPostalCodeTo = ShipmentPackage.ShippingAddress.ZipPostalCode;
            string countryCodeFrom = ShipmentPackage.CountryFrom.TwoLetterISOCode;
            string countryCodeTo = ShipmentPackage.ShippingAddress.Country.TwoLetterISOCode;

            //TODO convert measure weight
            MeasureWeight baseWeightIn = MeasureManager.BaseWeightIn;
            if (baseWeightIn.SystemKeyword != "lb")
                throw new NopException("UPS shipping service. Base weight should be set to lb(s)");

            //TODO convert measure dimension
            MeasureDimension baseDimensionIn = MeasureManager.BaseDimensionIn;
            if (baseDimensionIn.SystemKeyword != "inches")
                throw new NopException("UPS shipping service. Base dimension should be set to inch(es)");

            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version='1.0'?>");
            sb.Append("<AccessRequest xml:lang='en-US'>");
            sb.AppendFormat("<AccessLicenseNumber>{0}</AccessLicenseNumber>", AccessKey);
            sb.AppendFormat("<UserId>{0}</UserId>", Username);
            sb.AppendFormat("<Password>{0}</Password>", Password);
            sb.Append("</AccessRequest>");
            sb.Append("<?xml version='1.0'?>");
            sb.Append("<RatingServiceSelectionRequest xml:lang='en-US'>");
            sb.Append("<Request>");
            sb.Append("<TransactionReference>");
            sb.Append("<CustomerContext>Bare Bones Rate Request</CustomerContext>");
            sb.Append("<XpciVersion>1.0001</XpciVersion>");
            sb.Append("</TransactionReference>");
            sb.Append("<RequestAction>Rate</RequestAction>");
            sb.Append("<RequestOption>Shop</RequestOption>");
            sb.Append("</Request>");
            sb.Append("<PickupType>");
            sb.AppendFormat("<Code>{0}</Code>", GetPickupTypeCode(pickupType));
            sb.Append("</PickupType>");
            sb.Append("<CustomerClassification>");
            sb.AppendFormat("<Code>{0}</Code>", GetCustomerClassificationCode(customerClassification));
            sb.Append("</CustomerClassification>");
            sb.Append("<Shipment>");
            sb.Append("<Shipper>");
            sb.Append("<Address>");
            sb.AppendFormat("<PostalCode>{0}</PostalCode>", zipPostalCodeFrom);
            sb.AppendFormat("<CountryCode>{0}</CountryCode>", countryCodeFrom);
            sb.Append("</Address>");
            sb.Append("</Shipper>");
            sb.Append("<ShipTo>");
            sb.Append("<Address>");
            sb.Append("<ResidentialAddressIndicator/>");
            sb.AppendFormat("<PostalCode>{0}</PostalCode>", zipPostalCodeTo);
            sb.AppendFormat("<CountryCode>{0}</CountryCode>", countryCodeTo);
            sb.Append("</Address>");
            sb.Append("</ShipTo>");
            sb.Append("<ShipFrom>");
            sb.Append("<Address>");
            sb.AppendFormat("<PostalCode>{0}</PostalCode>", zipPostalCodeFrom);
            sb.AppendFormat("<CountryCode>{0}</CountryCode>", countryCodeFrom);
            sb.Append("</Address>");
            sb.Append("</ShipFrom>");
            sb.Append("<Service>");
            //UNDONE set correct service code
            sb.Append("<Code>03</Code>");
            sb.Append("</Service>");


            if ((!IsPackageTooHeavy(weight)) && (!IsPackageTooLarge(length, height, width)))
            {
                sb.Append("<Package>");
                sb.Append("<PackagingType>");
                sb.AppendFormat("<Code>{0}</Code>", GetPackagingTypeCode(packagingType));
                sb.Append("</PackagingType>");
                sb.Append("<Dimensions>");
                sb.AppendFormat("<Length>{0}</Length>", length);
                sb.AppendFormat("<Width>{0}</Width>", width);
                sb.AppendFormat("<Height>{0}</Height>", height);
                sb.Append("</Dimensions>");
                sb.Append("<PackageWeight>");
                sb.AppendFormat("<Weight>{0}</Weight>", weight);
                sb.Append("</PackageWeight>");
                sb.Append("</Package>");
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
                    totalPackagesDims = TotalPackageSize(length, height, width)/108;
                }
                totalPackages = totalPackagesDims > totalPackagesWeights ? totalPackagesDims : totalPackagesWeights;
                if (totalPackages == 0)
                    totalPackages = 1;

                int weight2 = weight/totalPackages;
                int height2 = height/totalPackages;
                int width2 = width/totalPackages;

                for (int i = 0; i < totalPackages; i++)
                {
                    sb.Append("<Package>");
                    sb.Append("<PackagingType>");
                    sb.AppendFormat("<Code>{0}</Code>", GetPackagingTypeCode(packagingType));
                    sb.Append("</PackagingType>");
                    sb.Append("<Dimensions>");
                    sb.AppendFormat("<Length>{0}</Length>", length);
                    sb.AppendFormat("<Width>{0}</Width>", width2);
                    sb.AppendFormat("<Height>{0}</Height>", height2);
                    sb.Append("</Dimensions>");
                    sb.Append("<PackageWeight>");
                    sb.AppendFormat("<Weight>{0}</Weight>", weight2);
                    sb.Append("</PackageWeight>");
                    sb.Append("</Package>");
                }
            }


            sb.Append("</Shipment>");
            sb.Append("</RatingServiceSelectionRequest>");
            string requestString = sb.ToString();
            return requestString;
        }

        private string DoRequest(string URL, string RequestString)
        {
            byte[] bytes = new ASCIIEncoding().GetBytes(RequestString);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            WebResponse response = request.GetResponse();
            string responseXML = string.Empty;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                responseXML = reader.ReadToEnd();

            return responseXML;
        }

        private string GetCustomerClassificationCode(UPSCustomerClassification CustomerClassification)
        {
            switch (CustomerClassification)
            {
                case UPSCustomerClassification.Wholesale:
                    return "01";
                case UPSCustomerClassification.Occasional:
                    return "03";
                case UPSCustomerClassification.Retail:
                    return "04";
                default:
                    throw new NopException("Unknown UPS customer classification code");
            }
        }

        private string GetPackagingTypeCode(UPSPackagingType PackagingType)
        {
            switch (PackagingType)
            {
                case UPSPackagingType.Letter:
                    return "01";
                case UPSPackagingType.CustomerSuppliedPackage:
                    return "02";
                case UPSPackagingType.Tube:
                    return "03";
                case UPSPackagingType.PAK:
                    return "04";
                case UPSPackagingType.ExpressBox:
                    return "21";
                case UPSPackagingType._10KgBox:
                    return "25";
                case UPSPackagingType._25KgBox:
                    return "24";
                default:
                    throw new NopException("Unknown UPS packaging type code");
            }
        }

        private string GetPickupTypeCode(UPSPickupType PickupType)
        {
            switch (PickupType)
            {
                case UPSPickupType.DailyPickup:
                    return "01";
                case UPSPickupType.CustomerCounter:
                    return "03";
                case UPSPickupType.OneTimePickup:
                    return "06";
                case UPSPickupType.OnCallAir:
                    return "07";
                case UPSPickupType.SuggestedRetailRates:
                    return "11";
                case UPSPickupType.LetterCenter:
                    return "19";
                case UPSPickupType.AirServiceCenter:
                    return "20";
                default:
                    throw new NopException("Unknown UPS pickup type code");
            }
        }

        private string GetServiceName(string serviceID)
        {
            switch (serviceID)
            {
                case "01":
                    return "UPS NextDay Air";
                case "02":
                    return "UPS 2nd Day Air";
                case "03":
                    return "UPS Ground";
                case "07":
                    return "UPS Worldwide Express";
                case "08":
                    return "UPS Worldwide Expidited";
                case "11":
                    return "UPS Standard";
                case "12":
                    return "UPS 3 Day Select";
                case "13":
                    return "UPS Next Day Air Saver";
                case "14":
                    return "UPS Next Day Air Early A.M.";
                case "54":
                    return "UPS Worldwide Express Plus";
                case "59":
                    return "UPS 2nd Day Air A.M.";
                default:
                    return "Unknown";
            }
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

        private ShippingOptionCollection ParseResponse(string response, ref string error)
        {
            ShippingOptionCollection shippingOptions = new ShippingOptionCollection();

            using (StringReader sr = new StringReader(response))
            using (XmlTextReader tr = new XmlTextReader(sr))
                while (tr.Read())
                {
                    if ((tr.Name == "Error") && (tr.NodeType == XmlNodeType.Element))
                    {
                        string errorText = "";
                        while (tr.Read())
                        {
                            if ((tr.Name == "ErrorCode") && (tr.NodeType == XmlNodeType.Element))
                            {
                                errorText += "UPS Rating Error, Error Code: " + tr.ReadString() + ", ";
                            }
                            if ((tr.Name == "ErrorDescription") && (tr.NodeType == XmlNodeType.Element))
                            {
                                errorText += "Error Desc: " + tr.ReadString();
                            }
                        }
                        error = "UPS Error returned: " + errorText;
                    }
                    if ((tr.Name == "RatedShipment") && (tr.NodeType == XmlNodeType.Element))
                    {
                        string serviceCode = "";
                        string monetaryValue = "";
                        while (tr.Read())
                        {
                            if ((tr.Name == "Service") && (tr.NodeType == XmlNodeType.Element))
                            {
                                while (tr.Read())
                                {
                                    if ((tr.Name == "Code") && (tr.NodeType == XmlNodeType.Element))
                                    {
                                        serviceCode = tr.ReadString();
                                        tr.ReadEndElement();
                                    }
                                    if ((tr.Name == "Service") && (tr.NodeType == XmlNodeType.EndElement))
                                    {
                                        break;
                                    }
                                }
                            }
                            if (((tr.Name == "RatedShipment") && (tr.NodeType == XmlNodeType.EndElement)) || ((tr.Name == "RatedPackage") && (tr.NodeType == XmlNodeType.Element)))
                            {
                                break;
                            }
                            if ((tr.Name == "TotalCharges") && (tr.NodeType == XmlNodeType.Element))
                            {
                                while (tr.Read())
                                {
                                    if ((tr.Name == "MonetaryValue") && (tr.NodeType == XmlNodeType.Element))
                                    {
                                        monetaryValue = tr.ReadString();
                                        tr.ReadEndElement();
                                    }
                                    if ((tr.Name == "TotalCharges") && (tr.NodeType == XmlNodeType.EndElement))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        string service = GetServiceName(serviceCode);

                        //Weed out unwanted or unkown service rates
                        if (service.ToUpper() != "UNKNOWN")
                        {
                            ShippingOption shippingOption = new ShippingOption();
                            shippingOption.Rate = Convert.ToDecimal(monetaryValue, new CultureInfo("en-US"));
                            shippingOption.Name = service;
                            shippingOptions.Add(shippingOption);
                        }

                    }
                }

            return shippingOptions;
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

            string url = SettingManager.GetSettingValue("ShippingRateComputationMethod.UPS.URL");
            string accessKey = SettingManager.GetSettingValue("ShippingRateComputationMethod.UPS.AccessKey");
            string username = SettingManager.GetSettingValue("ShippingRateComputationMethod.UPS.Username");
            string password = SettingManager.GetSettingValue("ShippingRateComputationMethod.UPS.Password");
            UPSCustomerClassification customerClassification = (UPSCustomerClassification)Enum.Parse(typeof(UPSCustomerClassification), SettingManager.GetSettingValue("ShippingRateComputationMethod.UPS.CustomerClassification"));
            UPSPickupType pickupType = (UPSPickupType)Enum.Parse(typeof(UPSPickupType), SettingManager.GetSettingValue("ShippingRateComputationMethod.UPS.PickupType"));
            UPSPackagingType packagingType = (UPSPackagingType)Enum.Parse(typeof(UPSPackagingType), SettingManager.GetSettingValue("ShippingRateComputationMethod.UPS.PackagingType"));
            decimal additionalHandlingCharge = SettingManager.GetSettingValueDecimalNative("ShippingRateComputationMethod.UPS.AdditionalHandlingCharge");
            if (ShipmentPackage.CountryFrom == null)
            {
                int defaultShippedFromCountryID = SettingManager.GetSettingValueInteger("ShippingRateComputationMethod.UPS.DefaultShippedFromCountryID");
                ShipmentPackage.CountryFrom = CountryManager.GetCountryByID(defaultShippedFromCountryID);
            }
            if (String.IsNullOrEmpty(ShipmentPackage.ZipPostalCodeFrom))
                ShipmentPackage.ZipPostalCodeFrom = SettingManager.GetSettingValue("ShippingRateComputationMethod.UPS.DefaultShippedFromZipPostalCode");
            
            string requestString = CreateRequest(accessKey, username, password, ShipmentPackage,
                customerClassification, pickupType, packagingType);
            string responseXML = DoRequest(url, requestString);
            shippingOptions = ParseResponse(responseXML, ref Error);
            foreach (ShippingOption shippingOption in shippingOptions)
                shippingOption.Rate += additionalHandlingCharge;
           
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
