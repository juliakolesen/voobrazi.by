﻿//------------------------------------------------------------------------------
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

using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Tax;

namespace NopSolutions.NopCommerce.Tax
{
    /// <summary>
    /// General tax provider
    /// </summary>
    public class GeneralTaxProvider : ITaxProvider
    {
        /// <summary>
        /// Gets tax rate
        /// </summary>
        /// <param name="calculateTaxRequest">Tax calculation request</param>
        /// <param name="Error">Error</param>
        /// <returns>Tax</returns>
        public decimal GetTaxRate(CalculateTaxRequest calculateTaxRequest, ref string Error)
        {
            if (calculateTaxRequest.Address == null)
            {
                Error = "Address is not set";
                return 0;
            }

            decimal taxRate = decimal.Zero;

            int taxClassID = 0;
            if (calculateTaxRequest.TaxClassID > 0)
            {
                taxClassID = calculateTaxRequest.TaxClassID;
            }
            else
            {
                ProductVariant productVariant = calculateTaxRequest.Item;
                if (productVariant != null)
                {
                    taxClassID = productVariant.TaxCategoryID;
                }
            }
            taxRate = GetTaxRate(calculateTaxRequest.Address, taxClassID);

            return taxRate;
        }

        /// <summary>
        /// Gets a tax rate
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="TaxCategoryID">The tax category identifier</param>
        /// <returns>Tax rate</returns>
        protected decimal GetTaxRate(Address address, int TaxCategoryID)
        {
            int CountryID = 0;
            int StateProvinceID = 0;

            if (address.Country != null)
            {
                CountryID = address.Country.CountryID;
            }
            if (address.StateProvince != null)
            {
                StateProvinceID = address.StateProvince.StateProvinceID;
            }
            decimal tr = decimal.Zero;
            TaxRateCollection taxRates = TaxRateManager.GetAllTaxRates(TaxCategoryID, CountryID, StateProvinceID, address.ZipPostalCode);
            if (taxRates.Count > 0)
                tr += taxRates[0].Percentage;
            return tr;
        }
    }
}