using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Colors;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Products;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class ColorsFilterControl : BaseNopUserControl
    {
        public List<ColorItem> Colors { get; set; }
        public string CategoryName { get; set; }

        protected string UrlToRedirect
        {
            get
            {
                string currentUrl = CommonHelper.GetThisPageURL(false);
                bool first = true;
                for (int i = 0; i < Request.QueryString.Keys.Count; i++)
                {
                    string key = Request.QueryString.Keys[i];
                    if (key != null && key != "Цвет" && key != "CategoryID")
                    {
                        currentUrl = String.Format("{0}{1}{2}={3}", currentUrl, first ? "?" : "&",
                                            key, CommonHelper.QueryStringInt(key));
                        first = false;
                    }
                }
                
                return currentUrl;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!String.IsNullOrEmpty(CategoryName))
            {
                BindData();
            }
            else
            {
                this.Visible = false;
            }
        }

        protected void onclick(object sender, EventArgs e)
        {
           Response.Redirect(this.UrlToRedirect);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void BindData()
        {
            if (Colors != null && Colors.Count > 0)
            {
                rptColors.DataSource = Colors;
                rptColors.DataBind();
            }
            else
            {
                this.Visible = false;
            }
        }
    }
}