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
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Configuration.Provider;
using System.Web.Hosting;
using System.Web.Configuration;
using System.Configuration;
using System.Collections.Specialized;

namespace NopSolutions.NopCommerce.DataAccess.Audit
{
    /// <summary>
    /// Acts as a base class for deriving custom log provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/LogProvider")]
    public abstract partial class DBLogProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Deletes a log item
        /// </summary>
        /// <param name="LogID">Log item identifier</param>
        public abstract void DeleteLog(int LogID);
        
        /// <summary>
        /// Clears a log
        /// </summary>
        public abstract void ClearLog();
        
        /// <summary>
        /// Gets all log items
        /// </summary>
        /// <returns>Log item collection</returns>
        public abstract DBLogCollection GetAllLogs();

        /// <summary>
        /// Gets a log item
        /// </summary>
        /// <param name="LogID">Log item identifier</param>
        /// <returns>Log item</returns>
        public abstract DBLog GetLogByID(int LogID);

        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="LogTypeID">Log item type identifier</param>
        /// <param name="Severity">The severity</param>
        /// <param name="Message">The short message</param>
        /// <param name="Exception">The full exception</param>
        /// <param name="IPAddress">The IP address</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="PageURL">The page URL</param>
        /// <param name="CreatedOn">The date and time of instance creationL</param>
        /// <returns>Log item</returns>
        public abstract DBLog InsertLog(int LogTypeID, int Severity, string Message,
            string Exception, string IPAddress, int CustomerID, string PageURL, DateTime CreatedOn);
        #endregion
    }
}