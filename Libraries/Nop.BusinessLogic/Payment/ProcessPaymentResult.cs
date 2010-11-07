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

namespace NopSolutions.NopCommerce.BusinessLogic.Payment
{
    /// <summary>
    /// Represents a ProcessPaymentResult
    /// </summary>
    public partial class ProcessPaymentResult
    {
        #region Fields
        private string avsResult = string.Empty;
        private string authorizationTransactionID = string.Empty;
        private string authorizationTransactionCode = string.Empty;
        private string authorizationTransactionResult = string.Empty;
        private string captureTransactionID = string.Empty;
        private string captureTransactionResult = string.Empty;
        private string error = string.Empty;
        private string fullError = string.Empty;
        private PaymentStatusEnum paymentStatus = PaymentStatusEnum.Pending;
        private bool allowStoringCreditCardNumber = false;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets an AVS result
        /// </summary>
        public string AVSResult
        {
            get
            {
                return avsResult;
            }
            set
            {
                avsResult = value;
            }
        }

        /// <summary>
        /// Gets or sets the authorization transaction ID
        /// </summary>
        public string AuthorizationTransactionID
        {
            get
            {
                return authorizationTransactionID;
            }
            set
            {
                authorizationTransactionID = value;
            }
        }

        /// <summary>
        /// Gets or sets the authorization transaction code
        /// </summary>
        public string AuthorizationTransactionCode
        {
            get
            {
                return authorizationTransactionCode;
            }
            set
            {
                authorizationTransactionCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the authorization transaction result
        /// </summary>
        public string AuthorizationTransactionResult
        {
            get
            {
                return authorizationTransactionResult;
            }
            set
            {
                authorizationTransactionResult = value;
            }
        }

        /// <summary>
        /// Gets or sets the capture transaction ID
        /// </summary>
        public string CaptureTransactionID
        {
            get
            {
                return captureTransactionID;
            }
            set
            {
                captureTransactionID = value;
            }
        }

        /// <summary>
        /// Gets or sets the capture transaction result
        /// </summary>
        public string CaptureTransactionResult
        {
            get
            {
                return captureTransactionResult;
            }
            set
            {
                captureTransactionResult = value;
            }
        }

        /// <summary>
        /// Gets or sets an error message for customer, or String.Empty if no errors
        /// </summary>
        public string Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;
            }
        }

        /// <summary>
        /// Gets or sets a full error message, or String.Empty if no errors
        /// </summary>
        public string FullError
        {
            get
            {
                return fullError;
            }
            set
            {
                fullError = value;
            }
        }

        /// <summary>
        /// Gets or sets a payment status after processing
        /// </summary>
        public PaymentStatusEnum PaymentStatus
        {
            get
            {
                return paymentStatus;
            }
            set
            {
                paymentStatus = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether storing of credit card number, CVV2 is allowed
        /// </summary>
        public bool AllowStoringCreditCardNumber
        {
            get
            {
                return allowStoringCreditCardNumber;
            }
            set
            {
                allowStoringCreditCardNumber = value;
            }
        }
        #endregion
    }
}
