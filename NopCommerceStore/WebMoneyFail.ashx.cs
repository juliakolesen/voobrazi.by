using System.Web;

namespace NopSolutions.NopCommerce.Web
{
    /// <summary>
    /// Summary description for WebMoneyFail
    /// </summary>
    public class WebMoneyFail : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Redirect(string.Format("~/checkoutconfirm.aspx?OrderId={0}", context.Request["OrderId"]));
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