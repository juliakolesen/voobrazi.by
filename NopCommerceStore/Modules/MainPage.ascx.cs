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
            SetTopicName(topicDefaultPage5, 5, div5);
            SetTopicName(topicDefaultPage6, 6, div6);
            SetTopicName(topicDefaultPage7, 7, div7);
        }

        private void SetTopicName(TopicControl topic, int number, HtmlGenericControl div)
        {
            LocalizedTopic lTopic = LocalizedTopicCollection.FirstOrDefault(t => t.ShowOnHomePage == number);
            if (lTopic != null)
            {
                topic.TopicName = lTopic.Topic.Name;
            }
            else
            {
                div.Attributes.Add("style", "background: url(../../images/ff_images/mainimg_logo.jpg) center center no-repeat ;");
            }
        }
    }
}