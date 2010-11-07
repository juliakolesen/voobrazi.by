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

namespace NopSolutions.NopCommerce.Common.Utils.Html
{
    /// <summary>
    /// Represents a HTML helper
    /// </summary>
    public partial class HtmlHelper
    {
        #region Fields
        private static Regex paragraphStartRegex = new Regex("<p>", RegexOptions.IgnoreCase);
        private static Regex paragraphEndRegex = new Regex("</p>", RegexOptions.IgnoreCase);
        private static Regex ampRegex = new Regex("&(?!(?:#[0-9]{2,4};|[a-z0-9]+;))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #endregion

        #region Utilities

        private static string EnsureOnlyAllowedHtml(string Text)
        {
            if (String.IsNullOrEmpty(Text))
                return string.Empty;

            string allowedTags = "br,hr,b,i,u,a,div,ol,ul,li,blockquote,img,span,p,em,strong,font,pre,h1,h2,h3,h4,h5,h6,address";

            RegexOptions options = RegexOptions.IgnoreCase;
            MatchCollection m = Regex.Matches(Text, "<.*?>", options);
            for (int i = m.Count - 1; i >= 0; i--)
            {
                string tag = Text.Substring(m[i].Index + 1, m[i].Length - 1).Trim().ToLower();

                if (!IsValidTag(tag, allowedTags))
                {
                    Text = Text.Remove(m[i].Index, m[i].Length);
                }
            }

            return Text;
        }

        private static bool IsValidTag(string tag, string tags)
        {
            string[] allowedTags = tags.Split(',');
            if (tag.IndexOf("javascript") >= 0) return false;
            if (tag.IndexOf("vbscript") >= 0) return false;
            if (tag.IndexOf("onclick") >= 0) return false;

            char[] endchars = new char[] { ' ', '>', '/', '\t' };

            int pos = tag.IndexOfAny(endchars, 1);
            if (pos > 0) tag = tag.Substring(0, pos);
            if (tag[0] == '/') tag = tag.Substring(1);

            foreach (string aTag in allowedTags)
            {
                if (tag == aTag) return true;
            }

            return false;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Formats the text
        /// </summary>
        /// <param name="Text">Text</param>
        /// <param name="StripTags">A value indicating whether to strip tags</param>
        /// <param name="ConvertPlainTextToHtml">A value indicating whether HTML is allowed</param>
        /// <param name="AllowHtml">A value indicating whether HTML is allowed</param>
        /// <param name="AllowBBCode">A value indicating whether BBCode is allowed</param>
        /// <param name="ResolveLinks">A value indicating whether to resolve links</param>
        /// <param name="AddNoFollowTag">A value indicating whether to add "noFollow" tag</param>
        /// <returns>Formatted text</returns>
        public static string FormatText(string Text, bool StripTags,
            bool ConvertPlainTextToHtml, bool AllowHtml, 
            bool AllowBBCode, bool ResolveLinks, bool AddNoFollowTag)
        {

            if (String.IsNullOrEmpty(Text))
                return string.Empty;

            try
            {
                if (StripTags)
                {
                    Text = HtmlHelper.StripTags(Text);
                }

                if (AllowHtml)
                {
                    Text = HtmlHelper.EnsureOnlyAllowedHtml(Text);
                }
                else
                {
                    Text = HttpUtility.HtmlEncode(Text);
                }

                if (ConvertPlainTextToHtml)
                {
                    Text = HtmlHelper.ConvertPlainTextToHtml(Text);
                }

                if (AllowBBCode)
                {
                    Text = BBCodeHelper.FormatText(Text, true, true, true, true, true);
                }

                if (ResolveLinks)
                {
                    Text = ResolveLinksHelper.FormatText(Text);
                }

                if (AddNoFollowTag)
                {
                    //add noFollow tag. not implemented
                }
            }
            catch (Exception exc)
            {
                Text = string.Format("Text cannot be formatted. Error: {0}", exc.Message);
            }
            return Text;
        }
        
        /// <summary>
        /// Strips tags
        /// </summary>
        /// <param name="Text">Text</param>
        /// <returns>Formatted text</returns>
        public static string StripTags(string Text)
        {
            if (String.IsNullOrEmpty(Text))
                return string.Empty;

            //return Regex.Replace(Text, @"<(.|\n)*?>", string.Empty);
            
            Text = Regex.Replace(Text, @"(>)(\r|\n)*(<)", "><");
            Text = Regex.Replace(Text, "(<[^>]*>)([^<]*)", "$2");
            Text = Regex.Replace(Text, "(&#x?[0-9]{2,4};|&quot;|&amp;|&nbsp;|&lt;|&gt;|&euro;|&copy;|&reg;|&permil;|&Dagger;|&dagger;|&lsaquo;|&rsaquo;|&bdquo;|&rdquo;|&ldquo;|&sbquo;|&rsquo;|&lsquo;|&mdash;|&ndash;|&rlm;|&lrm;|&zwj;|&zwnj;|&thinsp;|&emsp;|&ensp;|&tilde;|&circ;|&Yuml;|&scaron;|&Scaron;)", "@");

            return Text;
        }

        /// <summary>
        /// Converts plain text to HTML
        /// </summary>
        /// <param name="Text">Text</param>
        /// <returns>Formatted text</returns>
        public static string ConvertPlainTextToHtml(string Text)
        {
            if (String.IsNullOrEmpty(Text))
                return string.Empty;

            Text = Text.Replace("\r\n", "<br />");
            Text = Text.Replace("\r", "<br />");
            Text = Text.Replace("\n", "<br />");
            Text = Text.Replace("\t", "&nbsp;&nbsp;");
            Text = Text.Replace("  ", "&nbsp;&nbsp;");

            return Text;
        }

        /// <summary>
        /// Converts HTML to plain text
        /// </summary>
        /// <param name="Text">Text</param>
        /// <returns>Formatted text</returns>
        public static string ConvertHtmlToPlainText(string Text)
        {
            if (String.IsNullOrEmpty(Text))
                return string.Empty;

            Text = Text.Replace("<br>", "\n");
            Text = Text.Replace("<br >", "\n");
            Text = Text.Replace("<br />", "\n");
            Text = Text.Replace("&nbsp;&nbsp;", "\t");
            Text = Text.Replace("&nbsp;&nbsp;", "  ");

            return Text;
        }

        /// <summary>
        /// Converts text to paragraph
        /// </summary>
        /// <param name="Text">Text</param>
        /// <returns>Formatted text</returns>
        public static string ConvertPlainTextToParagraph(string Text)
        {
            if (String.IsNullOrEmpty(Text))
                return string.Empty;

            Text = paragraphStartRegex.Replace(Text, string.Empty);
            Text = paragraphEndRegex.Replace(Text, "\n");
            Text = Text.Replace("\r\n", "\n").Replace("\r", "\n");
            Text = Text + "\n\n";
            Text = Text.Replace("\n\n", "\n");
            string[] strArray = Text.Split(new char[] { '\n' });
            StringBuilder builder = new StringBuilder();
            foreach (string str in strArray)
            {
                if ((str != null) && (str.Trim().Length > 0))
                {
                    builder.AppendFormat("<p>{0}</p>\n", str);
                }
            }
            return builder.ToString();
        }
        
        #endregion
    }
}
