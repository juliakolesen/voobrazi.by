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
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Profile;



namespace NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts
{
    /// <summary>
    /// Represents a discount
    /// </summary>
    public partial class Discount : BaseEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of the Discount class
        /// </summary>
        public Discount()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the discount identifier
        /// </summary>
        public int DiscountID { get; set; }

        /// <summary>
        /// Gets or sets the discount type identifier
        /// </summary>
        public int DiscountTypeID { get; set; }

        /// <summary>
        /// Gets or sets the discount requirement identifier
        /// </summary>
        public int DiscountRequirementID { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use percentage
        /// </summary>
        public bool UsePercentage { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage
        /// </summary>
        public decimal DiscountPercentage { get; set; }

        /// <summary>
        /// Gets or sets the discount amount
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Gets or sets the discount start date and time
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the discount end date and time
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether discount requires coupon code
        /// </summary>
        public bool RequiresCouponCode { get; set; }

        /// <summary>
        /// Gets or sets the coupon code
        /// </summary>
        public string CouponCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a value indicating whether the discount is active now
        /// </summary>
        /// <returns>A value indicating whether the discount is active now</returns>
        public bool IsActive()
        {
            return IsActive(string.Empty);
        }

        /// <summary>
        /// Gets a value indicating whether the discount is active now
        /// </summary>
        /// <param name="CouponCodeToValidate">Coupon code to validate</param>
        /// <returns>A value indicating whether the discount is active now</returns>
        public bool IsActive(string CouponCodeToValidate)
        {
            if (this.RequiresCouponCode && !String.IsNullOrEmpty(this.CouponCode))
            {
                if (CouponCodeToValidate != this.CouponCode)
                    return false;
            }
            DateTime now = DateTimeHelper.ConvertToUtcTime(DateTime.Now);
            bool isActive = (!Deleted) && (StartDate.CompareTo(now) < 0) && (EndDate.CompareTo(now) > 0);
            return isActive;
        }


        /// <summary>
        /// Gets the discount amount for the specified value
        /// </summary>
        /// <param name="price">Price</param>
        /// <returns>The discount amount</returns>
        public decimal GetDiscountAmount(decimal price)
        {
            decimal result = decimal.Zero;
            if (UsePercentage)
                result = Math.Round((decimal)((((float)price) * ((float)this.DiscountPercentage)) / 100f), 2);
            else
                result = Math.Round(this.DiscountAmount, 2);

            if (result < decimal.Zero)
                result = decimal.Zero;

            return result;
        }

        /// <summary>
        /// Checks customer role requirement for customer
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns></returns>
        public bool CheckCustomerRoleRequirement(int CustomerID)
        {
            if (this.DiscountRequirement != DiscountRequirementEnum.MustBeAssignedToCustomerRole)
                return true;

            //rewrite logic. Load customer roles by discount id (and not discounts by customer role id)
            //CustomerRoleCollection customerRoles = CustomerManager.GetCustomerRolesByCustomerID(CustomerID);
            //foreach (CustomerRole _customerRole in customerRoles)
            //    foreach (Discount _discount in _customerRole.Discounts)
            //    {
            //        if (_discount.Name == this.Name)
            //        {
            //            return true;
            //        }
            //    }

            CustomerRoleCollection customerRoles = CustomerManager.GetCustomerRolesByCustomerID(CustomerID);
            CustomerRoleCollection assignedRoles = this.CustomerRoles;
            foreach (CustomerRole _customerRole in customerRoles)
                foreach (CustomerRole _assignedRole in assignedRoles)
                {
                    if (_customerRole.Name == _assignedRole.Name)
                    {
                        return true;
                    }
                }
            return false;
        }
        #endregion

        #region Custom Properties

        /// <summary>
        /// Gets or sets the discount type
        /// </summary>
        public DiscountTypeEnum DiscountType
        {
            get
            {
                return (DiscountTypeEnum)DiscountTypeID;
            }
            set
            {
                DiscountTypeID = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the discount requirement
        /// </summary>
        public DiscountRequirementEnum DiscountRequirement
        {
            get
            {
                return (DiscountRequirementEnum)DiscountRequirementID;
            }
            set
            {
                DiscountRequirementID = (int)value;
            }
        }

        /// <summary>
        /// Gets the customer role assigned to discount
        /// </summary>
        public CustomerRoleCollection CustomerRoles
        {
            get
            {
                return CustomerManager.GetCustomerRolesByDiscountID(DiscountID);
            }
        }
        #endregion
    }
}
