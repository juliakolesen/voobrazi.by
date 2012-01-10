using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class SearchControlDefault : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch1_Click(object sender, EventArgs e)
        {
            if (tbSearchCriteria1.Text.Length > 0)
            {
                Response.Redirect("~/SearchResults.aspx?searchParameter=" + tbSearchCriteria1.Text);
            }
        }
    }
}