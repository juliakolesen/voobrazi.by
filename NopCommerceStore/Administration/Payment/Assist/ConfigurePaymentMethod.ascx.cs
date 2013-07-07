using System;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.Web.Templates.Payment;

namespace NopSolutions.NopCommerce.Web.Administration.Payment.Assist
{
    public partial class ConfigurePaymentMethod : BaseNopAdministrationUserControl, IConfigurePaymentMethodModule
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var setting = SettingManager.GetSettingByName("PaymentMethod.Assist.OkUrl");
                if (setting == null)
                {
                    SettingManager.AddSetting("PaymentMethod.Assist.OkUrl", string.Format("{0}{1}", Request.Url.GetLeftPart(UriPartial.Authority), "\\Assist.aspx?status=succeeded&orderId={0}"), string.Empty);
                    SettingManager.AddSetting("PaymentMethod.Assist.NoUrl", string.Format("{0}{1}", Request.Url.GetLeftPart(UriPartial.Authority), "\\Assist.aspx?status=failed&orderId={0}"), string.Empty);
                    SettingManager.AddSetting("PaymentMethod.Assist.TestMode", "true", string.Empty);
                    SettingManager.AddSetting("PaymentMethod.Assist.PaymentUrl", "https://test.paysec.by/pay/order.cfm", string.Empty);
                }
                
                BindData();
            }
        }

        private void BindData()
        {
            tbOKUrl.Text = SettingManager.GetSettingValue("PaymentMethod.Assist.OkUrl");
            tbNoUrl.Text = SettingManager.GetSettingValue("PaymentMethod.Assist.NoUrl");
            tbTestMode.Text = SettingManager.GetSettingValue("PaymentMethod.Assist.TestMode");
            tbUrl.Text = SettingManager.GetSettingValue("PaymentMethod.Assist.PaymentUrl");
        }

        public void Save()
        {
            SettingManager.SetParam("PaymentMethod.Assist.OkUrl", tbOKUrl.Text);
            SettingManager.SetParam("PaymentMethod.Assist.NoUrl", tbNoUrl.Text);
            SettingManager.SetParam("PaymentMethod.Assist.TestMode", tbTestMode.Text);
            SettingManager.SetParam("PaymentMethod.Assist.PaymentUrl", tbUrl.Text);
        }
    }
}