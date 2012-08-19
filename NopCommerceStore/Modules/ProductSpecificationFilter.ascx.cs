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
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Specs;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Modules
{
	public partial class ProductSpecificationFilterControl : BaseNopUserControl
	{
        private String[] excludeParamsForFilter = new String[] { "sortBy", "pageSize", "visoutDesign", "wrapping", "bunch", 
                                                                 "composition", "flowers", "bunchForm", "light" };
        private String[] excludeFilteredOptions = new String[] { "оформление без оформления", "оформление упаковка", 
                                                                 "оформление букет", "оформление композиция", 
                                                                 "по основным цветам", "форма букета", "требование к освещению" };
        #region Utilities
		protected void BindData()
		{
			SpecificationAttributeOptionFilterCollection alreadyFilteredOptions = getAlreadyFilteredSpecs();
			SpecificationAttributeOptionFilterCollection notFilteredOptions = getNotFilteredSpecs();

			if (alreadyFilteredOptions.Count > 0 || notFilteredOptions.Count > 0)
			{
				if (alreadyFilteredOptions.Count > 0)
				{
					rptAlreadyFilteredPSO.DataSource = alreadyFilteredOptions;
					rptAlreadyFilteredPSO.DataBind();

                    String navigateUrl = CommonHelper.GetThisPageURL(false);
                    bool first = true;
                    foreach (var key in Request.QueryString.Keys)
                    {
                        if (key == null)
                            continue;

                        string skey = key.ToString();
                        if (excludeParamsForFilter.Contains(skey))
                        {
                            if (first)
                            {
                                navigateUrl += "?";
                                first = false;
                            }
                            else
                            {
                                navigateUrl += "&";
                            }

                            navigateUrl += key + "=" + CommonHelper.QueryStringInt(key.ToString());
                        }
                    }

                    hlRemoveFilter.NavigateUrl = navigateUrl; 
				}
				else
				{
					pnlAlreadyFilteredPSO.Visible = false;
					pnlRemoveFilter.Visible = false;
				}

				if (notFilteredOptions.Count > 0)
				{
					rptFilterByPSO.DataSource = notFilteredOptions;
					rptFilterByPSO.DataBind();
				}
				else
				{
					pnlPSOSelector.Visible = false;
				}
			}
			else
			{
				Visible = false;
			}
		}

		protected SpecificationAttributeOptionFilterCollection getAlreadyFilteredSpecs()
		{
			SpecificationAttributeOptionFilterCollection result = new SpecificationAttributeOptionFilterCollection();

			string[] queryStringParams = getAlreadyFilteredSpecsQueryStringParams();
			foreach (string qsp in queryStringParams)
			{
				int id = 0;
				int.TryParse(Request.QueryString[qsp], out id);
				SpecificationAttributeOption sao = SpecificationAttributeManager.GetSpecificationAttributeOptionByID(id);
				if (sao != null)
				{
					SpecificationAttribute sa = sao.SpecificationAttribute;
					if (sa != null)
					{
						result.Add(new SpecificationAttributeOptionFilter
						{
							SpecificationAttributeID = sa.SpecificationAttributeID,
							SpecificationAttributeName = sa.Name,
							DisplayOrder = sa.DisplayOrder,
							SpecificationAttributeOptionID = sao.SpecificationAttributeOptionID,
							SpecificationAttributeOptionName = sao.Name
						});
					}
				}
			}

			return result;
		}


		public class SpecificationAttributeOptionFilterComparer : IComparer<SpecificationAttributeOptionFilter>
		{
			public int Compare(SpecificationAttributeOptionFilter first, SpecificationAttributeOptionFilter second)
			{
				return first.SpecificationAttributeID.CompareTo(second.SpecificationAttributeID);
			}
		}

		protected SpecificationAttributeOptionFilterCollection getNotFilteredSpecs()
		{
			//get all
			SpecificationAttributeOptionFilterCollection result = SpecificationAttributeManager.GetSpecificationAttributeOptionFilter(this.CategoryID);

			//remove already filtered
			SpecificationAttributeOptionFilterCollection alreadyFilteredOptions = getAlreadyFilteredSpecs();
			foreach (SpecificationAttributeOptionFilter saof1 in alreadyFilteredOptions)
			{
				var query = from s
								in result
							where s.SpecificationAttributeID == saof1.SpecificationAttributeID
							select s;

				List<SpecificationAttributeOptionFilter> toRemove = query.ToList();

				foreach (SpecificationAttributeOptionFilter saof2 in toRemove)
				{
					result.Remove(saof2);
				}
			}

            result.RemoveAll(x => excludeFilteredOptions.Contains(x.SpecificationAttributeName.ToLower()));
			result.Sort(new SpecificationAttributeOptionFilterComparer());
			return result;
		}

		protected string[] getAlreadyFilteredSpecsQueryStringParams()
		{
			List<String> result = new List<string>();

			List<string> reservedQueryStringParamsSplitted = new List<string>();
			if (!String.IsNullOrEmpty(this.ReservedQueryStringParams))
			{
				reservedQueryStringParamsSplitted = this.ReservedQueryStringParams.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
			}

			for (int i=0; i<Request.QueryString.Keys.Count; i++)
			{
                if (Request.QueryString.Keys[i] == "CategoryID" || excludeParamsForFilter.Contains(Request.QueryString.Keys[i]))
					continue;

				string qsp = Request.QueryString.Keys[i];
				if (!String.IsNullOrEmpty(qsp))
				{
					if (!reservedQueryStringParamsSplitted.Contains(qsp))
					{
						if (!result.Contains(qsp))
							result.Add(qsp);
					}
				}
			}
			return result.ToArray();
		}

		protected string excludeQueryStringParams(string url)
		{
			if (!String.IsNullOrEmpty(this.ExcludedQueryStringParams))
			{
				string[] excludedQueryStringParamsSplitted = this.ExcludedQueryStringParams.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string exclude in excludedQueryStringParamsSplitted)
				{
					url = CommonHelper.RemoveQueryString(url, exclude);
				}
			}

			return url;
		}

		//private string lastSA = string.Empty;
		readonly List<string> usedSA = new List<string>();
		private int cnt;
		protected string addSpecificationAttribute()
		{
			string retVal = string.Empty;
			//Get the data field value of interest for this row   
			string currentSA = Eval("SpecificationAttributeName").ToString();

			//See if there's been a change in value
			if (!usedSA.Contains(currentSA))
			{
				//string tmp = hfLastSA.Value + " - " + currentSA;
				cnt++;
				//hfLastSA.Value = currentSA;
				usedSA.Add(currentSA);
				if (cnt == 1)
					retVal = String.Format("<table width=\"100%\"><tr><td valign=\"top\" width=\"50%\" style=\"vertical-align:top;\"><p class=\"headers\">» {0}</p><div class=\"var\">", Server.HtmlEncode(currentSA));
				else if (CountSpecificationAttributes() == cnt)
					retVal = String.Format("</div></td>{0}<p class=\"headers\">» {1}</p><div class=\"var\">", cnt % 2 == 1 ? "</tr><tr><td valign=\"top\" style=\"vertical-align:top;\" colspan='2'>" : "<td valign=\"top\" width=\"50%\" style=\"vertical-align:top;\">", Server.HtmlEncode(currentSA));
				else
					retVal = String.Format("</div></td>{0}<td valign=\"top\" width=\"50%\" style=\"vertical-align:top;\"><p class=\"headers\">» {1}</p><div class=\"var\">", cnt % 2 == 1 ? "</tr><tr>" : "", Server.HtmlEncode(currentSA));
				//retVal = tmp + " " + retVal;
			}

			return retVal;
		}

		private int countSpecificationAttributes = -1;
		private int CountSpecificationAttributes()
		{
			if (countSpecificationAttributes != -1)
				return countSpecificationAttributes;
			List<int> counted = new List<int>();
			return getNotFilteredSpecs().Count(c =>
			{
				if (!counted.Contains(c.SpecificationAttributeID))
				{
					counted.Add(c.SpecificationAttributeID);
					return true;
				}
				return false;
			});
		}

		#endregion

		#region Handlers


		protected override void OnPreRender(EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				BindData();
			}

			base.OnPreRender(e);
		}

		protected void rptFilterByPSO_OnItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				SpecificationAttributeOptionFilter row = e.Item.DataItem as SpecificationAttributeOptionFilter;
				HyperLink lnkFilter = e.Item.FindControl("lnkFilter") as HyperLink;
				if (lnkFilter != null)
				{
					string name = row.SpecificationAttributeName.Replace(" ", "");
					string url = CommonHelper.ModifyQueryString(CommonHelper.GetThisPageURL(true), name + "=" + row.SpecificationAttributeOptionID, null);
					url = excludeQueryStringParams(url);
					lnkFilter.NavigateUrl = url;
				}
			}
		}

		protected void rptAlreadyFilteredPSO_OnItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				SpecificationAttributeOptionFilter row = e.Item.DataItem as SpecificationAttributeOptionFilter;
			}
		}

		#endregion

		#region Methods
		public List<int> GetAlreadyFilteredSpecOptionIDs()
		{
			List<int> result = new List<int>();
			SpecificationAttributeOptionFilterCollection filterOptions = getAlreadyFilteredSpecs();
			foreach (SpecificationAttributeOptionFilter saof in filterOptions)
			{
				if (!result.Contains(saof.SpecificationAttributeOptionID))
					result.Add(saof.SpecificationAttributeOptionID);
			}
			return result;
		}
		#endregion

		#region Properties
		public string ExcludedQueryStringParams
		{
			get
			{
				if (ViewState["ExcludedQueryStringParams"] == null)
					return string.Empty;
				else
					return (string)ViewState["ExcludedQueryStringParams"];
			}
			set
			{
				ViewState["ExcludedQueryStringParams"] = value;
			}
		}

		public string ReservedQueryStringParams
		{
			get
			{
				if (ViewState["ReservedQueryStringParams"] == null)
					return string.Empty;
				else
					return (string)ViewState["ReservedQueryStringParams"];
			}
			set
			{
				ViewState["ReservedQueryStringParams"] = value;
			}
		}

		/// <summary>
		/// Category identifier
		/// </summary>
		public int CategoryID
		{
			get
			{
				int categoryId;
				if (ViewState["CategoryID"] == null)
				{
					categoryId = CommonHelper.QueryStringInt("CategoryID");
					if (categoryId == 0)
					{
						Product product = ProductManager.GetProductByID(ProductID);
						categoryId = product.ProductCategories.Find(p => p.Category.ParentCategory != null).CategoryID;
					}
				}
				else
					categoryId = (int)ViewState["CategoryID"];
				return categoryId;
			}
			set
			{
				ViewState["CategoryID"] = value;
			}
		}

		public int ProductID
		{
			get
			{
				return CommonHelper.QueryStringInt("ProductID");
			}
		}

		#endregion
	}
}