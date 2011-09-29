using System;
using System.Web.UI;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.Web.Templates.Payment;

namespace NopSolutions.NopCommerce.Web.Administration.Payment.EasyPay
{
    public partial class ConfigurePaymentMethod : BaseNopAdministrationUserControl, IConfigurePaymentMethodModule
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var secretKey = SettingManager.GetSettingByName("PaymentMethod.EasyPay.UseSandbox");
                if (secretKey == null)
                {
                    SettingManager.AddSetting("PaymentMethod.EasyPay.UseSandbox", "true", string.Empty);
                }
                BindData();
            }
        }

        private void BindData()
        {
            chbUseSandbox.Checked = SettingManager.GetSettingValueBoolean("PaymentMethod.EasyPay.UseSandbox");
        }

        public void Save()
        {
            SettingManager.SetParam("PaymentMethod.EasyPay.UseSandBox", chbUseSandbox.Checked.ToString());
        }
    }
}