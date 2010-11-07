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
using System.Configuration.Provider;
using System.Web.Hosting;
using System.Web.Configuration;
using System.Collections.Specialized;

namespace NopSolutions.NopCommerce.DataAccess.Manufacturers
{
    /// <summary>
    /// Acts as a base class for deriving custom manufacturer provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/ManufacturerProvider")]
    public abstract partial class DBManufacturerProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Gets all manufacturers
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Manufacturer collection</returns>
        public abstract DBManufacturerCollection GetAllManufacturers(bool showHidden);

        /// <summary>
        /// Gets a manufacturer
        /// </summary>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <returns>Manufacturer</returns>
        public abstract DBManufacturer GetManufacturerByID(int ManufacturerID);

        /// <summary>
        /// Inserts a manufacturer
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="TemplateID">The template identifier</param>
        /// <param name="MetaKeywords">The meta keywords</param>
        /// <param name="MetaDescription">The meta description</param>
        /// <param name="MetaTitle">The meta title</param>
        /// <param name="SEName">The search-engine name</param>
        /// <param name="PictureID">The parent picture identifier</param>
        /// <param name="PageSize">The page size</param>
        /// <param name="PriceRanges">The price ranges</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Manufacturer</returns>
        public abstract DBManufacturer InsertManufacturer(string Name, string Description,
            int TemplateID, string MetaKeywords, string MetaDescription, string MetaTitle,
            string SEName, int PictureID, int PageSize, string PriceRanges, bool Published, bool Deleted,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Updates the manufacturer
        /// </summary>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="TemplateID">The template identifier</param>
        /// <param name="MetaKeywords">The meta keywords</param>
        /// <param name="MetaDescription">The meta description</param>
        /// <param name="MetaTitle">The meta title</param>
        /// <param name="SEName">The search-engine name</param>
        /// <param name="PictureID">The parent picture identifier</param>
        /// <param name="PageSize">The page size</param>
        /// <param name="PriceRanges">The price ranges</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Manufacturer</returns>
        public abstract DBManufacturer UpdateManufacturer(int ManufacturerID, string Name, string Description,
            int TemplateID, string MetaKeywords, string MetaDescription, string MetaTitle,
            string SEName, int PictureID, int PageSize, string PriceRanges, bool Published, bool Deleted,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Deletes a product manufacturer mapping
        /// </summary>
        /// <param name="ProductManufacturerID">Product manufacturer mapping identifer</param>
        public abstract void DeleteProductManufacturer(int ProductManufacturerID);

        /// <summary>
        /// Gets product category manufacturer collection
        /// </summary>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product category manufacturer collection</returns>
        public abstract DBProductManufacturerCollection GetProductManufacturersByManufacturerID(int ManufacturerID, bool showHidden);

        /// <summary>
        /// Gets a product manufacturer mapping collection
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product manufacturer mapping collection</returns>
        public abstract DBProductManufacturerCollection GetProductManufacturersByProductID(int ProductID, bool showHidden);

        /// <summary>
        /// Gets a product manufacturer mapping 
        /// </summary>
        /// <param name="ProductManufacturerID">Product manufacturer mapping identifier</param>
        /// <returns>Product manufacturer mapping</returns>
        public abstract DBProductManufacturer GetProductManufacturerByID(int ProductManufacturerID);

        /// <summary>
        /// Inserts a product manufacturer mapping
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <param name="IsFeaturedProduct">A value indicating whether the product is featured</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product manufacturer mapping </returns>
        public abstract DBProductManufacturer InsertProductManufacturer(int ProductID, int ManufacturerID, bool IsFeaturedProduct, int DisplayOrder);

        /// <summary>
        /// Updates the product manufacturer mapping
        /// </summary>
        /// <param name="ProductManufacturerID">Product manufacturer mapping identifier</param>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <param name="IsFeaturedProduct">A value indicating whether the product is featured</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product manufacturer mapping </returns>
        public abstract DBProductManufacturer UpdateProductManufacturer(int ProductManufacturerID,
            int ProductID, int ManufacturerID, bool IsFeaturedProduct, int DisplayOrder);

        #endregion
    }
}
