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


namespace NopSolutions.NopCommerce.DataAccess.Orders
{
    /// <summary>
    /// Represents an order average report line
    /// </summary>
    public partial class DBOrderAverageReportLine : BaseDBEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of the DBOrderAverageReportLine class
        /// </summary>
        public DBOrderAverageReportLine()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the sum today total
        /// </summary>
        public decimal SumTodayOrders { get; set; }

        /// <summary>
        /// Gets or sets the today count
        /// </summary>
        public decimal CountTodayOrders { get; set; }

        /// <summary>
        /// Gets or sets the sum this week total
        /// </summary>
        public decimal SumThisWeekOrders { get; set; }

        /// <summary>
        /// Gets or sets the this week count
        /// </summary>
        public decimal CountThisWeekOrders { get; set; }

        /// <summary>
        /// Gets or sets the sum this month total
        /// </summary>
        public decimal SumThisMonthOrders { get; set; }

        /// <summary>
        /// Gets or sets the this month count
        /// </summary>
        public decimal CountThisMonthOrders { get; set; }

        /// <summary>
        /// Gets or sets the sum this year total
        /// </summary>
        public decimal SumThisYearOrders { get; set; }

        /// <summary>
        /// Gets or sets the this year count
        /// </summary>
        public decimal CountThisYearOrders { get; set; }

        /// <summary>
        /// Gets or sets the sum all time total
        /// </summary>
        public decimal SumAllTimeOrders { get; set; }

        /// <summary>
        /// Gets or sets the all time count
        /// </summary>
        public decimal CountAllTimeOrders { get; set; }

        #endregion
    }

}
