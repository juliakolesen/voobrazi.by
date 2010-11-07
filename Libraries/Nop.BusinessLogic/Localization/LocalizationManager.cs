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
using System.Globalization;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.Common;
 

namespace NopSolutions.NopCommerce.BusinessLogic.Localization
{
    /// <summary>
    /// Provides information about localization
    /// </summary>
    public partial class LocalizationManager
    {
        #region Methods
        /// <summary>
        /// Gets currency string
        /// </summary>
        /// <param name="Amount">Amount</param>
        /// <returns>Currency string without exchange rate</returns>
        public static string GetCurrencyString(decimal Amount)
        {
            bool ShowCurrency = true;
            Currency TargetCurrency = NopContext.Current.WorkingCurrency;
            return GetCurrencyString(Amount, ShowCurrency, TargetCurrency);
        }

        /// <summary>
        /// Gets currency string
        /// </summary>
        /// <param name="Amount">Amount</param>
        /// <param name="ShowCurrency">A value indicating whether to show a currency</param>
        /// <param name="TargetCurrency">Target currency</param>
        /// <returns>Currency string without exchange rate</returns>
        public static string GetCurrencyString(decimal Amount, bool ShowCurrency, Currency TargetCurrency)
        {
            string result = string.Empty;
            if (!String.IsNullOrEmpty(TargetCurrency.CustomFormatting))
            {
                result = Amount.ToString(TargetCurrency.CustomFormatting);
            }
            else
            {
                if (!String.IsNullOrEmpty(TargetCurrency.DisplayLocale))
                {
                    result = Amount.ToString("C", new CultureInfo(TargetCurrency.DisplayLocale));
                }
                else
                {
                    result = String.Format("{0} ({1})", Amount.ToString("N"), TargetCurrency.CurrencyCode);
                    return result;
                }
            }

            if (ShowCurrency && CurrencyManager.GetAllCurrencies().Count > 1)
                result = String.Format("{0} ({1})", result, TargetCurrency.CurrencyCode);
            return result;
        }

        /// <summary>
        /// Gets a resource string based on the specified ResourceKey property.
        /// </summary>
        /// <param name="ResourceKey">A string representing a ResourceKey.</param>
        /// <returns>A string representing the requested resource string.</returns>
        public static string GetLocaleResourceString(string ResourceKey)
        {
            Language language = NopContext.Current.WorkingLanguage;
            return GetLocaleResourceString(ResourceKey, language.LanguageID);
        }

        /// <summary>
        /// Gets a resource string based on the specified ResourceKey property.
        /// </summary>
        /// <param name="ResourceKey">A string representing a ResourceKey.</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>A string representing the requested resource string.</returns>
        public static string GetLocaleResourceString(string ResourceKey, int LanguageID)
        {
            return GetLocaleResourceString(ResourceKey, LanguageID, true);
        }

        /// <summary>
        /// Gets a resource string based on the specified ResourceKey property.
        /// </summary>
        /// <param name="ResourceKey">A string representing a ResourceKey.</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="LogIfNotFound">A value indicating whether to log error if locale string resource is not found</param>
        /// <returns>A string representing the requested resource string.</returns>
        public static string GetLocaleResourceString(string ResourceKey, int LanguageID, bool LogIfNotFound)
        {
            string result = string.Empty;
            if (ResourceKey == null)
                ResourceKey = string.Empty;
            ResourceKey = ResourceKey.Trim().ToLowerInvariant();
            LocaleStringResourceDictionary resources = LocaleStringResourceManager.GetAllResourcesByLanguageID(LanguageID);

            if (resources.ContainsKey(ResourceKey))
            {
                LocaleStringResource lsr = resources[ResourceKey];
                if (lsr != null)
                    result = lsr.ResourceValue;
            }
            if (String.IsNullOrEmpty(result))
            {
                result = ResourceKey;
                if (LogIfNotFound)
                {
                    LogManager.InsertLog(LogTypeEnum.CommonError, "Resource string is not found", string.Format("Resource string ({0}) is not found. Language ID ={1}", ResourceKey, LanguageID));
                }
            }
            return result;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the default admin language
        /// </summary>
        public static Language DefaultAdminLanguage
        {
            get
            {
                int defaultAdminLanguageID = SettingManager.GetSettingValueInteger("Localization.DefaultAdminLanguageID");

                Language language = LanguageManager.GetLanguageByID(defaultAdminLanguageID);
                if (language != null & language.Published)
                {
                    return language;
                }
                else
                {
                    LanguageCollection publishedLanguages = LanguageManager.GetAllLanguages(false);
                    foreach (Language publishedLanguage in publishedLanguages)
                        return publishedLanguage;
                }
                throw new NopException("Default admin language could not be loaded");
            }
            set
            {
                if (value != null)
                    SettingManager.SetParam("Localization.DefaultAdminLanguageID", value.LanguageID.ToString());
            }
        }
        #endregion
    }
}
