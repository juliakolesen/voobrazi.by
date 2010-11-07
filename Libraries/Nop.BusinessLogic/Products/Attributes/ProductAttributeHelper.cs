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
using System.Text;
using System.Web;
using System.Xml;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;

namespace NopSolutions.NopCommerce.BusinessLogic.Products.Attributes
{
    /// <summary>
    /// Product attribute helper
    /// </summary>
    public class ProductAttributeHelper
    {
        /// <summary>
        /// Gets selected product variant attribute identifiers
        /// </summary>
        /// <param name="Attributes">Attributes</param>
        /// <returns>Selected product variant attribute identifiers</returns>
        public static List<int> ParseProductVariantAttributeIDs(string Attributes)
        {
            List<int> IDs = new List<int>();
            if (String.IsNullOrEmpty(Attributes))
                return IDs;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(Attributes);

                XmlNodeList nodeList1 = xmlDoc.SelectNodes(@"//Attributes/ProductVariantAttribute");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["ID"] != null)
                    {
                        int id = Convert.ToInt32(node1.Attributes["ID"].InnerText.Trim());
                        IDs.Add(id);
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }
            return IDs;
        }

        /// <summary>
        /// Gets selected product variant attributes
        /// </summary>
        /// <param name="Attributes">Attributes</param>
        /// <returns>Selected product variant attributes</returns>
        public static ProductVariantAttributeCollection ParseProductVariantAttributes(string Attributes)
        {
            ProductVariantAttributeCollection pvaCollection = new ProductVariantAttributeCollection();
            List<int> IDs = ParseProductVariantAttributeIDs(Attributes);
            foreach (int id in IDs)
            {
                ProductVariantAttribute pva = ProductAttributeManager.GetProductVariantAttributeByID(id);
                if (pva != null)
                {
                    pvaCollection.Add(pva);
                }
            }
            return pvaCollection;
        }
       
        /// <summary>
        /// Get product variant attribute values
        /// </summary>
        /// <param name="Attributes">Attributes</param>
        /// <returns>Product variant attribute values</returns>
        public static ProductVariantAttributeValueCollection ParseProductVariantAttributeValues(string Attributes)
        {
            ProductVariantAttributeValueCollection pvaValues = new ProductVariantAttributeValueCollection();
            ProductVariantAttributeCollection pvaCollection = ParseProductVariantAttributes(Attributes);
            foreach (ProductVariantAttribute pva in pvaCollection)
            {
                if (!pva.ShouldHaveValues)
                    continue;

                List<string> pvaValuesStr = ParseValues(Attributes, pva.ProductVariantAttributeID);
                foreach (string pvaValueStr in pvaValuesStr)
                {
                    if (!String.IsNullOrEmpty(pvaValueStr))
                    {
                        int pvaValueID = 0;
                        if (int.TryParse(pvaValueStr, out pvaValueID))
                        {
                            ProductVariantAttributeValue pvaValue = ProductAttributeManager.GetProductVariantAttributeValueByID(pvaValueID);
                            if (pvaValue != null)
                                pvaValues.Add(pvaValue);
                        }
                    }
                }
            }
            return pvaValues;
        }
       
        /// <summary>
        /// Gets selected product variant attribute value
        /// </summary>
        /// <param name="Attributes">Attributes</param>
        /// <param name="ProductVariantAttributeID">Product variant attribute identifier</param>
        /// <returns>Product variant attribute value</returns>
        public static List<string> ParseValues(string Attributes, int ProductVariantAttributeID)
        {
            List<string> selectedProductVariantAttributeValues = new List<string>();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(Attributes);

                XmlNodeList nodeList1 = xmlDoc.SelectNodes(@"//Attributes/ProductVariantAttribute");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["ID"] != null)
                    {
                        int id = Convert.ToInt32(node1.Attributes["ID"].InnerText.Trim());
                        if (id == ProductVariantAttributeID)
                        {
                            XmlNodeList nodeList2 = node1.SelectNodes(@"ProductVariantAttributeValue/Value");
                            foreach (XmlNode node2 in nodeList2)
                            {
                                string value = node2.InnerText.Trim();
                                selectedProductVariantAttributeValues.Add(value);
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }
            return selectedProductVariantAttributeValues;
        }
        
        /// <summary>
        /// Adds an attribute
        /// </summary>
        /// <param name="Attributes">Attributes</param>
        /// <param name="pva">Product variant attribute</param>
        /// <param name="value">Value</param>
        /// <returns>Attributes</returns>
        public static string AddAttribute(string Attributes, ProductVariantAttribute pva, string value)
        {
            string result = string.Empty;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                if (String.IsNullOrEmpty(Attributes))
                {
                    XmlElement _element1 = xmlDoc.CreateElement("Attributes");
                    xmlDoc.AppendChild(_element1);
                }
                else
                {
                    xmlDoc.LoadXml(Attributes);
                }
                XmlElement rootElement = (XmlElement)xmlDoc.SelectSingleNode(@"//Attributes");

                XmlElement pvaElement = null;
                //find existing
                XmlNodeList nodeList1 = xmlDoc.SelectNodes(@"//Attributes/ProductVariantAttribute");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["ID"] != null)
                    {
                        int id = Convert.ToInt32(node1.Attributes["ID"].InnerText.Trim());
                        if (id == pva.ProductVariantAttributeID)
                        {
                            pvaElement = (XmlElement)node1;
                            break;
                        }
                    }
                }

