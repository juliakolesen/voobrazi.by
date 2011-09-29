using System;
using System.Web.UI;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.Web.Templates.Payment;

namespace NopSolutions.NopCommerce.Web.Administration.Payment.WebMoney
{
    public partial class ConfigurePaymentMethod : BaseNopAdministrationUserControl, IConfigurePaymentMethodModule
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var secretKey = SettingManager.GetSettingByName("PaymentMethod.WebMoney.UseSandbox");
                if (secretKey == null)
                {
                    SettingManager.AddSetting("PaymentMethod.WebMoney.UseSandbox", "true", string.Empty);
                    SettingManager.AddSetting("PaymentMethod.WebMoney.SimulationMode", "2", string.Empty);
                    SettingManager.AddSetting("PaymentMethod.WebMoney.WMID", string.Empty, string.Empty);
                }
                BindData();
            }
        }

        private void BindData()
        {

            chbUseSandbox.Checked = SettingManager.GetSettingValueBoolean("PaymentMethod.WebMoney.UseSandbox");
            ddlSimulationMode.SelectedValue = SettingManager.GetSettingValue("PaymentMethod.WebMoney.SimulationMode");
            tbWMId.Text = SettingManager.GetSettingValue("PaymentMethod.WebMoney.WMID");
        }

        public void Save()
        {
            SettingManager.SetParam("PaymentMethod.WebMoney.UseSandbox", chbUseSandbox.Checked.ToString());
            SettingManager.SetParam("PaymentMethod.WebMoney.SimulationMode", ddlSimulationMode.SelectedValue);
            SettingManager.SetParam("PaymentMethod.WebMoney.WMID", tbWMId.Text);
        }
    }
}