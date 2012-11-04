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
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic.Products.Specs;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Colors;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class SpecificationAttributeOptionAddControl : BaseNopAdministrationUserControl
    {
        private bool IsColor
        {
            get
            {
                SpecificationAttribute sa = SpecificationAttributeManager.GetSpecificationAttributeByID(SpecificationAttributeID);
                return (sa != null && sa.Name.Equals("цвет", StringComparison.CurrentCultureIgnoreCase));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsColor)
            {
                ToolTipLabelColor.Visible = true;
                txtColorArgb.Visible = true;
                cusCustom.Enabled = true;
                FileUploadControl.Visible = true;
            }

            hlBack.NavigateUrl = "~/Administration/SpecificationAttributeDetails.aspx?SpecificationAttributeID=" + SpecificationAttributeID;
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && IsValidUpload())
            {
                try
                {
                    SpecificationAttributeOption sao = null;
                    if (IsColor)
                    {
                        int colorArgb = -1;
                        if(!String.IsNullOrEmpty(txtColorArgb.Text))
                        {
                            colorArgb = Int32.Parse(txtColorArgb.Text, System.Globalization.NumberStyles.AllowHexSpecifier);
                        }
                        ColorManager.InsertColor(txtNewOptionName.Text, colorArgb);
                        ColorItem colorItem = ColorManager.GetColorByColorName(txtNewOptionName.Text);
                        Upload(colorItem);
                    }
                    sao = SpecificationAttributeManager.InsertSpecificationAttributeOption(SpecificationAttributeID, txtNewOptionName.Text, txtNewOptionDisplayOrder.Value);
                    Response.Redirect("SpecificationAttributeDetails.aspx?SpecificationAttributeID=" + sao.SpecificationAttributeID.ToString());
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        public int SpecificationAttributeID
        {
            get
            {
                return CommonHelper.QueryStringInt("SpecificationAttributeID");
            }
        }

        protected void cusCustom_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = ColorManager.GetColorByColorName(txtNewOptionName.Text) == null;
        }

        private bool IsValidUpload()
        {
            if (IsColor)
            {
                if (FileUploadControl.HasFile)
                {
                    if (FileUploadControl.PostedFile.ContentType != "image/jpeg")
                    {
                        ProcessException(new Exception("Допустим только файлы jpeg!"));
                        return false;
                    }

                    if (FileUploadControl.PostedFile.ContentLength > 5242880)
                    {
                        ProcessException(new Exception("Файл должен быть меньше чем 5 Mb!"));
                        return false;
                    }
                }
                else if (String.IsNullOrEmpty(txtColorArgb.Text))
                {
                    ProcessException(new Exception("Выберите цвет или палитру"));
                    return false;
                }
            }

            return true;
        }

        protected void Upload(ColorItem item)
        {
            try
            {
                if (FileUploadControl.HasFile)
                {
                    string directory = Server.MapPath("~/images/palette/");
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    string newFileName = String.Format("{0}{1}{2}", directory, item.ColorID,
                                                       Path.GetExtension(FileUploadControl.FileName));
                    if(File.Exists(newFileName))
                    {
                        File.Delete(newFileName);
                    }

                    FileUploadControl.SaveAs(newFileName);
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }
    }
}