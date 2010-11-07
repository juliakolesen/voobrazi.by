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
// Contributor(s): RJH 08/07/2009. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Measures;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.Common;

namespace NopSolutions.NopCommerce.Shipping.Methods.USPS
{
    /// <summary>
    /// US Postal Service computation method
    /// </summary>
    public class USPSComputationMethod : IShippingRateComputationMethod
    {
        #region Const

        private const int MAXPACKAGEWEIGHT = 70;
        #endregion
        #region Utilities

        private string CreateRequest(string Username, string Password, ShipmentPackage ShipmentPackage)
        {
            int length = Convert.ToInt32(Math.Ceiling(ShipmentPackage.GetTotalLength()));
            int height = Convert.ToInt32(Math.Ceiling(ShipmentPackage.GetTotalHeight()));
            int width = Convert.ToInt32(Math.Ceiling(ShipmentPackage.GetTotalWidth()));
            decimal weight = ShippingManager.GetShoppingCartTotalWeigth(ShipmentPackage.Items);
            string zipPostalCodeFrom = ShipmentPackage.ZipPostalCodeFrom;
            string zipPostalCodeTo = ShipmentPackage.ShippingAddress.ZipPostalCode;

            //valid values for testing. http://testing.shippingapis.com/ShippingAPITest.dll
            //Zip to = "20008"; Zip from ="10022"; weight = 2;

            //TODO convert measure weight
            MeasureWeight baseWeightIn = MeasureManager.BaseWeightIn;
            if (baseWeightIn.SystemKeyword != "lb")
                throw new NopException("USPS shipping service. Base weight should be set to lb(s)");

            //TODO convert measure dimension
            MeasureDimension baseDimensionIn = MeasureManager.BaseDimensionIn;
            if (baseDimensionIn.SystemKeyword != "inches")
                throw new NopException("USPS shipping service. Base dimension should be set to inch(es)");

            int pounds = Convert.ToInt32(Math.Truncate(weight));
            //int ounces = Convert.ToInt32((weight - pounds) * 16.0M);
            int ounces = 0;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<RateV3Request USERID=\"{0}\" PASSWORD=\"{1}\">", Username, Password);

            USPSStrings xmlStrings = new USPSStrings(); // Create new instance with string array


            if ((!IsPackageTooHeavy(pounds)) && (!IsPackageTooLarge(length, height, width)))
            {
                USPSPackageSize packageSize = GetPackageSize(length, height, width);
                // RJH get all XML strings not commented out for USPSStrings. 
                // RJH V3 USPS Service must be Express, Express SH, Express Commercial, Express SH Commercial, First Class, Priority, Priority Commercial, Parcel, Library, BPM, Media, ALL or ONLINE;
                foreach (string element in xmlStrings.Elements) // Loop over elements with property
                {
                    sb.Append("<Package ID=\"0\">");

                    // sb.AppendFormat("<Service>{0}</Service>", USPSService.All);
                    sb.AppendFormat("<Service>{0}</Service>", element);
                    sb.AppendFormat("<ZipOrigination>{0}</ZipOrigination>", zipPostalCodeFrom);
                    sb.AppendFormat("<ZipDestination>{0}</ZipDestination>", zipPostalCodeTo);
                    sb.AppendFormat("<Pounds>{0}</Pounds>", pounds);
                    sb.AppendFormat("<Ounces>{0}</Ounces>", ounces);
                    sb.AppendFormat("<Size>{0}</Size>", packageSize);
                    sb.Append("<Machinable>FALSE</Machinable>");

                    sb.Append("</Package>");
                }
            }
            else
            {
                int totalPackages = 1;
                int totalPackagesDims = 1;
                int totalPackagesWeights = 1;
                if (IsPackageTooHeavy(pounds))
                {
                    totalPackagesWeights = Convert.ToInt32(Math.Ceiling((decimal)pounds / (decimal)MAXPACKAGEWEIGHT));
                }
                if (IsPackageTooLarge(length, height, width))
                {
                    totalPackagesDims = TotalPackageSize(length, height, width)/108;
                }
                totalPackages = totalPackagesDims > totalPackagesWeights ? totalPackagesDims : totalPackagesWeights;
                if (totalPackages == 0)
                    totalPackages = 1;

                int pounds2 = pounds / totalPackages;
                int ounces2 = ounces / totalPackages;
                int height2 = height / totalPackages;
                int width2 = width / totalPackages;
                USPSPackageSize packageSize = GetPackageSize(length, height2, width2);
                
                for (int i = 0; i < totalPackages; i++)
                {
                    foreach (string element in xmlStrings.Elements)
                    {
                        sb.AppendFormat("<Package ID=\"{0}\">", i.ToString());
                        // sb.AppendFormat("<Service>{0}</Service>", USPSService.All);
                        sb.AppendFormat("<Service>{0}</Service>", element);
                        sb.AppendFormat("<ZipOrigination>{0}</ZipOrigination>", zipPostalCodeFrom);
                        sb.AppendFormat("<ZipDestination>{0}</ZipDestination>", zipPostalCodeTo);
                        sb.AppendFormat("<Pounds>{0}</Pounds>", pounds2);
                        sb.AppendFormat("<Ounces>{0}</Ounces>", ounces2);
                        sb.AppendFormat("<Size>{0}</Size>", packageSize);
                        sb.Append("<Machinable>FALSE</Machinable>");
                        sb.Append("</Package>");
                    }
                }
            }

