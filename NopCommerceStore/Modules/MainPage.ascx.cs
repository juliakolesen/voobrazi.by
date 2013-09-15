using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using NopSolutions.NopCommerce.BusinessLogic.Content.Topics;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class MainPage : BaseNopUserControl
    {
        private LocalizedTopicCollection LocalizedTopicCollection;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            LocalizedTopicCollection = TopicManager.TopicLocalizedLoadAllOnHomePage();
            SetTopicName(topicDefaultPage1, 1, div1);
            SetTopicName(topicDefaultPage2, 2, div2);
            SetTopicName(topicDefaultPage3, 3, div3);
            SetTopicName(topicDefaultPage4, 4, div4);
        }

        private void SetTopicName(TopicControl topic, int number, HtmlGenericControl div)
        {
            LocalizedTopic lTopic = LocalizedTopicCollection.FirstOrDefault(t => t.ShowOnHomePage == number);
            if (lTopic != null)
            {
                topic.TopicName = lTopic.Topic.Name;
                if (number == 1)
                {
                    div.Attributes.Add("style", "background: url(../../images/ff_images/mainimg_logo_big_background.gif) center center no-repeat ;");
                }
            }
            else
            {
                div.Attributes.Add("style",
                                   number == 1
                                       ? "background: url(../../images/ff_images/mainimg_logo_big.gif) center center no-repeat ;"
                                       : "background: url(../../images/ff_images/mainimg_logo.gif) center center no-repeat ;");
            }
        }
    }
}