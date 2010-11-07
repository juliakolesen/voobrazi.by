//------------------------------------------------------------------------------
// The contents of this file are subject to the nopCommerce Public License Version 1.0 ("License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at  http://www.nopCommerce.com/License.aspx. 
// 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. 
// See the License for the specific language governing rights and limitations under the License.
// 
// The Original Code is nopCommerce.
// The Initial Developer of the Original Code is NopSolutions.
// All Rights Reserved.
// 
// Contributor(s): _______. 
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Measures;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Security;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Content.Forums;
using NopSolutions.NopCommerce.BusinessLogic.Products;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class GlobalSettingsControl : BaseNopAdministrationUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.SelectTab(this.CommonSettingsTabs, this.TabID);
                FillDropDowns();
                BindData();
                TogglePanels();
            }
        }

        private void BindData()
        {
            txtStoreName.Text = SettingManager.StoreName;
            txtStoreURL.Text = SettingManager.StoreURL;
            cbStoreClosed.Checked = SettingManager.GetSettingValueBoolean("Common.StoreClosed");
            cbAnonymousCheckoutAllowed.Checked = CustomerManager.AnonymousCheckoutAllowed;

            cbStoreNameInTitle.Checked = SettingManager.GetSettingValueBoolean("SEO.IncludeStoreNameInTitle");
            txtDefaulSEOTitle.Text = SettingManager.GetSettingValue("SEO.DefaultTitle");
            txtDefaulSEODescription.Text = SettingManager.GetSettingValue("SEO.DefaultMetaDescription");
            txtDefaulSEOKeywords.Text = SettingManager.GetSettingValue("SEO.DefaultMetaKeywords");
            cbShowWelcomeMessage.Checked = SettingManager.GetSettingValueBoolean("Display.ShowWelcomeMessageOnMainPage");
            cbShowNewsHeaderRssURL.Checked = SettingManager.GetSettingValueBoolean("Display.ShowNewsHeaderRssURL");
            cbShowBlogHeaderRssURL.Checked = SettingManager.GetSettingValueBoolean("Display.ShowBlogHeaderRssURL");
            txtProductUrlRewriteFormat.Text = SettingManager.GetSettingValue("SEO.Product.UrlRewriteFormat");
            txtCategoryUrlRewriteFormat.Text = SettingManager.GetSettingValue("SEO.Category.UrlRewriteFormat");
            txtManufacturerUrlRewriteFormat.Text = SettingManager.GetSettingValue("SEO.Manufacturer.UrlRewriteFormat");
            txtNewsUrlRewriteFormat.Text = SettingManager.GetSettingValue("SEO.News.UrlRewriteFormat");
            txtBlogUrlRewriteFormat.Text = SettingManager.GetSettingValue("SEO.Blog.UrlRewriteFormat");
            txtTopicUrlRewriteFormat.Text = SettingManager.GetSettingValue("SEO.Topic.UrlRewriteFormat");


            txtMaxImageSize.Value = SettingManager.GetSettingValueInteger("Media.MaximumImageSize");
            txtProductThumbSize.Value = SettingManager.GetSettingValueInteger("Media.Product.ThumbnailImageSize");
            txtProductDetailSize.Value = SettingManager.GetSettingValueInteger("Media.Product.DetailImageSize");
            txtProductVariantSize.Value = SettingManager.GetSettingValueInteger("Media.Product.VariantImageSize");
            txtCategoryThumbSize.Value = SettingManager.GetSettingValueInteger("Media.Category.ThumbnailImageSize");
            txtManufacturerThumbSize.Value = SettingManager.GetSettingValueInteger("Media.Manufacturer.ThumbnailImageSize");
            cbShowCartImages.Checked = SettingManager.GetSettingValueBoolean("Display.ShowProductImagesOnShoppingCart");
            cbShowWishListImages.Checked = SettingManager.GetSettingValueBoolean("Display.ShowProductImagesOnWishList");
            txtShoppingCartThumbSize.Value = SettingManager.GetSettingValueInteger("Media.ShoppingCart.ThumbnailImageSize");


            MeasureWeight baseWeight = MeasureManager.BaseWeightIn;
            if (baseWeight != null)
                CommonHelper.SelectListItem(this.ddlBaseWeight, baseWeight.MeasureWeightID);
            MeasureDimension baseDimension = MeasureManager.BaseDimensionIn;
            if (baseDimension != null)
                CommonHelper.SelectListItem(this.ddlBaseDimension, baseDimension.MeasureDimensionID);


            txtAdminEmailAddress.Text = MessageManager.AdminEmailAddress;
            txtAdminEmailDisplayName.Text = MessageManager.AdminEmailDisplayName;
            txtAdminEmailHost.Text = MessageManager.AdminEmailHost;
            txtAdminEmailPort.Text = MessageManager.AdminEmailPort.ToString();
            txtAdminEmailUser.Text = MessageManager.AdminEmailUser;
            txtAdminEmailPassword.Text = MessageManager.AdminEmailPassword;
            cbAdminEmailEnableSsl.Checked = MessageManager.AdminEmailEnableSsl;
            cbAdminEmailUseDefaultCredentials.Checked = MessageManager.AdminEmailUseDefaultCredentials;


            txtEncryptionPrivateKey.Text = SettingManager.GetSettingValue("Security.EncryptionPrivateKey");
            cbEnableLoginCaptchaImage.Checked = SettingManager.GetSettingValueBoolean("Common.LoginCaptchaImageEnabled");
            cbEnableRegisterCaptchaImage.Checked = SettingManager.GetSettingValueBoolean("Common.RegisterCaptchaImageEnabled");


            CommonHelper.SelectListItem(this.ddlCustomerNameFormat, (int)CustomerManager.CustomerNameFormatting);
            cbShowCustomersLocation.Checked = CustomerManager.ShowCustomersLocation;
            cbShowCustomersJoinDate.Checked = CustomerManager.ShowCustomersJoinDate;
            cbAllowPM.Checked = ForumManager.AllowPrivateMessages;
            cbAllowViewingProfiles.Checked = CustomerManager.AllowViewingProfiles;
            cbCustomersAllowedToUploadAvatars.Checked = CustomerManager.AllowCustomersToUploadAvatars;
            cbDefaultAvatarEnabled.Checked = CustomerManager.DefaultAvatarEnabled;
            lblCurrentTimeZone.Text = DateTimeHelper.CurrentTimeZone.DisplayName;
            TimeZoneInfo defaultStoreTimeZone = DateTimeHelper.DefaultStoreTimeZone;
            if (defaultStoreTimeZone != null)
                CommonHelper.SelectListItem(this.ddlDefaultStoreTimeZone, defaultStoreTimeZone.Id);
            cbAllowCustomersToSetTimeZone.Checked = DateTimeHelper.AllowCustomersToSetTimeZone;


            cbUsernamesEnabled.Checked = CustomerManager.UsernamesEnabled;
            cbNewCustomerRegistrationDisabled.Checked = CustomerManager.NewCustomerRegistrationDisabled;
            cbAllowNavigationOnlyRegisteredCustomers.Checked = CustomerManager.AllowNavigationOnlyRegisteredCustomers;
            cbRegistrationEmailValidation.Checked = CustomerManager.RegistrationEmailValidation;
            cbEnableCompareProducts.Checked = ProductManager.CompareProductsEnabled;
            cbEnableWishlist.Checked = SettingManager.GetSettingValueBoolean("Common.EnableWishlist");
            cbEnableEmailAFriend.Checked = SettingManager.GetSettingValueBoolean("Common.EnableEmailAFirend");
            cbRecentlyViewedProductsEnabled.Checked = ProductManager.RecentlyViewedProductsEnabled;
            cbRecentlyAddedProductsEnabled.Checked = ProductManager.RecentlyAddedProductsEnabled;
            cbNotifyAboutNewProductReviews.Checked = ProductManager.NotifyAboutNewProductReviews;
            cbShowBestsellersOnHomePage.Checked = SettingManager.GetSettingValueBoolean("Display.ShowBestsellersOnMainPage");
            cbProductsAlsoPurchased.Checked = ProductManager.ProductsAlsoPurchasedEnabled;
            txtProductsAlsoPurchasedNumber.Value = ProductManager.ProductsAlsoPurchasedNumber;
        }

        private void FillDropDowns()
        {
            this.ddlBaseWeight.Items.Clear();
            MeasureWeightCollection measureWeightCollection = MeasureManager.GetAllMeasureWeights();
            foreach (MeasureWeight measureWeight in measureWeightCollection)
            {
                ListItem ddlMeasureWeightItem2 = new ListItem(measureWeight.Name, measureWeight.MeasureWeightID.ToString());
                this.ddlBaseWeight.Items.Add(ddlMeasureWeightItem2);
            }

            this.ddlBaseDimension.Items.Clear();
            MeasureDimensionCollection measureDimensionCollection = MeasureManager.GetAllMeasureDimensions();
            foreach (MeasureDimension measureDimension in measureDimensionCollection)
            {
                ListItem ddlMeasureDimensionItem2 = new ListItem(measureDimension.Name, measureDimension.MeasureDimensionID.ToString());
                this.ddlBaseDimension.Items.Add(ddlMeasureDimensionItem2);
            }

            this.ddlDefaultStoreTimeZone.Items.Clear();
            ReadOnlyCollection<TimeZoneInfo> timeZones = DateTimeHelper.GetSystemTimeZones();
            foreach (TimeZoneInfo timeZone in timeZones)
            {
                string timeZoneName = timeZone.DisplayName;
                ListItem ddlDefaultStoreTimeZoneItem2 = new ListItem(timeZoneName, timeZone.Id);
                this.ddlDefaultStoreTimeZone.Items.Add(ddlDefaultStoreTimeZoneItem2);
            }

            CommonHelper.FillDropDownWithEnum(this.ddlCustomerNameFormat, typeof(CustomerNameFormatEnum));
        }

        private void TogglePanels()
        {
            pnlDefaultAvatarEnabled.Visible = cbCustomersAllowedToUploadAvatars.Checked;
            pnlProductsAlsoPurchasedNumber.Visible = cbProductsAlsoPurchased.Checked;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    SettingManager.StoreName = txtStoreName.Text;
                    SettingManager.StoreURL = txtStoreURL.Text;
                    SettingManager.SetParam("Common.StoreClosed", cbStoreClosed.Checked.ToString());
                    CustomerManager.AnonymousCheckoutAllowed = cbAnonymousCheckoutAllowed.Checked;


                    SettingManager.SetParam("SEO.IncludeStoreNameInTitle", cbStoreNameInTitle.Checked.ToString());
                    SettingManager.SetParam("SEO.DefaultTitle", txtDefaulSEOTitle.Text);
                    SettingManager.SetParam("SEO.DefaultMetaDescription", txtDefaulSEODescription.Text);
                    SettingManager.SetParam("SEO.DefaultMetaKeywords", txtDefaulSEOKeywords.Text);
                    SettingManager.SetParam("Display.PublicStoreTheme", ctrlThemeSelector.SelectedTheme);
                    SettingManager.SetParam("Display.ShowWelcomeMessageOnMainPage", cbShowWelcomeMessage.Checked.ToString());
                    SettingManager.SetParam("Display.ShowNewsHeaderRssURL", cbShowNewsHeaderRssURL.Checked.ToString());
                    SettingManager.SetParam("Display.ShowBlogHeaderRssURL", cbShowBlogHeaderRssURL.Checked.ToString());
                    SettingManager.SetParam("SEO.Product.UrlRewriteFormat", txtProductUrlRewriteFormat.Text);
                    SettingManager.SetParam("SEO.Category.UrlRewriteFormat", txtCategoryUrlRewriteFormat.Text);
                    SettingManager.SetParam("SEO.Manufacturer.UrlRewriteFormat", txtManufacturerUrlRewriteFormat.Text);
                    SettingManager.SetParam("SEO.News.UrlRewriteFormat", txtNewsUrlRewriteFormat.Text);
                    SettingManager.SetParam("SEO.Blog.UrlRewriteFormat", txtBlogUrlRewriteFormat.Text);
                    SettingManager.SetParam("SEO.Topic.UrlRewriteFormat", txtTopicUrlRewriteFormat.Text);


                    SettingManager.SetParam("Media.MaximumImageSize", txtMaxImageSize.Value.ToString());
                    SettingManager.SetParam("Media.Product.ThumbnailImageSize", txtProductThumbSize.Value.ToString());
                    SettingManager.SetParam("Media.Product.DetailImageSize", txtProductDetailSize.Value.ToString());
                    SettingManager.SetParam("Media.Product.VariantImageSize", txtProductVariantSize.Value.ToString());
                    SettingManager.SetParam("Media.Category.ThumbnailImageSize", txtCategoryThumbSize.Value.ToString());
                    SettingManager.SetParam("Media.Manufacturer.ThumbnailImageSize", txtManufacturerThumbSize.Value.ToString());
                    SettingManager.SetParam("Display.ShowProductImagesOnShoppingCart", cbShowCartImages.Checked.ToString());
                    SettingManager.SetParam("Display.ShowProductImagesOnWishList", cbShowWishListImages.Checked.ToString());
                    SettingManager.SetParam("Media.ShoppingCart.ThumbnailImageSize", txtShoppingCartThumbSize.Value.ToString());


                    int baseWeightID = int.Parse(ddlBaseWeight.SelectedItem.Value);
                    MeasureManager.BaseWeightIn = MeasureManager.GetMeasureWeightByID(baseWeightID);
                    int baseDimensionID = int.Parse(ddlBaseDimension.SelectedItem.Value);
                    MeasureManager.BaseDimensionIn = MeasureManager.GetMeasureDimensionByID(baseDimensionID);


                    MessageManager.AdminEmailAddress = txtAdminEmailAddress.Text;
                    MessageManager.AdminEmailDisplayName = txtAdminEmailDisplayName.Text;
                    MessageManager.AdminEmailHost = txtAdminEmailHost.Text;
                    if (!String.IsNullOrEmpty(txtAdminEmailPort.Text))
                        MessageManager.AdminEmailPort = int.Parse(txtAdminEmailPort.Text);
                    else
                        MessageManager.AdminEmailPort = 0;
                    MessageManager.AdminEmailUser = txtAdminEmailUser.Text;
                    MessageManager.AdminEmailPassword = txtAdminEmailPassword.Text;
                    MessageManager.AdminEmailEnableSsl = cbAdminEmailEnableSsl.Checked;
                    MessageManager.AdminEmailUseDefaultCredentials = cbAdminEmailUseDefaultCredentials.Checked;


                    SettingManager.SetParam("Common.LoginCaptchaImageEnabled", cbEnableLoginCaptchaImage.Checked.ToString());
                    SettingManager.SetParam("Common.RegisterCaptchaImageEnabled", cbEnableRegisterCaptchaImage.Checked.ToString());


                    CustomerManager.CustomerNameFormatting = (CustomerNameFormatEnum)Enum.ToObject(typeof(CustomerNameFormatEnum), int.Parse(this.ddlCustomerNameFormat.SelectedItem.Value));
                    CustomerManager.ShowCustomersLocation = cbShowCustomersLocation.Checked;
                    CustomerManager.ShowCustomersJoinDate = cbShowCustomersJoinDate.Checked;
                    ForumManager.AllowPrivateMessages = cbAllowPM.Checked;
                    CustomerManager.AllowViewingProfiles = cbAllowViewingProfiles.Checked;
                    CustomerManager.AllowCustomersToUploadAvatars = cbCustomersAllowedToUploadAvatars.Checked;
                    CustomerManager.DefaultAvatarEnabled = cbDefaultAvatarEnabled.Checked;
                    string defaultStoreTimeZoneID = ddlDefaultStoreTimeZone.SelectedItem.Value;
                    DateTimeHelper.DefaultStoreTimeZone = DateTimeHelper.FindTimeZoneById(defaultStoreTimeZoneID);
                    DateTimeHelper.AllowCustomersToSetTimeZone = cbAllowCustomersToSetTimeZone.Checked;


                    CustomerManager.UsernamesEnabled = cbUsernamesEnabled.Checked;
                    CustomerManager.NewCustomerRegistrationDisabled = cbNewCustomerRegistrationDisabled.Checked;
                    CustomerManager.AllowNavigationOnlyRegisteredCustomers = cbAllowNavigationOnlyRegisteredCustomers.Checked;
                    CustomerManager.RegistrationEmailValidation = cbRegistrationEmailValidation.Checked;
                    ProductManager.CompareProductsEnabled = cbEnableCompareProducts.Checked;
                    SettingManager.SetParam("Common.EnableWishlist", cbEnableWishlist.Checked.ToString());
                    SettingManager.SetParam("Common.EnableEmailAFirend", cbEnableEmailAFriend.Checked.ToString());
                    ProductManager.RecentlyViewedProductsEnabled = cbRecentlyViewedProductsEnabled.Checked;
                    ProductManager.RecentlyAddedProductsEnabled = cbRecentlyAddedProductsEnabled.Checked;
                    ProductManager.NotifyAboutNewProductReviews = cbNotifyAboutNewProductReviews.Checked;
                    SettingManager.SetParam("Display.ShowBestsellersOnMainPage", cbShowBestsellersOnHomePage.Checked.ToString());
                    ProductManager.ProductsAlsoPurchasedEnabled = cbProductsAlsoPurchased.Checked;
                    ProductManager.ProductsAlsoPurchasedNumber = txtProductsAlsoPurchasedNumber.Value;

                    Response.Redirect(string.Format("GlobalSettings.aspx?TabID={0}", this.GetActiveTabID(this.CommonSettingsTabs)));
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void btnSendTestEmail_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    MailAddress from = new MailAddress(MessageManager.AdminEmailAddress, MessageManager.AdminEmailDisplayName);
                    MailAddress to = new MailAddress(txtSendEmailTo.Text);
                    MessageManager.SendEmail("Test message", "Test message", from, to);
                    lblSendTestEmailResult.Text = GetLocaleResourceString("Admin.GlobalSettings.MailSettings.SendTestEmailSuccess");
                }
                catch (Exception exc)
                {
                    lblSendTestEmailResult.Text = exc.Message;
                }
            }
        }

        protected void cbCustomersAllowedToUploadAvatars_CheckedChanged(object sender, EventArgs e)
        {
            TogglePanels();
        }

        protected void cbProductsAlsoPurchased_CheckedChanged(object sender, EventArgs e)
        {
            TogglePanels();
        }

        protected void btnChangeEncryptionPrivateKey_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    SecurityHelper.ChangeEncryptionPrivateKey(txtEncryptionPrivateKey.Text);
                    lblChangeEncryptionPrivateKeyResult.Text = GetLocaleResourceString("Admin.GlobalSettings.Security.ChangeKeySuccess");
                }
                catch (Exception exc)
                {
                    lblChangeEncryptionPrivateKeyResult.Text = exc.Message;
                }
            }
        }

        protected string TabID
        {
            get
            {
                return CommonHelper.QueryString("TabID");
            }
        }
    }
}