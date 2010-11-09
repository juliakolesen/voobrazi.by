using UrlRewritingNet.Web;

namespace UrlRewritingNet.Configuration.Provider
{
    public class RegExUrlRewritingProvider : UrlRewritingProvider
    {
        public override UrlRewritingNet.Web.RewriteRule CreateRewriteRule()
        {
            return new RegExRewriteRule();
        }
    }
}
