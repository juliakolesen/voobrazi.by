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
using System.Linq;
using System.Text;
using System.Configuration.Provider;
using System.Web.Configuration;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Hosting;

namespace NopSolutions.NopCommerce.DataAccess.Security
{
    /// <summary>
    /// Represents a banned IP address
    /// </summary>
    [DBProviderSectionName("nopDataProviders/BlacklistProvider")]
    public abstract partial class DBBlacklistProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Gets an IP address by its ID
        /// </summary>
        /// <param name="bannedIpAddressID">IP Address unique identifier</param>
        /// <returns>IP address</returns>
        public abstract DBBannedIpAddress GetIpAddressByID(int bannedIpAddressID);

        /// <summary>
        /// Gets all IP addresses
        /// </summary>
        /// <returns>IP address collection</returns>
        public abstract DBBannedIpAddressCollection GetIpAddressAll();

        /// <summary>
        /// Inserts an IP address
        /// </summary>
        /// <param name="address">IP address</param>
        /// <param name="comment">Reason why the IP was banned</param>
        /// <param name="createdOn">When the record was inserted</param>
        /// <param name="updatedOn">When the record was last updated</param>
        /// <returns>IP address</returns>
        public abstract DBBannedIpAddress InsertBannedIpAddress(string address, string comment, DateTime createdOn, DateTime updatedOn);

        /// <summary>
        /// Updates an IP address
        /// </summary>
        /// <param name="bannedIpAddressID">IP address unique identifier</param>
        /// <param name="address">IP address</param>
        /// <param name="comment">Reason why the IP was banned</param>
        /// <param name="createdOn">When the record was last updated</param>
        /// <param name="updatedOn">When the record was last updated</param>
        /// <returns></returns>
        public abstract DBBannedIpAddress UpdateBannedIpAddress(int bannedIpAddressID, string address, string comment, DateTime createdOn, DateTime updatedOn);

        /// <summary>
        /// Deletes an IP address
        /// </summary>
        /// <param name="bannedIpAddressID">IP address unique identifier</param>
        public abstract void DeleteBannedIpAddress(int bannedIpAddressID);

        /// <summary>
        /// Gets an IP network by its ID
        /// </summary>
        /// <param name="bannedIpNetworkID">IP network unique identifier</param>
        /// <returns>IP network</returns>
        public abstract DBBannedIpNetwork GetIpNetworkByID(int bannedIpNetworkID);

        /// <summary>
        /// Gets all IP addresses
        /// </summary>
        /// <returns>IP address collection</returns>
        public abstract DBBannedIpNetworkCollection GetIpNetworkAll();

        /// <summary>
        /// Inserts an IP network
        /// </summary>
        /// <param name="startAddress">First IP address in the range</param>
        /// <param name="endAddress">Last IP address in the range</param>
        /// <param name="comment">Reason why the IP network was banned</param>
        /// <param name="ipException">Exceptions in the range</param>
        /// <param name="createdOn">When the record was inserted</param>
        /// <param name="updatedOn">When the record was last updated</param>
        /// <returns>IP network</returns>
        public abstract DBBannedIpNetwork InsertBannedIpNetwork(string startAddress, string endAddress, string comment, string ipException, DateTime createdOn, DateTime updatedOn);

        /// <summary>
        /// Updates an IP network
        /// </summary>
        /// <param name="bannedIpNetworkID">IP network unique identifier</param>
        /// <param name="startAddress">First IP address in the range</param>
        /// <param name="endAddress">Last IP address in the range</param>
        /// <param name="comment">Reason why the IP network was banned</param>
        /// <param name="ipException">Exceptions in the range</param>
        /// <param name="createdOn">When the record was inserted</param>
        /// <param name="updatedOn">When the record was last updated</param>
        /// <returns>IP network</returns>
        public abstract DBBannedIpNetwork UpdateBannedIpNetwork(int bannedIpNetworkID, string startAddress, string endAddress, string comment, string ipException, DateTime createdOn, DateTime updatedOn);

        /// <summary>
        /// Deletes an IP network
        /// </summary>
        /// <param name="bannedIpNetworkID">IP network unique identifier</param>
        public abstract void DeleteBannedIpNetwork(int bannedIpNetworkID);
        #endregion
    }
}
