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

namespace NopSolutions.NopCommerce.DataAccess.Templates
{
    /// <summary>
    /// Acts as a base class for deriving custom template provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/TemplateProvider")]
    public abstract partial class DBTemplateProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Deletes a category template
        /// </summary>
        /// <param name="CategoryTemplateID">Category template identifier</param>
        public abstract void DeleteCategoryTemplate(int CategoryTemplateID);

        /// <summary>
        /// Gets all category templates
        /// </summary>
        /// <returns>Category template collection</returns>
        public abstract DBCategoryTemplateCollection GetAllCategoryTemplates();

        /// <summary>
        /// Gets a category template
        /// </summary>
        /// <param name="CategoryTemplateID">Category template identifier</param>
        /// <returns>A category template</returns>
        public abstract DBCategoryTemplate GetCategoryTemplateByID(int CategoryTemplateID);

        /// <summary>
        /// Inserts a category template
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="TemplatePath">The template path</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>A category template</returns>
        public abstract DBCategoryTemplate InsertCategoryTemplate(string Name, string TemplatePath,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Updates the category template
        /// </summary>
        /// <param name="CategoryTemplateID">Category template identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="TemplatePath">The template path</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>A category template</returns>
        public abstract DBCategoryTemplate UpdateCategoryTemplate(int CategoryTemplateID, string Name, string TemplatePath,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Deletes a manufacturer template
        /// </summary>
        /// <param name="ManufacturerTemplateID">Manufacturer template identifier</param>
        public abstract void DeleteManufacturerTemplate(int ManufacturerTemplateID);

        /// <summary>
        /// Gets all manufacturer templates
        /// </summary>
        /// <returns>Manufacturer template collection</returns>
        public abstract DBManufacturerTemplateCollection GetAllManufacturerTemplates();

        /// <summary>
        /// Gets a manufacturer template
        /// </summary>
        /// <param name="ManufacturerTemplateID">Manufacturer template identifier</param>
        /// <returns>Manufacturer template</returns>
        public abstract DBManufacturerTemplate GetManufacturerTemplateByID(int ManufacturerTemplateID);

        /// <summary>
        /// Inserts a manufacturer template
        /// </summary>
        /// <param name="Name">The manufacturer template identifier</param>
        /// <param name="TemplatePath">The template path</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Manufacturer template</returns>
        public abstract DBManufacturerTemplate InsertManufacturerTemplate(string Name, string TemplatePath,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Updates the manufacturer template
        /// </summary>
        /// <param name="ManufacturerTemplateID">Manufacturer template identifer</param>
        /// <param name="Name">The manufacturer template identifier</param>
        /// <param name="TemplatePath">The template path</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Manufacturer template</returns>
        public abstract DBManufacturerTemplate UpdateManufacturerTemplate(int ManufacturerTemplateID, string Name, string TemplatePath,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Deletes a product template
        /// </summary>
        /// <param name="ProductTemplateID">Product template identifier</param>
        public abstract void DeleteProductTemplate(int ProductTemplateID);

        /// <summary>
        /// Gets all product templates
        /// </summary>
        /// <returns>Product template collection</returns>
        public abstract DBProductTemplateCollection GetAllProductTemplates();

        /// <summary>
        /// Gets a product template
        /// </summary>
        /// <param name="ProductTemplateID">Product template identifier</param>
        /// <returns>Product template</returns>
        public abstract DBProductTemplate GetProductTemplateByID(int ProductTemplateID);

        /// <summary>
        /// Inserts a product template
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="TemplatePath">The template path</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Product template</returns>
        public abstract DBProductTemplate InsertProductTemplate(string Name, string TemplatePath,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Updates the product template
        /// </summary>
        /// <param name="ProductTemplateID">The product template identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="TemplatePath">The template path</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Product template</returns>
        public abstract DBProductTemplate UpdateProductTemplate(int ProductTemplateID, string Name, string TemplatePath,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn);
        #endregion
    }
}