            sb.Append("</RateV3Request>");

            string requestString = "API=RateV3&XML=" + sb.ToString();

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

        private bool IsPackageTooLarge(int length, int height, int width)
        {
            int total = TotalPackageSize(length, height, width);
            if (total > 130)
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

        private USPSPackageSize GetPackageSize(int length, int height, int width)
        {
            int girth = height + height + width + width;
            int total = girth + length;
            if (total <= 84)
                return USPSPackageSize.Regular;
            if ((total > 84) && (total <= 108))
                return USPSPackageSize.Large;
            if ((total > 108) && (total <= 130))
                return USPSPackageSize.Oversize;
            else
                throw new NopException("Shipping Error: Package too large.");
        }

        private ShippingOptionCollection ParseResponse(string response, ref string error)
        {
            ShippingOptionCollection shippingOptions = new ShippingOptionCollection();

            using (StringReader sr = new StringReader(response))
            using (XmlTextReader tr = new XmlTextReader(sr))


                do
                // while (tr.Read())
                {
                    // Read the next XML record
                    tr.Read();

                    if ((tr.Name == "Error") && (tr.NodeType == XmlNodeType.Element))
                    {
                        string errorText = "";
                        while (tr.Read())
                        {
                            if ((tr.Name == "Description") && (tr.NodeType == XmlNodeType.Element))
                                errorText += "Error Desc: " + tr.ReadString();
                            if ((tr.Name == "HelpContext") && (tr.NodeType == XmlNodeType.Element))
                                errorText += "USPS Help Context: " + tr.ReadString() + ". ";
                        }
                        error = "USPS Error returned: " + errorText;
                    }

                    // Process the inner postage XML
                    if ((tr.Name == "Postage") && (tr.NodeType == XmlNodeType.Element))
                    {
                        string serviceCode = "";
                        string postalRate = "";

                        // while (tr.Read())
                        do
                        {

                            tr.Read();

                            if ((tr.Name == "MailService") && (tr.NodeType == XmlNodeType.Element))
                            {
                                serviceCode = tr.ReadString();

                                tr.ReadEndElement();
                                if ((tr.Name == "MailService") && (tr.NodeType == XmlNodeType.EndElement))
                                    break;
                            }
                            // if (((tr.Name == "Postage") && (tr.NodeType == XmlNodeType.EndElement)) || ((tr.Name == "Postage") && (tr.NodeType == XmlNodeType.Element)))
                            //   break;

                            if ((tr.Name == "Rate") && (tr.NodeType == XmlNodeType.Element))
                            {
                                postalRate = tr.ReadString();
                                tr.ReadEndElement();
                                if ((tr.Name == "Rate") && (tr.NodeType == XmlNodeType.EndElement))
                                    break;
                            }

                        } while (!((tr.Name == "Postage") && (tr.NodeType == XmlNodeType.EndElement)));

                        if (shippingOptions.Find((s) => s.Name == serviceCode) == null)
                        {
                            ShippingOption shippingOption = new ShippingOption();
                            //TODO check whether we need to multiply rate by package quantity
                            shippingOption.Rate = Convert.ToDecimal(postalRate, new CultureInfo("en-US"));
                            shippingOption.Name = serviceCode;
                            shippingOptions.Add(shippingOption);
                        }
                    }
                } while (!tr.EOF);
            return shippingOptions;
        }

        #endregion

        #region Methods
        /// <summary>
        ///  Gets available shipping options
        /// </summary>
        /// <param name="ShipmentPackage">Shipment option</param>
        /// <param name="Error">Error</param>
        /// <returns>Shipping options</returns>
        public ShippingOptionCollection GetShippingOptions(ShipmentPackage ShipmentPackage, ref string Error)
        {
            ShippingOptionCollection shippingOptions = new ShippingOptionCollection();

            if (ShipmentPackage == null)
                throw new ArgumentNullException("ShipmentPackage");
            if (ShipmentPackage.Items == null)
                throw new NopSolutions.NopCommerce.Common.NopException("No shipment items");
            if (ShipmentPackage.ShippingAddress == null)
            {
                Error = "Shipping address is not set";
                return shippingOptions;
            }

            string url = SettingManager.GetSettingValue("ShippingRateComputationMethod.USPS.URL");
            string username = SettingManager.GetSettingValue("ShippingRateComputationMethod.USPS.Username");
            string password = SettingManager.GetSettingValue("ShippingRateComputationMethod.USPS.Password");
            decimal additionalHandlingCharge = SettingManager.GetSettingValueDecimalNative("ShippingRateComputationMethod.USPS.AdditionalHandlingCharge");
            ShipmentPackage.ZipPostalCodeFrom = SettingManager.GetSettingValue("ShippingRateComputationMethod.USPS.DefaultShippedFromZipPostalCode");
            string requestString = CreateRequest(username, password, ShipmentPackage);
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
