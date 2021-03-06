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



namespace NopSolutions.NopCommerce.DataAccess.Content.Topics
{
    /// <summary>
    /// Represents a topic
    /// </summary>
    public partial class DBTopic : BaseDBEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of the DBTopic class
        /// </summary>
        public DBTopic()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the topic identifier
        /// </summary>
        public int TopicID { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Key words adds to title (SEO)
        /// </summary>
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Description for SEO
        /// </summary>
        public string MetaDescription { get; set; }
        
        ///<summary>
        /// Title for SEO
        /// </summary>
        public string MetaTitle { get; set; }
        
        #endregion
    }

}
