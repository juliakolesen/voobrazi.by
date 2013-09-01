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
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Content.Topics;

namespace NopSolutions.NopCommerce.Web.Modules
{
	public partial class TopicControl : BaseNopUserControl
	{
		protected override void OnLoad(EventArgs e)
		{
			BindData();
			base.OnLoad(e);
		}

		private void BindData()
		{
			LocalizedTopic localizedTopic = TopicManager.GetLocalizedTopic(TopicName, NopContext.Current.WorkingLanguage.LanguageID);
			if (localizedTopic != null)
			{
				if (!string.IsNullOrEmpty(localizedTopic.Body))
				{
					lBody.Text = localizedTopic.Body;
				}
				else
				{
					lBody.Visible = false;
				}
			}
			else
				Visible = false;
		}

		public string TopicName
		{
			get
			{
				object obj2 = ViewState["TopicName"];
				if (obj2 != null)
					return (string)obj2;

				return string.Empty;
			}
			set
			{
				ViewState["TopicName"] = value;
			}
		}
	}
}
