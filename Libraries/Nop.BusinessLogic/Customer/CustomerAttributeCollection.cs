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


namespace NopSolutions.NopCommerce.BusinessLogic.CustomerManagement
{
    /// <summary>
    /// Represents a customer attribute collection
    /// </summary>
    public partial class CustomerAttributeCollection : BaseEntityCollection<CustomerAttribute>
    {
        #region Methods
        /// <summary>
        /// Returns a customer attribute that has the specified attribute value
        /// </summary>
        /// <param name="Key">Customer attribute key</param>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>A customer attribute that has the specified attribute value; otherwise null</returns>
        public CustomerAttribute FindAttribute(string Key, int CustomerID)
        {
            foreach (CustomerAttribute customerAttribute in this)
                if (customerAttribute.Key == Key && customerAttribute.CustomerID == CustomerID)
                    return customerAttribute;
            return null;
        }
        #endregion
    }
}
