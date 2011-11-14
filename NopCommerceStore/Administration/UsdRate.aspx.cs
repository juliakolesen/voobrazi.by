using System;

namespace NopCommerceStore.Administration
{
    using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;

    public partial class UsdRate : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                var setting = SettingManager.GetSettingByName("PaymentMethods.UsdRate");
                if (setting == null)
                {
                    SettingManager.AddSetting("PaymentMethods.UsdRate", "0", string.Empty);
                }
                else
                {
                    tbUsdRate.Text = setting.Value;
                }
            }
        }

        protected void BtnUpdateClick(Object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SettingManager.SetParam("PaymentMethods.UsdRate", tbUsdRate.Text);
            }
        }
    }
}