using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web
{
    public partial class SearchResults : BaseNopPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Результаты поиска";
        }
    }
}