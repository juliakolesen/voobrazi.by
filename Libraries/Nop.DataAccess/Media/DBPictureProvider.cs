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

namespace NopSolutions.NopCommerce.DataAccess.Media
{
    /// <summary>
    /// Acts as a base class for deriving custom picture provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/PictureProvider")]
    public abstract partial class DBPictureProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Gets a picture
        /// </summary>
        /// <param name="PictureID">Picture identifier</param>
        /// <returns>Picture</returns>
        public abstract DBPicture GetPictureByID(int PictureID);

        /// <summary>
        /// Deletes a picture
        /// </summary>
        /// <param name="PictureID">Picture identifier</param>
        public abstract void DeletePicture(int PictureID);

        /// <summary>
        /// Inserts a picture
        /// </summary>
        /// <param name="PictureBinary">The picture binary</param>
        /// <param name="Extension">The picture extension</param>
        /// <param name="IsNew">A value indicating whether the picture is new</param>
        /// <returns>Picture</returns>
        public abstract DBPicture InsertPicture(byte[] PictureBinary, string Extension, bool IsNew);

        /// <summary>
        /// Updates the picture
        /// </summary>
        /// <param name="PictureID">The picture identifier</param>
        /// <param name="PictureBinary">The picture binary</param>
        /// <param name="Extension">The picture extension</param>
        /// <param name="IsNew">A value indicating whether the picture is new</param>
        /// <returns>Picture</returns>
        public abstract DBPicture UpdatePicture(int PictureID, byte[] PictureBinary, string Extension, bool IsNew);
        #endregion
    }
}
