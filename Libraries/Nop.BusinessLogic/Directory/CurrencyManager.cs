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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Net;
using System.Text;
using System.Xml;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Directory;
using NopSolutions.NopCommerce.Common;

namespace NopSolutions.NopCommerce.BusinessLogic.Directory
{
    /// <summary>
    /// Currency manager
    /// </summary>
    public partial class CurrencyManager
    {
        #region Constants
        private const string CURRENCIES_ALL_KEY = "Nop.currency.all-{0}";
        private const string CURRENCIES_BY_ID_KEY = "Nop.currency.id-{0}";
        private const string CURRENCIES_PATTERN_KEY = "Nop.currency.";
        #endregion

        #region Utilities
        private static CurrencyCollection DBMapping(DBCurrencyCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            CurrencyCollection collection = new CurrencyCollection();
            foreach (DBCurrency dbItem in dbCollection)
            {
                Currency item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Currency DBMapping(DBCurrency dbItem)
        {
            if (dbItem == null)
                return null;

            Currency item = new Currency();
            item.CurrencyID = dbItem.CurrencyID;
            item.Name = dbItem.Name;
            item.CurrencyCode = dbItem.CurrencyCode;
            item.Rate = dbItem.Rate;
            item.DisplayLocale = dbItem.DisplayLocale;
            item.CustomFormatting = dbItem.CustomFormatting;
            item.Published = dbItem.Published;
            item.DisplayOrder = dbItem.DisplayOrder;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets currency live rates
        /// </summary>
        /// <param name="ExchangeRateCurrencyCode">Exchange rate currency code</param>
        /// <param name="UpdateDate">Update date</param>
        /// <param name="Rates">Currency rates table</param>
        public static void GetCurrencyLiveRates(string ExchangeRateCurrencyCode, out DateTime UpdateDate, out DataTable Rates)
        {
            if (String.IsNullOrEmpty(ExchangeRateCurrencyCode) || ExchangeRateCurrencyCode.ToLower() != "eur")
                throw new NopException("You can use our \"CurrencyLiveRate\" service only when exchange rate currency code is set to EURO");

            UpdateDate = DateTime.Now;
            Rates = new DataTable();
            Rates.Columns.Add("CurrencyCode", typeof(string));
            Rates.Columns.Add("Rate", typeof(decimal));
            HttpWebRequest request = WebRequest.Create("http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml") as HttpWebRequest;
            using (WebResponse response = request.GetResponse())
            {
                XmlDocument document = new XmlDocument();
                document.Load(response.GetResponseStream());
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
                nsmgr.AddNamespace("ns", "http://www.ecb.int/vocabulary/2002-08-01/eurofxref");
                nsmgr.AddNamespace("gesmes", "http://www.gesmes.org/xml/2002-08-01");
                XmlNode node = document.SelectSingleNode("gesmes:Envelope/ns:Cube/ns:Cube", nsmgr);
                UpdateDate = DateTime.ParseExact(node.Attributes["time"].Value, "yyyy-MM-dd", null);
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                provider.NumberGroupSeparator = "";
                foreach (XmlNode node2 in node.ChildNodes)
                    Rates.Rows.Add(new object[] {node2.Attributes["currency"].Value, double.Parse(node2.Attributes["rate"].Value, provider) });
            }
        }

        /// <summary>
        /// Deletes currency
        /// </summary>
        /// <param name="CurrencyID">Currency identifier</param>
        public static void DeleteCurrency(int CurrencyID)
        {
            DBProviderManager<DBCurrencyProvider>.Provider.DeleteCurrency(CurrencyID);
            if (CurrencyManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CURRENCIES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a currency
        /// </summary>
        /// <param name="CurrencyID">Currency identifier</param>
        /// <returns>Currency</returns>
        public static Currency GetCurrencyByID(int CurrencyID)
        {
            if (CurrencyID == 0)
                return null;

            string key = string.Format(CURRENCIES_BY_ID_KEY, CurrencyID);
            object obj2 = NopCache.Get(key);
            if (CurrencyManager.CacheEnabled && (obj2 != null))
            {
                return (Currency)obj2;
            }

            DBCurrency dbItem = DBProviderManager<DBCurrencyProvider>.Provider.GetCurrencyByID(CurrencyID);
            Currency currency = DBMapping(dbItem);

            if (CurrencyManager.CacheEnabled)
            {
                NopCache.Max(key, currency);
            }
            return currency;
        }

        /// <summary>
        /// Gets a currency by code
        /// </summary>
        /// <param name="CurrencyCode">Currency code</param>
        /// <returns>Currency</returns>
        public static Currency GetCurrencyByCode(string CurrencyCode)
        {
            if (String.IsNullOrEmpty(CurrencyCode))
                return null;
            CurrencyCollection currencies = GetAllCurrencies();
            foreach (Currency currency in currencies)
            {
                if (currency.CurrencyCode.ToLowerInvariant() == CurrencyCode.ToLowerInvariant())
                {
                    return currency;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets all currencies
        /// </summary>
        /// <returns>Currency collection</returns>
        public static CurrencyCollection GetAllCurrencies()
        {
            bool showHidden = NopContext.Current.IsAdmin;
            string key = string.Format(CURRENCIES_ALL_KEY, showHidden);
            object obj2 = NopCache.Get(key);
            if (CurrencyManager.CacheEnabled && (obj2 != null))
            {
                return (CurrencyCollection)obj2;
            }

            DBCurrencyCollection dbCollection = DBProviderManager<DBCurrencyProvider>.Provider.GetAllCurrencies(showHidden);
            CurrencyCollection currencyCollection = DBMapping(dbCollection);

            if (CurrencyManager.CacheEnabled)
            {
                NopCache.Max(key, currencyCollection);
            }
            return currencyCollection;
        }

        /// <summary>
        /// Inserts a currency
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="CurrencyCode">The currency code</param>
        /// <param name="Rate">The rate</param>
        /// <param name="DisplayLocale">The display locale</param>
        /// <param name="CustomFormatting">The custom formatting</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>A currency</returns>
        public static Currency InsertCurrency(string Name, string CurrencyCode, decimal Rate,
           string DisplayLocale, string CustomFormatting, bool Published, int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            try
            {
                CultureInfo ci = CultureInfo.GetCultureInfo(DisplayLocale);
            }
            catch (Exception)
            {
                throw new NopException("Specified display locale culture is not supported");
            }

            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBCurrency dbItem = DBProviderManager<DBCurrencyProvider>.Provider.InsertCurrency(Name, CurrencyCode, Rate,
                DisplayLocale, CustomFormatting, Published, DisplayOrder, CreatedOn, UpdatedOn);
            Currency currency = DBMapping(dbItem);

            if (CurrencyManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CURRENCIES_PATTERN_KEY);
            }
            return currency;
        }

        /// <summary>
        /// Updates the currency
        /// </summary>
        /// <param name="CurrencyID">Currency identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="CurrencyCode">The currency code</param>
        /// <param name="Rate">The rate</param>
        /// <param name="DisplayLocale">The display locale</param>
        /// <param name="CustomFormatting">The custom formatting</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>A currency</returns>
        public static Currency UpdateCurrency(int CurrencyID, string Name, string CurrencyCode, decimal Rate,
           string DisplayLocale, string CustomFormatting, bool Published, int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            try
            {
                CultureInfo ci = CultureInfo.GetCultureInfo(DisplayLocale);
            }
            catch (Exception)
            {
                throw new NopException("Specified display locale culture is not supported");
            }

            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBCurrency dbItem = DBProviderManager<DBCurrencyProvider>.Provider.UpdateCurrency(CurrencyID, Name, CurrencyCode, Rate,
                DisplayLocale, CustomFormatting, Published, DisplayOrder, CreatedOn, UpdatedOn);
            Currency currency = DBMapping(dbItem);

            if (CurrencyManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CURRENCIES_PATTERN_KEY);
            }
            return currency;
        }

        /// <summary>
        /// Converts currency
        /// </summary>
        /// <param name="Amount">Amount</param>
        /// <param name="SourceCurrencyCode">Source currency code</param>
        /// <param name="TargetCurrencyCode">Target currency code</param>
        /// <returns>Converted value</returns>
        public static decimal ConvertCurrency(decimal Amount, Currency SourceCurrencyCode, Currency TargetCurrencyCode)
        {
            decimal result = Amount;
            if (SourceCurrencyCode.CurrencyID == TargetCurrencyCode.CurrencyID)
                return result;
            if (result != decimal.Zero && SourceCurrencyCode.CurrencyID != TargetCurrencyCode.CurrencyID)
            {
                result = ConvertToPrimaryExchangeRateCurrency(result, SourceCurrencyCode);
                result = ConvertFromPrimaryExchangeRateCurrency(result, TargetCurrencyCode);
            }
            result = Math.Round(result, 2);
            return result;
        }

        /// <summary>
        /// Converts to primary exchange rate currency 
        /// </summary>
        /// <param name="Amount">Amount</param>
        /// <param name="SourceCurrencyCode">Source currency code</param>
        /// <returns>Converted value</returns>
        public static decimal ConvertToPrimaryExchangeRateCurrency(decimal Amount, Currency SourceCurrencyCode)
        {
            decimal result = Amount;
            if (result != decimal.Zero && SourceCurrencyCode.CurrencyID != PrimaryExchangeRateCurrency.CurrencyID)
            {
                decimal ExchangeRate = SourceCurrencyCode.Rate;
                if (ExchangeRate == decimal.Zero)
                    throw new NopException(string.Format("Exchange rate not found for currency [{0}]", SourceCurrencyCode.Name));
                result = result / ExchangeRate;
            }
            return result;
        }

        /// <summary>
        /// Converts from primary exchange rate currency
        /// </summary>
        /// <param name="Amount">Amount</param>
        /// <param name="TargetCurrencyCode">Target currency code</param>
        /// <returns>Converted value</returns>
        public static Decimal ConvertFromPrimaryExchangeRateCurrency(Decimal Amount, Currency TargetCurrencyCode)
        {
            Decimal result = Amount;
            if (result != decimal.Zero && TargetCurrencyCode.CurrencyID != PrimaryExchangeRateCurrency.CurrencyID)
            {
                Decimal ExchangeRate = TargetCurrencyCode.Rate;
                if (ExchangeRate == decimal.Zero)
                    throw new NopException(string.Format("Exchange rate not found for currency [{0}]", TargetCurrencyCode.Name));
                result = result * ExchangeRate;
            }
            return result;
        }
        #endregion
        
        #region Property

        /// <summary>
        /// Gets or sets a primary store currency
        /// </summary>
        public static Currency PrimaryStoreCurrency
        {
            get
            {
                int primaryStoreCurrencyID = SettingManager.GetSettingValueInteger("Currency.PrimaryStoreCurrency");
                return GetCurrencyByID(primaryStoreCurrencyID);
            }
            set
            {
                if (value != null)
                    SettingManager.SetParam("Currency.PrimaryStoreCurrency", value.CurrencyID.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a primary exchange rate currency
        /// </summary>
        public static Currency PrimaryExchangeRateCurrency
        {
            get
            {
                int primaryExchangeRateCurrencyID = SettingManager.GetSettingValueInteger("Currency.PrimaryExchangeRateCurrency");
                return GetCurrencyByID(primaryExchangeRateCurrencyID);
            }
            set
            {
                if (value != null)
                    SettingManager.SetParam("Currency.PrimaryExchangeRateCurrency", value.CurrencyID.ToString());
            }
        }

        /// <summary>
        /// Gets a value indicating whether cache is enabled
        /// </summary>
        public static bool CacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.CurrencyManager.CacheEnabled");
            }
        }
        #endregion
    }
}
