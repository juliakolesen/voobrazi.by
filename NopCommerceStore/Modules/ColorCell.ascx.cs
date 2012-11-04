using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.Colors;
using System.Drawing;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Products.Specs;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class ColorCell : BaseNopUserControl
    {
        public ColorItem ColorItem { get; set; }
        private const string color = "Цвет";

        protected string UrlToRedirect
        {
            get
            {
                string currentUrl = CommonHelper.GetThisPageURL(false);
                bool first = true;
                for (int i = 0; i < Request.QueryString.Keys.Count; i++)
                {
                    string key = Request.QueryString.Keys[i];
                    if (key != null && key != color && key != "CategoryID" && key != color)
                    {
                        currentUrl = String.Format("{0}{1}{2}={3}", currentUrl, first ? "?" : "&",
                                            key, CommonHelper.QueryStringInt(key));
                        first = false;
                    }
                }

                SpecificationAttributeOptionCollection saoc = SpecificationAttributeManager.GetSpecificationAttributeOptions();
                SpecificationAttributeOption saOption = saoc.Find(x => x.Name == ColorItem.ColorName);
                if (saOption != null)
                {
                    currentUrl = String.Format("{0}{1}{2}={3}", currentUrl, first ? "?" : "&",
                                    color, saOption.SpecificationAttributeOptionID);
                }

                return currentUrl;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string paletteFolderPath = Server.MapPath("~/images/palette/");
                string palettePath = String.Format("{0}{1}.jpeg", paletteFolderPath, ColorItem.ColorID);
                if (File.Exists(palettePath))
                {
                    imagePalette.ImageUrl = String.Format("~/images/palette/{0}.jpeg", ColorItem.ColorID);
                    imagePalette.Attributes.Add("onclick", String.Format("javascript:location.replace('{0}');", this.UrlToRedirect));
                    imagePalette.ToolTip = String.Format("Фильтровать по цвету:{0}", ColorItem.ColorName);
                }
                else
                {
                    if (ColorItem.ColorArgb != -1)
                    {
                        var bytes = BitConverter.GetBytes((Int32)ColorItem.ColorArgb);
                        cellPanel.BackColor = Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
                        cellPanel.Attributes.Add("onclick", String.Format("javascript:location.replace('{0}');", this.UrlToRedirect));
                        cellPanel.ToolTip = String.Format("Фильтровать по цвету:{0}", ColorItem.ColorName);
                        imagePalette.Visible = false;
                    }
                    else
                    {
                        imagePalette.Attributes.Add("onclick", String.Format("javascript:location.replace('{0}');", this.UrlToRedirect));
                        imagePalette.ToolTip = String.Format("Фильтровать по цвету:{0}", ColorItem.ColorName);
                    }
                }
            }
        }
    }
}