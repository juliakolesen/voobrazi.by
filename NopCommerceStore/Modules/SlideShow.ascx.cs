using System;
using System.Linq;
using NopSolutions.NopCommerce.BusinessLogic.Content.Topics;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class SlideShow : BaseNopUserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            LocalizedTopic[] topicCollection =
                TopicManager.TopicLocalizedLoadAllOnHomePage().Where(topic => topic.ShowOnHomePage > 4).ToArray();
            //7 - count small topics on the main page
            if (topicCollection.Any())
            {
                topics.DataSource = topicCollection;
                topics.DataBind();
                divSlider.Attributes.Add("style", "background: url(../../images/ff_images/background_slideshow.gif) center center no-repeat ;");
            }
            else
            {
                divSlider.Attributes.Add("style", "background: url(../../images/ff_images/mainimg_slideshow.gif) center center no-repeat ;");
            }
        }

        protected string GetTopicName(LocalizedTopic localizedTopic)
        {
            return localizedTopic.Topic.Name;
        }
    }
}