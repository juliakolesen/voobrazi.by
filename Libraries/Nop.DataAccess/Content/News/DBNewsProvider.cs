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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;

namespace NopSolutions.NopCommerce.DataAccess.Content.NewsManagement
{
    /// <summary>
    /// Acts as a base class for deriving custom news provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/NewsProvider")]
    public abstract partial class DBNewsProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Gets a news
        /// </summary>
        /// <param name="NewsID">The news identifier</param>
        /// <returns>News</returns>
        public abstract DBNews GetNewsByID(int NewsID);
        
        /// <summary>
        /// Deletes a news
        /// </summary>
        /// <param name="NewsID">The news identifier</param>
        public abstract void DeleteNews(int NewsID);
        
        /// <summary>
        /// Gets news item collection
        /// </summary>
        /// <param name="LanguageID">Language identifier. 0 if you want to get all news</param>
        /// <param name="NewsCount">News item count. 0 if you want to get all news</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>News item collection</returns>
        public abstract DBNewsCollection GetNews(int LanguageID, int NewsCount, bool showHidden);
        
        /// <summary>
        /// Inserts a news item
        /// </summary>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="Title">The news title</param>
        /// <param name="Short">The short text</param>
        /// <param name="Full">The full text</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="AllowComments">A value indicating whether the entity allows comments</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>News item</returns>
        public abstract DBNews InsertNews(int LanguageID, string Title, string Short,
            string Full, bool Published, bool AllowComments, DateTime CreatedOn);

        /// <summary>
        /// Updates the news item
        /// </summary>
        /// <param name="NewsID">The news identifier</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="Title">The news title</param>
        /// <param name="Short">The short text</param>
        /// <param name="Full">The full text</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="AllowComments">A value indicating whether the entity allows comments</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>News item</returns>
        public abstract DBNews UpdateNews(int NewsID, int LanguageID, string Title, string Short,
            string Full, bool Published, bool AllowComments, DateTime CreatedOn);

        /// <summary>
        /// Gets a news comment
        /// </summary>
        /// <param name="NewsCommentID">News comment identifer</param>
        /// <returns>News comment</returns>
        public abstract DBNewsComment GetNewsCommentByID(int NewsCommentID);
        
        /// <summary>
        /// Gets a news comment collection by news identifier
        /// </summary>
        /// <param name="NewsID">The news identifier</param>
        /// <returns>News comment collection</returns>
        public abstract DBNewsCommentCollection GetNewsCommentsByNewsID(int NewsID);

        /// <summary>
        /// Deletes a news comment
        /// </summary>
        /// <param name="NewsCommentID">The news comment identifier</param>
        public abstract void DeleteNewsComment(int NewsCommentID);
        
        /// <summary>
        /// Gets all news comments
        /// </summary>
        /// <returns>News comment collection</returns>
        public abstract DBNewsCommentCollection GetAllNewsComments();

        /// <summary>
        /// Inserts a news comment
        /// </summary>
        /// <param name="NewsID">The news identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="Title">The title</param>
        /// <param name="Comment">The comment</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>News comment</returns>
        public abstract DBNewsComment InsertNewsComment(int NewsID, int CustomerID, string Title,
            string Comment, DateTime CreatedOn);

        /// <summary>
        /// Updates the news comment
        /// </summary>
        /// <param name="NewsCommentID">The news comment identifier</param>
        /// <param name="NewsID">The news identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="Title">The title</param>
        /// <param name="Comment">The comment</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>News comment</returns>
        public abstract DBNewsComment UpdateNewsComment(int NewsCommentID, int NewsID, int CustomerID, string Title,
            string Comment, DateTime CreatedOn);
        #endregion
    }
}
