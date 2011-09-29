using System.Web;

namespace NopSolutions.NopCommerce.Web
{
    /// <summary>
    /// Summary description for WebMoneySucceeded
    /// </summary>
    public class WebMoneySucceeded : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Redirect("~/checkoutcompleted.aspx");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}