                //create new one if not found
                if (pvaElement == null)
                {
                    pvaElement = xmlDoc.CreateElement("ProductVariantAttribute");
                    pvaElement.SetAttribute("ID", pva.ProductVariantAttributeID.ToString());
                    rootElement.AppendChild(pvaElement);
                }

                XmlElement pvavElement = xmlDoc.CreateElement("ProductVariantAttributeValue");
                pvaElement.AppendChild(pvavElement);

                XmlElement pvavVElement = xmlDoc.CreateElement("Value");
                pvavVElement.InnerText = value;
                pvavElement.AppendChild(pvavVElement);

                result = xmlDoc.OuterXml;
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }
            return result;
        }
       
        /// <summary>
        /// Formats attributes
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="Attributes">Attributes</param>
        /// <returns>Attributes</returns>
        public static string FormatAttributes(ProductVariant productVariant, string Attributes)
        {
            Customer customer = NopContext.Current.User;
            return FormatAttributes(productVariant, Attributes, customer, "<br />");
        }

        /// <summary>
        /// Formats attributes
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="Attributes">Attributes</param>
        /// <param name="customer">Customer</param>
        /// <param name="Serapator">Serapator</param>
        /// <returns>Attributes</returns>
        public static string FormatAttributes(ProductVariant productVariant, string Attributes, Customer customer, string Serapator)
        {
            return FormatAttributes(productVariant, Attributes, customer, Serapator, true);
        }

        /// <summary>
        /// Formats attributes
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="Attributes">Attributes</param>
        /// <param name="customer">Customer</param>
        /// <param name="Serapator">Serapator</param>
        /// <param name="HTMLEncode">A value indicating whether to encode (HTML) values</param>
        /// <returns>Attributes</returns>
        public static string FormatAttributes(ProductVariant productVariant, string Attributes, 
            Customer customer, string Serapator, bool HTMLEncode)
        {
            StringBuilder result = new StringBuilder();

            ProductVariantAttributeCollection pvaCollection = ParseProductVariantAttributes(Attributes);
            foreach (ProductVariantAttribute pva in pvaCollection)
            {
                List<string> valuesStr = ParseValues(Attributes, pva.ProductVariantAttributeID);
                foreach (string valueStr in valuesStr)
                {
                    string pvaAttribute = string.Empty;
                    if (!pva.ShouldHaveValues)
                    {
                        pvaAttribute = string.Format("{0}: {1}", pva.ProductAttribute.Name, valueStr);
                    }
                    else
                    {
                        ProductVariantAttributeValue pvaValue = ProductAttributeManager.GetProductVariantAttributeValueByID(Convert.ToInt32(valueStr));
                        if (pvaValue != null)
                        {
                            pvaAttribute = string.Format("{0}: {1}", pva.ProductAttribute.Name, pvaValue.Name);
                            decimal priceAdjustmentBase = TaxManager.GetPrice(productVariant, pvaValue.PriceAdjustment, customer);
                            decimal priceAdjustment = CurrencyManager.ConvertCurrency(priceAdjustmentBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
                            if (priceAdjustmentBase > 0)
                            {
                                string priceAdjustmentStr = PriceHelper.FormatPrice(priceAdjustment, false, false);
                                pvaAttribute += string.Format(" [+{0}]", priceAdjustmentStr);
                            }
                        }
                    }

                    if (!String.IsNullOrEmpty(pvaAttribute))
                    {
                        result.Append(Serapator);
                        if (HTMLEncode)
                        {
                            result.Append(HttpUtility.HtmlEncode(pvaAttribute));
                        }
                        else
                        {
                            result.Append(pvaAttribute);
                        }
                    }
                }
            }
            return result.ToString();
        }
    }
}
