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


namespace NopSolutions.NopCommerce.DataAccess.Shipping
{
    /// <summary>
    /// Represents a ShippingByWeightAndCountry
    /// </summary>
    public partial class DBShippingByWeightAndCountry : BaseDBEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of the DBShippingByWeightAndCountry class
        /// </summary>
        public DBShippingByWeightAndCountry()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the ShippingByWeightAndCountry identifier
        /// </summary>
        public int ShippingByWeightAndCountryID { get; set; }

        /// <summary>
        /// Gets or sets the shipping method identifier
        /// </summary>
        public int ShippingMethodID { get; set; }

        /// <summary>
        /// Gets or sets the country identifier
        /// </summary>
        public int CountryID { get; set; }

        /// <summary>
        /// Gets or sets the "from" value
        /// </summary>
        public decimal From { get; set; }

        /// <summary>
        /// Gets or sets the "to" value
        /// </summary>
        public decimal To { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use percentage
        /// </summary>
        public bool UsePercentage { get; set; }

        /// <summary>
        /// Gets or sets the shipping charge percentage
        /// </summary>
        public decimal ShippingChargePercentage { get; set; }

        /// <summary>
        /// Gets or sets the shipping charge amount
        /// </summary>
        public decimal ShippingChargeAmount { get; set; }

        #endregion 
    }

}
