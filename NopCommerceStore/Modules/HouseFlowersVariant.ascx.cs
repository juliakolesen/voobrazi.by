﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.Products.Specs;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class HouseFlowersVariant : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillDesignVariants();
            }
        }

        private void FillDesignVariants()
        {
            SpecificationAttributeCollection sa = SpecificationAttributeManager.GetSpecificationAttributes();
            SpecificationAttributeOptionCollection saoc = SpecificationAttributeManager.GetSpecificationAttributeOptions();

            FillDropDownList(this.light, sa, saoc, "требование к освещению");
            SetSelectedIndexes();
        }

        private void SetSelectedIndexes()
        {
            SetSelectedIndex(this.light, "light");
        }

        private void SetSelectedIndex(DropDownList dropDownList, String type)
        {
            dropDownList.SelectedIndex = CommonHelper.QueryStringInt(type);
            if (dropDownList.SelectedIndex == 0)
            {
                dropDownList.BackColor = System.Drawing.Color.Gray;
            }
        }

        private void FillDropDownList(DropDownList dropDownList, SpecificationAttributeCollection sa,
                                      SpecificationAttributeOptionCollection saoc, String specificationAttributeName)
        {
            dropDownList.Items.Add("Не выбрано");
            SpecificationAttribute specAttribute = sa.Find(x => x.Name.ToLower() == specificationAttributeName);
            if (specAttribute != null)
            {
                int specificationAttributeID = sa.Find(x => x.Name.ToLower() == specificationAttributeName).SpecificationAttributeID;
                List<SpecificationAttributeOption> specificationAttributeOptionByType = saoc.FindAll(x => x.SpecificationAttributeID == specificationAttributeID);
                foreach (SpecificationAttributeOption sao in specificationAttributeOptionByType)
                {
                    dropDownList.Items.Add(new ListItem(sao.Name, sao.SpecificationAttributeOptionID.ToString()));
                }
            }
        }

        public List<int> GetDesignVariantIds()
        {
            List<int> ids = new List<int>();
            AddFilter(ids, this.light);
            return ids;
        }

        private void AddFilter(List<int> ids, DropDownList dropDownList)
        {
            int id;
            if (dropDownList.SelectedIndex != 0)
            {
                if (Int32.TryParse(dropDownList.SelectedValue, out id))
                {
                    ids.Add(id);
                }
            }
        }

        protected void light_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeDesignParameter("light", this.light);
        }


        private void ChangeDesignParameter(String parameter, DropDownList dropDownList)
        {
            string url = CommonHelper.GetThisPageURL(true);
            url = CommonHelper.RemoveQueryString(url, "light");
            int index = dropDownList.SelectedIndex;
            if (index != 0)
            {
                if (!url.Contains("?"))
                    url += "?";
                else
                    url += "&";

                Response.Redirect(String.Format("{0}{1}={2}", url, parameter, index));
            }
            else { Response.Redirect(url); }
        }
    }
}