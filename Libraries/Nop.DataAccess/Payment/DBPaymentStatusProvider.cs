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
    /// Acts as a base class for deriving custom payment status provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/PaymentStatusProvider")]
    public abstract partial class DBPaymentStatusProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Gets a payment status by ID
        /// </summary>
        /// <param name="PaymentStatusID">payment status identifier</param>
        /// <returns>Payment status</returns>
        public abstract DBPaymentStatus GetPaymentStatusByID(int PaymentStatusID);

        /// <summary>
        /// Gets all payment statuses
        /// </summary>
        /// <returns>Payment status collection</returns>
        public abstract DBPaymentStatusCollection GetAllPaymentStatuses();

        #endregion
    }
}
