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
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;

namespace NopSolutions.NopCommerce.DataAccess.Payment
{
    /// <summary>
    /// Acts as a base class for deriving custom payment method provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/PaymentMethodProvider")]
    public abstract partial class DBPaymentMethodProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Deletes a payment method
        /// </summary>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        public abstract void DeletePaymentMethod(int PaymentMethodID);

        /// <summary>
        /// Gets a payment method
        /// </summary>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <returns>Payment method</returns>
        public abstract DBPaymentMethod GetPaymentMethodByID(int PaymentMethodID);

        /// <summary>
        /// Gets a payment method
        /// </summary>
        /// <param name="SystemKeyword">Payment method system keyword</param>
        /// <returns>Payment method</returns>
        public abstract DBPaymentMethod GetPaymentMethodBySystemKeyword(string SystemKeyword);

        /// <summary>
        /// Gets all payment methods
        /// </summary>
        /// <returns>Payment method collection</returns>
        public abstract DBPaymentMethodCollection GetAllPaymentMethods(bool showHidden);

        /// <summary>
        /// Inserts a payment method
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="VisibleName">The visible name</param>
        /// <param name="Description">The description</param>
        /// <param name="ConfigureTemplatePath">The configure template path</param>
        /// <param name="UserTemplatePath">The user template path</param>
        /// <param name="ClassName">The class name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="IsActive">A value indicating whether the payment method is active</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Payment method</returns>
        public abstract DBPaymentMethod InsertPaymentMethod(string Name, string VisibleName, string Description,
           string ConfigureTemplatePath, string UserTemplatePath, string ClassName, string SystemKeyword, bool IsActive, int DisplayOrder);

        /// <summary>
        /// Updates the payment method
        /// </summary>
        /// <param name="PaymentMethodID">The payment method identifer</param>
        /// <param name="Name">The name</param>
        /// <param name="VisibleName">The visible name</param>
        /// <param name="Description">The description</param>
        /// <param name="ConfigureTemplatePath">The configure template path</param>
        /// <param name="UserTemplatePath">The user template path</param>
        /// <param name="ClassName">The class name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="IsActive">A value indicating whether the payment method is active</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Payment method</returns>
        public abstract DBPaymentMethod UpdatePaymentMethod(int PaymentMethodID, string Name, string VisibleName, string Description,
           string ConfigureTemplatePath, string UserTemplatePath, string ClassName, string SystemKeyword, bool IsActive, int DisplayOrder);
        #endregion
    }
}
