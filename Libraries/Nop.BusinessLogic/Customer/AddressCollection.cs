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
    /// Represents an address collection
    /// </summary>
    public partial class AddressCollection : BaseEntityCollection<Address>
    {
        /// <summary>
        /// Finds an address
        /// </summary>
        /// <param name="FirstName">First name</param>
        /// <param name="LastName">Last name</param>
        /// <param name="PhoneNumber">Phone number</param>
        /// <param name="Email">Email</param>
        /// <param name="FaxNumber">Fax number</param>
        /// <param name="Company">Company</param>
        /// <param name="Address1">Address 1</param>
        /// <param name="Address2">Address 2</param>
        /// <param name="City">City</param>
        /// <param name="StateProvinceID">State/province identifier</param>
        /// <param name="ZipPostalCode">Zip postal code</param>
        /// <param name="CountryID">Country identifier</param>
        /// <returns>Address</returns>
        public Address FindAddress(string FirstName, string LastName, string PhoneNumber, string Email,
            string FaxNumber, string Company,string Address1, string Address2, string City, int StateProvinceID,
            string ZipPostalCode, int CountryID)
        {
            return this.Find((a) => a.FirstName == FirstName && a.LastName == LastName && a.PhoneNumber == PhoneNumber &&
                 a.Email == Email && a.FaxNumber == FaxNumber && a.Company == Company && 
                 a.Address1 == Address1 && a.Address2 == Address2 &&
                 a.City == City && a.StateProvinceID == StateProvinceID 
                 && a.ZipPostalCode == ZipPostalCode && a.CountryID == CountryID);
        }
    }
}
