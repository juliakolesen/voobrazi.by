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
using System.Configuration;
using System.Data;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic.Manufacturers;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Templates;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Web.Administration.Modules;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class ManufacturerSEOControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            Manufacturer manufacturer = ManufacturerManager.GetManufacturerByID(this.ManufacturerID);
            if (manufacturer != null)
            {
                this.txtMetaKeywords.Text = manufacturer.MetaKeywords;
                this.txtMetaDescription.Text = manufacturer.MetaDescription;
                this.txtMetaTitle.Text = manufacturer.MetaTitle;
                this.txtSEName.Text = manufacturer.SEName;
                this.txtPageSize.Value = manufacturer.PageSize;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }

        public void SaveInfo()
        {
            Manufacturer manufacturer = ManufacturerManager.GetManufacturerByID(this.ManufacturerID);

            if (manufacturer != null)
            {
                manufacturer = ManufacturerManager.UpdateManufacturer(manufacturer.ManufacturerID, manufacturer.Name, manufacturer.Description,
                   manufacturer.TemplateID, txtMetaKeywords.Text, txtMetaDescription.Text,
                   txtMetaTitle.Text, txtSEName.Text, manufacturer.PictureID, txtPageSize.Value,
                   manufacturer.PriceRanges, manufacturer.Published,
                   manufacturer.Deleted, manufacturer.DisplayOrder, manufacturer.CreatedOn, manufacturer.UpdatedOn);
            }
        }

        public int ManufacturerID
        {
            get
            {
                return CommonHelper.QueryStringInt("ManufacturerID");
            }
        }
    }
}