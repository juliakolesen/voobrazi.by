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
                TopicManager.TopicLocalizedLoadAllOnHomePage().Where(topic => topic.ShowOnHomePage > 7).ToArray();
            //7 - count small topics on the main page
            if (topicCollection.Any())
            {
                topics.DataSource = topicCollection;
                topics.DataBind();
            }
            else
            {
                divSlider.Attributes.Add("style", "background: url(../../images/ff_images/mainimg_slideshow.jpg) center center no-repeat ;");
            }
        }

        protected string GetTopicName(LocalizedTopic localizedTopic)
        {
            return localizedTopic.Topic.Name;
        }
    }
}