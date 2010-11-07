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
using System.Configuration;
using System.Web.Hosting;
using System.Web.Configuration;
using System.Collections.Specialized;
using System.Configuration.Provider;

namespace NopSolutions.NopCommerce.DataAccess.Media
{
    /// <summary>
    /// Acts as a base class for deriving custom download provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/DownloadProvider")]
    public abstract partial class DBDownloadProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Gets a download
        /// </summary>
        /// <param name="DownloadID">Download identifier</param>
        /// <returns>Download</returns>
        public abstract DBDownload GetDownloadByID(int DownloadID);

        /// <summary>
        /// Deletes a download
        /// </summary>
        /// <param name="DownloadID">Download identifier</param>
        public abstract void DeleteDownload(int DownloadID);

        /// <summary>
        /// Inserts a download
        /// </summary>
        /// <param name="UseDownloadURL">The value indicating whether DownloadURL property should be used</param>
        /// <param name="DownloadURL">The download URL</param>
        /// <param name="DownloadBinary">The download binary</param>
        /// <param name="ContentType">The content type</param>
        /// <param name="Extension">The extension</param>
        /// <param name="IsNew">A value indicating whether the download is new</param>
        /// <returns>Download</returns>
        public abstract DBDownload InsertDownload(bool UseDownloadURL, string DownloadURL, 
            byte[] DownloadBinary, string ContentType, string Extension, bool IsNew);

        /// <summary>
        /// Updates the download
        /// </summary>
        /// <param name="DownloadID">The download identifier</param>
        /// <param name="UseDownloadURL">The value indicating whether DownloadURL property should be used</param>
        /// <param name="DownloadURL">The download URL</param>
        /// <param name="DownloadBinary">The download binary</param>
        /// <param name="ContentType">The content type</param>
        /// <param name="Extension">The extension</param>
        /// <param name="IsNew">A value indicating whether the download is new</param>
        /// <returns>Download</returns>
        public abstract DBDownload UpdateDownload(int DownloadID, bool UseDownloadURL, string DownloadURL, 
            byte[] DownloadBinary, string ContentType, string Extension, bool IsNew);
        #endregion
    }
}
