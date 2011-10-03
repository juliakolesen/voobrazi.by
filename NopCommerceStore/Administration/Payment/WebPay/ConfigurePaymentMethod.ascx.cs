using System;
using System.Web.UI;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.Web.Templates.Payment;

namespace NopSolutions.NopCommerce.Web.Administration.Payment.WebPay
{
    public partial class ConfigurePaymentMethod : BaseNopAdministrationUserControl, IConfigurePaymentMethodModule
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var secretKey = SettingManager.GetSettingByName("PaymentMethod.WebPay.UseSandbox");
                if (secretKey == null)
                {
                    SettingManager.AddSetting("PaymentMethod.WebPay.UseSandbox", "true", string.Empty);
                    SettingManager.AddSetting("PaymentMethod.WebPay.StoreName", "voobrazi.by", string.Empty);
                    SettingManager.AddSetting("PaymentMethod.WebPay.ReturnUrl", string.Format("{0}{1}", Request.Url.GetLeftPart(UriPartial.Authority), "WebPay/Success.aspx"), string.Empty);
                    SettingManager.AddSetting("PaymentMethod.WebPay.CancelUrl", string.Format("{0}{1}", Request.Url.GetLeftPart(UriPartial.Authority), "WebPay/Cancel.aspx"), string.Empty);
                    SettingManager.AddSetting("PaymentMethod.WebPay.NotifyUrl", string.Format("{0}{1}", Request.Url.GetLeftPart(UriPartial.Authority), "WebPay/Notify.aspx"), string.Empty);
                    SettingManager.AddSetting("PaymentMethod.WebPay.ShippingName", "Стоимость доставки", string.Empty);
                }
                BindData();
            }
        }

        private void BindData()
        {

            chbUseSandbox.Checked = SettingManager.GetSettingValueBoolean("PaymentMethod.WebPay.UseSandbox");
            tbStoreName.Text = SettingManager.GetSettingValue("PaymentMethod.WebPay.StoreName");
            tbReturnUrl.Text = SettingManager.GetSettingValue("PaymentMethod.WebPay.ReturnUrl");
            tbCancelUrl.Text = SettingManager.GetSettingValue("PaymentMethod.WebPay.CancelUrl");
            tbNotifyUrl.Text = SettingManager.GetSettingValue("PaymentMethod.WebPay.NotifyUrl");
            tbShippingName.Text = SettingManager.GetSettingValue("PaymentMethod.WebPay.ShippingName");
        }

        public void Save()
        {
            SettingManager.SetParam("PaymentMethod.WebPay.UseSandbox", chbUseSandbox.Checked.ToString());
            SettingManager.SetParam("PaymentMethod.WebPay.StoreName", tbStoreName.Text);
            SettingManager.SetParam("PaymentMethod.WebPay.ReturnUrl", tbReturnUrl.Text);
            SettingManager.SetParam("PaymentMethod.WebPay.CancelUrl", tbCancelUrl.Text);
            SettingManager.SetParam("PaymentMethod.WebPay.NotifyUrl", tbNotifyUrl.Text);
            SettingManager.SetParam("PaymentMethod.WebPay.ShippingName", tbShippingName.Text);
        }
    }
}