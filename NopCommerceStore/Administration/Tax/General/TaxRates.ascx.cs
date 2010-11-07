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
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Web.Templates.Shipping;
using NopSolutions.NopCommerce.Web.Templates.Tax;

namespace NopSolutions.NopCommerce.Web.Administration.Tax.GeneralTaxConfigure
{
    public partial class TaxRatesControl : BaseNopAdministrationUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ToggleGrid(true);
                BindGrid();
            }
        }

        protected void FillTaxCategoryDropDown()
        {
            this.ddlTaxCategory.Items.Clear();
            TaxCategoryCollection taxCategories = TaxCategoryManager.GetAllTaxCategories();
            foreach (TaxCategory taxCategory in taxCategories)
            {
                ListItem ddlTaxCategoryItem2 = new ListItem(taxCategory.Name, taxCategory.TaxCategoryID.ToString());
                this.ddlTaxCategory.Items.Add(ddlTaxCategoryItem2);
            }
        }

        protected void FillCountryDropDowns()
        {
            this.ddlCountry.Items.Clear();
            CountryCollection countryCollection = CountryManager.GetAllCountries();
            foreach (Country country in countryCollection)
            {
                ListItem ddlCountryItem2 = new ListItem(country.Name, country.CountryID.ToString());
                this.ddlCountry.Items.Add(ddlCountryItem2);
            }
        }

        protected void FillStateProvinceDropDowns()
        {
            this.ddlStateProvince.Items.Clear();
            int countryID = int.Parse(this.ddlCountry.SelectedItem.Value);

            StateProvinceCollection stateProvinceCollection = StateProvinceManager.GetStateProvincesByCountryID(countryID);
            ListItem ddlStateProvinceItem = new ListItem("*", "0");
            this.ddlStateProvince.Items.Add(ddlStateProvinceItem);
            foreach (StateProvince stateProvince in stateProvinceCollection)
            {
                ListItem ddlStateProviceItem2 = new ListItem(stateProvince.Name, stateProvince.StateProvinceID.ToString());
                this.ddlStateProvince.Items.Add(ddlStateProviceItem2);
            }
        }

        protected void BindGrid()
        {
            TaxRateCollection taxRates = TaxRateManager.GetAllTaxRates();
            gvTaxRates.DataSource = taxRates;
            gvTaxRates.DataBind();
            if (taxRates.Count > 10)
            {
                btnAddNew1.Visible = true;
                btnAddNew2.Visible = true;
            }
            else
            {
                btnAddNew1.Visible = true;
                btnAddNew2.Visible = false;
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillStateProvinceDropDowns();
        }

        protected void gvTaxRates_RowEditing(object source, GridViewEditEventArgs e)
        {
            HiddenField hfTaxRateID = gvTaxRates.Rows[e.NewEditIndex].Cells[0].FindControl("hfTaxRateID") as HiddenField;
            int taxRateID = int.Parse(hfTaxRateID.Value);            
            LoadEditor(taxRateID);
            ToggleGrid(false);
        }
        
        protected void gvTaxRates_RowDeleting(object source, GridViewDeleteEventArgs e)
        {
            HiddenField hfTaxRateID = gvTaxRates.Rows[e.RowIndex].Cells[0].FindControl("hfTaxRateID") as HiddenField;
            int taxRateID = int.Parse(hfTaxRateID.Value);
            TaxRateManager.DeleteTaxRate(taxRateID);
            BindGrid();
        }
        
        protected string GetStateProvinceInfo(int StateProvinceID)
        {
            StateProvince stateProvince = StateProvinceManager.GetStateProvinceByID(StateProvinceID);
            if (stateProvince != null)
                return stateProvince.Name;
            else
                return "*";
        }

        protected string GetZipInfo(string Zip)
        {
            if (string.IsNullOrEmpty(Zip))
                return "*";
            else
                return Zip;
        }

        protected void ToggleGrid(bool show)
        {
            pnlGrid.Visible = show;
            pnlEdit.Visible = !show;
        }

        protected void LoadEditor(int TaxRateID)
        {
            ToggleGrid(false);


            TaxRate taxRate = TaxRateManager.GetTaxRateByID(TaxRateID);
            if (taxRate != null)
            {
                lblTaxRateID.Text = taxRate.TaxRateID.ToString();

                FillTaxCategoryDropDown();
                CommonHelper.SelectListItem(this.ddlTaxCategory, taxRate.TaxCategoryID);
                FillCountryDropDowns();
                CommonHelper.SelectListItem(this.ddlCountry, taxRate.CountryID);
                FillStateProvinceDropDowns();
                CommonHelper.SelectListItem(this.ddlStateProvince, taxRate.StateProvinceID);
                this.txtZip.Text = taxRate.Zip;
                this.txtPercentage.Value = taxRate.Percentage;
            }
            else
            {
                lblTaxRateID.Text = "0";

                FillTaxCategoryDropDown();
                FillCountryDropDowns();
                FillStateProvinceDropDowns();

                txtZip.Text = string.Empty;
                txtPercentage.Value = decimal.Zero;
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                LoadEditor(0);
                ToggleGrid(false);
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    int taxRateID = 0;
                    int.TryParse(lblTaxRateID.Text, out taxRateID);
                    int TaxCategoryID = int.Parse(this.ddlTaxCategory.SelectedItem.Value);
                    int CountryID = int.Parse(this.ddlCountry.SelectedItem.Value);
                    int StateProvinceID = int.Parse(this.ddlStateProvince.SelectedItem.Value);
                    string ZipPostalCode = txtZip.Text;
                    decimal Percentage = txtPercentage.Value;

                    TaxRate taxRate = TaxRateManager.GetTaxRateByID(taxRateID);

                    if (taxRate != null)
                    {
                        taxRate = TaxRateManager.UpdateTaxRate(taxRate.TaxRateID, TaxCategoryID,
                            CountryID, StateProvinceID, ZipPostalCode, Percentage);
                    }
                    else
                    {
                        taxRate = TaxRateManager.InsertTaxRate(TaxCategoryID,
                            CountryID, StateProvinceID, ZipPostalCode, Percentage);
                    }

                    BindGrid();
                    ToggleGrid(true);
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            ToggleGrid(true);
        }
    }
}
