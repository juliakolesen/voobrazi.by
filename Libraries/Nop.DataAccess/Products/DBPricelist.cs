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
using System.Text;
using NopSolutions.NopCommerce.DataAccess;

namespace NopSolutions.NopCommerce.DataAccess.Products
{
    /// <summary>
    /// Represents a pricelist
    /// </summary>
    public partial class DBPricelist : BaseDBEntity
    {
        #region Ctor
        /// <summary>
		/// Creates a new instance of the DBPricelist class
		/// </summary>
		public DBPricelist()
		{
		
		}
		#endregion
				
		#region Properties
		/// <summary>
		/// Gets or sets the Pricelist identifier
		/// </summary>
		public int PricelistID { get; set; }
		
		/// <summary>
		/// Gets or sets the Mode of list creation (Export all, assigned only, assigned only with special price)
		/// </summary>
		public int ExportModeID { get; set; }
		
		/// <summary>
		/// Gets or sets the CSV or XML
		/// </summary>
		public int ExportTypeID { get; set; }

        /// <summary>
        /// Gets or sets the Affiliate connected to this pricelist (optional), links will be created with AffiliateID
        /// </summary>
        public int AffiliateID { get; set; }

		/// <summary>
		/// Gets or sets the Displayedname
		/// </summary>
		public string DisplayName { get; set; }
		
		/// <summary>
		/// Gets or sets the shortname to identify the pricelist
		/// </summary>
		public string ShortName { get; set; }
		
		/// <summary>
		/// Gets or sets the unique identifier to get pricelist "anonymous"
		/// </summary>
		public string PricelistGuid { get; set; }
		
		/// <summary>
		/// Gets or sets the how long will the pricelist be in cached before new creation
		/// </summary>
		public int CacheTime { get; set; }
		
		/// <summary>
		/// Gets or sets the what localization will be used (numeric formats, etc.) en-US, de-DE etc.
		/// </summary>
		public string FormatLocalization { get; set; }
		
		/// <summary>
		/// Gets or sets the Displayed description
		/// </summary>
		public string Description { get; set; }
		
		/// <summary>
		/// Gets or sets the Admin can put some notes here, not displayed in public
		/// </summary>
		public string AdminNotes { get; set; }
		
		/// <summary>
		/// Gets or sets the Headerline of the exported file (plain text)
		/// </summary>
		public string Header { get; set; }
		
		/// <summary>
		/// Gets or sets the template for an exportet productvariant, uses delimiters and replacement strings
		/// </summary>
		public string Body { get; set; }
		
		/// <summary>
		/// Gets or sets the Footer line of the exportet file (plain text)
		/// </summary>
		public string Footer { get; set; }

        /// <summary>
        /// Gets or sets the type of price adjustment (if used) (relative or absolute)
        /// </summary>
        public int PriceAdjustmentTypeID { get; set; }

        /// <summary>
        /// Gets or sets the price will be adjusted by this amount (in accordance with PriceAdjustmentType)
        /// </summary>
        public decimal PriceAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the use individual adjustment, if available, or override
        /// </summary>
        public bool OverrideIndivAdjustment { get; set; }

		/// <summary>
		/// Gets or sets the when was this record originally created
		/// </summary>
		public DateTime CreatedOn { get; set; }
		
		/// <summary>
		/// Gets or sets the last time this recordset was updated
		/// </summary>
		public DateTime UpdatedOn { get; set; }
		
		#endregion
	}
}

