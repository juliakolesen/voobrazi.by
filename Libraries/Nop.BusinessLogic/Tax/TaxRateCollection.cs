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
using System.Linq;
using System.Text;


namespace NopSolutions.NopCommerce.BusinessLogic.Tax
{
    /// <summary>
    /// Represents a tax rate collection
    /// </summary>
    public partial class TaxRateCollection : BaseEntityCollection<TaxRate>
    {
        /// <summary>
        /// Find records
        /// </summary>
        /// <param name="CountryID">Country identifier</param>
        /// <param name="TaxCategoryID">Tax category identifier</param>
        /// <returns>Tax rates</returns>
        public TaxRateCollection FindTaxRates(int CountryID, int TaxCategoryID)
        {
            TaxRateCollection result = new TaxRateCollection();
            foreach (TaxRate taxRate in this)
            {
                if (taxRate.CountryID == CountryID && taxRate.TaxCategoryID == TaxCategoryID)
                    result.Add(taxRate);
            }
            return result;
        }
    }
}
