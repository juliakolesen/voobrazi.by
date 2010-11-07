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
using System.ComponentModel;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.Content.Forums;
 
namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class ForumSelectorControl : BaseNopUserControl
    {
        private int selectedForumId;

        public void BindData()
        {
            string prefix = "--";
            ddlForums.Items.Clear();
            ForumGroupCollection forumGroups = ForumManager.GetAllForumGroups();
            foreach (ForumGroup forumGroup in forumGroups)
            {
                ListItem forumGroupItem = new ListItem(forumGroup.Name, "0");
                this.ddlForums.Items.Add(forumGroupItem);

                ForumCollection forums = forumGroup.Forums;
                foreach (Forum forum in forums)
                {
                    ListItem forumItem = new ListItem(prefix + forum.Name, forum.ForumID.ToString());
                    this.ddlForums.Items.Add(forumItem);
                    if (forum.ForumID == this.selectedForumId)
                        forumItem.Selected = true;
                }
            }

            this.ddlForums.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        
        public string CssClass
        {
            get
            {
                return ddlForums.CssClass;
            }
            set
            {
                ddlForums.CssClass = value;
            }
        }

        public int SelectedForumId
        {
            get
            {
                if (this.ddlForums.SelectedItem != null)
                    return int.Parse(this.ddlForums.SelectedItem.Value);
                else
                    return 0;
            }
            set
            {
                this.selectedForumId = value;
            }
        }
    }
}