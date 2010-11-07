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
using NopSolutions.NopCommerce.BusinessLogic.Configuration;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.Common;

namespace NopSolutions.NopCommerce.BusinessLogic.Security
{
    /// <summary>
    /// Represents a security helper
    /// </summary>
    public partial class SecurityHelper
    {
        #region Utilities

        private static byte[] EncryptTextToMemory(string Data, byte[] Key, byte[] IV)
        {
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, new TripleDESCryptoServiceProvider().CreateEncryptor(Key, IV), CryptoStreamMode.Write);
            byte[] toEncrypt = new UnicodeEncoding().GetBytes(Data);
            cStream.Write(toEncrypt, 0, toEncrypt.Length);
            cStream.FlushFinalBlock();
            byte[] ret = mStream.ToArray();
            cStream.Close();
            mStream.Close();
            return ret;
        }

        private static string DecryptTextFromMemory(byte[] Data, byte[] Key, byte[] IV)
        {
            MemoryStream msDecrypt = new MemoryStream(Data);
            CryptoStream csDecrypt = new CryptoStream(msDecrypt, new TripleDESCryptoServiceProvider().CreateDecryptor(Key, IV), CryptoStreamMode.Read);
            StreamReader sReader = new StreamReader(csDecrypt, new UnicodeEncoding());
            return sReader.ReadLine();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Decrypts text
        /// </summary>
        /// <param name="CipherText">Cipher text</param>
        /// <returns>Decrypted string</returns>
        public static string Decrypt(string CipherText)
        {
            string encryptionPrivateKey = SettingManager.GetSettingValue("Security.EncryptionPrivateKey");
            return Decrypt(CipherText, encryptionPrivateKey);
        }

        /// <summary>
        /// Decrypts text
        /// </summary>
        /// <param name="CipherText">Cipher text</param>
        /// <param name="EncryptionPrivateKey">Encryption private key</param>
        /// <returns>Decrypted string</returns>
        protected static string Decrypt(string CipherText, string EncryptionPrivateKey)
        {
            if (String.IsNullOrEmpty(CipherText))
                return CipherText;

            TripleDESCryptoServiceProvider tDESalg = new TripleDESCryptoServiceProvider();
            tDESalg.Key = new ASCIIEncoding().GetBytes(EncryptionPrivateKey.Substring(0, 16));
            tDESalg.IV = new ASCIIEncoding().GetBytes(EncryptionPrivateKey.Substring(8, 8));

            byte[] buffer = Convert.FromBase64String(CipherText);
            string result = DecryptTextFromMemory(buffer, tDESalg.Key, tDESalg.IV);
            return result;
        }

        /// <summary>
        /// Encrypts text
        /// </summary>
        /// <param name="PlainText">Plaint text</param>
        /// <returns>Encrypted string</returns>
        public static string Encrypt(string PlainText)
        {
            string encryptionPrivateKey = SettingManager.GetSettingValue("Security.EncryptionPrivateKey");
            return Encrypt(PlainText, encryptionPrivateKey);
        }

        /// <summary>
        /// Encrypts text
        /// </summary>
        /// <param name="PlainText">Plaint text</param>
        /// <param name="EncryptionPrivateKey">Encryption private key</param>
        /// <returns>Encrypted string</returns>
        protected static string Encrypt(string PlainText, string EncryptionPrivateKey)
        {
            if (String.IsNullOrEmpty(PlainText))
                return PlainText;

            TripleDESCryptoServiceProvider tDESalg = new TripleDESCryptoServiceProvider();
            tDESalg.Key = new ASCIIEncoding().GetBytes(EncryptionPrivateKey.Substring(0, 16));
            tDESalg.IV = new ASCIIEncoding().GetBytes(EncryptionPrivateKey.Substring(8, 8));

            byte[] encryptedBinary = EncryptTextToMemory(PlainText, tDESalg.Key, tDESalg.IV);
            string result = Convert.ToBase64String(encryptedBinary);
            return result;
        }

        /// <summary>
        /// Change encryption private key
        /// </summary>
        /// <param name="NewEncryptionPrivateKey">New encryption private key</param>
        public static void ChangeEncryptionPrivateKey(string NewEncryptionPrivateKey)
        {
            if (String.IsNullOrEmpty(NewEncryptionPrivateKey) || NewEncryptionPrivateKey.Length != 16)
                throw new NopException("Encryption private key must be 16 characters long");

            string oldEncryptionPrivateKey = SettingManager.GetSettingValue("Security.EncryptionPrivateKey");

            if (oldEncryptionPrivateKey == NewEncryptionPrivateKey)
                return;

            OrderCollection orders = OrderManager.LoadAllOrders();
            foreach (Order order in orders)
            {
                string decryptedCardType = Decrypt(order.CardType, oldEncryptionPrivateKey);
                string decryptedCardName = Decrypt(order.CardName, oldEncryptionPrivateKey);
                string decryptedCardNumber = Decrypt(order.CardNumber, oldEncryptionPrivateKey);
                string decryptedMaskedCreditCardNumber = Decrypt(order.MaskedCreditCardNumber, oldEncryptionPrivateKey);
                string decryptedCardCVV2 = Decrypt(order.CardCVV2, oldEncryptionPrivateKey);
                string decryptedCardExpirationMonth = Decrypt(order.CardExpirationMonth, oldEncryptionPrivateKey);
                string decryptedCardExpirationYear = Decrypt(order.CardExpirationYear, oldEncryptionPrivateKey);

                string encryptedCardType = Encrypt(decryptedCardType, NewEncryptionPrivateKey);
                string encryptedCardName = Encrypt(decryptedCardName, NewEncryptionPrivateKey);
                string encryptedCardNumber = Encrypt(decryptedCardNumber, NewEncryptionPrivateKey);
                string encryptedMaskedCreditCardNumber = Encrypt(decryptedMaskedCreditCardNumber, NewEncryptionPrivateKey);
                string encryptedCardCVV2 = Encrypt(decryptedCardCVV2, NewEncryptionPrivateKey);
                string encryptedCardExpirationMonth = Encrypt(decryptedCardExpirationMonth, NewEncryptionPrivateKey);
                string encryptedCardExpirationYear = Encrypt(decryptedCardExpirationYear, NewEncryptionPrivateKey);

                OrderManager.UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID,
                   order.CustomerLanguageID, order.CustomerTaxDisplayType,
                   order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
                   order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
                   order.OrderTax, order.OrderTotal, order.OrderDiscount,
                   order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
                   order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
                   order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
                   order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency,
                   order.CustomerCurrencyCode, order.OrderWeight,
                   order.AffiliateID, order.OrderStatus, order.AllowStoringCreditCardNumber,
                   encryptedCardType, encryptedCardName, encryptedCardNumber,
                   encryptedMaskedCreditCardNumber, encryptedCardCVV2, encryptedCardExpirationMonth, encryptedCardExpirationYear,
                   order.PaymentMethodID, order.PaymentMethodName, order.AuthorizationTransactionID,
                   order.AuthorizationTransactionCode, order.AuthorizationTransactionResult,
                   order.CaptureTransactionID, order.CaptureTransactionResult, order.PurchaseOrderNumber,
                   order.PaymentStatus, order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber,
                   order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
                   order.BillingAddress2, order.BillingCity,
                   order.BillingStateProvince, order.BillingStateProvinceID, order.BillingZipPostalCode,
                   order.BillingCountry, order.BillingCountryID, order.ShippingStatus,
                   order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber,
                   order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany,
                   order.ShippingAddress1, order.ShippingAddress2, order.ShippingCity,
                   order.ShippingStateProvince, order.ShippingStateProvinceID, order.ShippingZipPostalCode,
                   order.ShippingCountry, order.ShippingCountryID,
                   order.ShippingMethod, order.ShippingRateComputationMethodID,
                   order.ShippedDate, order.Deleted, order.CreatedOn);
            }

            SettingManager.SetParam("Security.EncryptionPrivateKey", NewEncryptionPrivateKey);
        }

        #endregion
    }
}
