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
// Contributor(s): Danny Battison (gabehabe@googlemail.com). 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using NopSolutions.NopCommerce.Common.Utils.Html.CodeFormatter;
using System.Data;

namespace NopSolutions.NopCommerce.Common.Utils.Html
{
    /// <summary>
    /// Represents a BBCode helper
    /// </summary>
    public partial class BBCodeHelper
    {
        #region Fields
        private static readonly Regex regexBold = new Regex(@"\[b\](.+?)\[/b\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexItalic = new Regex(@"\[i\](.+?)\[/i\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexUnderLine = new Regex(@"\[u\](.+?)\[/u\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexUrl1 = new Regex(@"\[url\=([^\]]+)\]([^\]]+)\[/url\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexUrl2 = new Regex(@"\[url\](.+?)\[/url\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion

        #region Methods
        /// <summary>
        /// Formats the text
        /// </summary>
        /// <param name="str">Text</param>
        /// <returns>Formatted text</returns>
        public static string FormatText(string Text, bool ReplaceBold, bool ReplaceItalic,
            bool ReplaceUnderLine, bool ReplaceUrl, bool ReplaceCode)
        {
            if (String.IsNullOrEmpty(Text))
                return string.Empty;

            if (ReplaceBold)
            {
                // format the bold tags: [b][/b]
                // becomes: <strong></strong>
                Text = regexBold.Replace(Text, "<strong>$1</strong>");
            }

            if (ReplaceItalic)
            {
                // format the italic tags: [i][/i]
                // becomes: <em></em>
                Text = regexItalic.Replace(Text, "<em>$1</em>");
            }

            if (ReplaceUnderLine)
            {
                // format the underline tags: [u][/u]
                // becomes: <u></u>
                Text = regexUnderLine.Replace(Text, "<u>$1</u>");
            }

            if (ReplaceUrl)
            {
                // format the url tags: [url=http://www.nopCommerce.com]my site[/url]
                // becomes: <a href="http://www.nopCommerce.com">my site</a>
                Text = regexUrl1.Replace(Text, "<a href=\"$1\" rel=\"nofollow\">$2</a>");

                // format the url tags: [url]http://www.nopCommerce.com[/url]
                // becomes: <a href="http://www.nopCommerce.com">http://www.nopCommerce.com</a>
                Text = regexUrl2.Replace(Text, "<a href=\"$1\" rel=\"nofollow\">$1</a>");
            }

            if (ReplaceCode)
            {
                //Text = CodeFormatHelper.FormatText(Text);
                Text = CodeFormatHelper.FormatTextSimple(Text);
            }

            return Text;
        }

        #endregion
    }
}
