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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace NopSolutions.NopCommerce.Common.Xml
{
    /// <summary>
    /// Xml helper class
    /// </summary>
    public partial class XmlHelper
    {
        #region Methods
        /// <summary>
        /// XML Encode
        /// </summary>
        /// <param name="s">String</param>
        /// <returns>Encoded string</returns>
        public static string XmlEncode(string s)
        {
            if (s == null)
                return null;
            s = Regex.Replace(s, @"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", "", RegexOptions.Compiled);
            return XmlEncodeAsIs(s);
        }

        /// <summary>
        /// XML Encode as is
        /// </summary>
        /// <param name="s">String</param>
        /// <returns>Encoded string</returns>
        public static string XmlEncodeAsIs(string s)
        {
            if (s == null)
                return null;
            using (StringWriter sw = new StringWriter())
            using (XmlTextWriter xwr = new XmlTextWriter(sw))
            {
                xwr.WriteString(s);
                String sTmp = sw.ToString();
                return sTmp;
            }
        }

        /// <summary>
        /// Encodes an attribute
        /// </summary>
        /// <param name="s">Attribute</param>
        /// <returns>Encoded attribute</returns>
        public static string XmlEncodeAttribute(string s)
        {
            if (s == null)
                return null;
            s = Regex.Replace(s, @"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", "", RegexOptions.Compiled);
            return XmlEncodeAttributeAsIs(s);
        }

        /// <summary>
        /// Encodes an attribute as is
        /// </summary>
        /// <param name="s">Attribute</param>
        /// <returns>Encoded attribute</returns>
        public static string XmlEncodeAttributeAsIs(string s)
        {
            return XmlEncodeAsIs(s).Replace("\"", "&quot;");
        }

        /// <summary>
        /// Decodes an attribute
        /// </summary>
        /// <param name="s">Attribute</param>
        /// <returns>Decoded attribute</returns>
        public static string XmlDecode(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            return sb.Replace("&quot;", "\"").Replace("&apos;", "'").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&").ToString();
        }

        /// <summary>
        /// Serializes a datetime
        /// </summary>
        /// <param name="dateTime">Datetime</param>
        /// <returns>Serialized datetime</returns>
        public static string SerializeDateTime(DateTime dateTime)
        {
            XmlSerializer xmlS = new XmlSerializer(typeof(DateTime));
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                xmlS.Serialize(sw, dateTime);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Deserializes a datetime
        /// </summary>
        /// <param name="dateTime">Datetime</param>
        /// <returns>Deserialized datetime</returns>
        public static DateTime DeserializeDateTime(string dateTime)
        {
            XmlSerializer xmlS = new XmlSerializer(typeof(DateTime));
            StringBuilder sb = new StringBuilder();
            using (StringReader sr = new StringReader(dateTime))
            {
                object test = xmlS.Deserialize(sr);
                return (DateTime)test;
            }
        }
        #endregion
    }
}
