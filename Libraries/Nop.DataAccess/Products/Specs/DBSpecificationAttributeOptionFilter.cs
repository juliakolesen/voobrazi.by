using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NopSolutions.NopCommerce.DataAccess.Products.Specs
{
    /// <summary>
    /// Represents a specification attribute option filter
    /// </summaryd
    public class DBSpecificationAttributeOptionFilter : BaseDBEntity
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DBSpecificationAttributeOptionFilter()
        { }

        /// <summary>
        /// Gets or sets the SpecificationAttributeID
        /// </summary>
        public int SpecificationAttributeID { get; set; }

        /// <summary>
        /// Gets or sets the SpecificationAttributeName
        /// </summary>
        public string SpecificationAttributeName { get; set; }

        /// <summary>
        /// Gets or sets the DisplayOrder
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the SpecificationAttributeOptionID
        /// </summary>
        public int SpecificationAttributeOptionID { get; set; }

        /// <summary>
        /// Gets or sets the SpecificationAttributeOptionName
        /// </summary>
        public string SpecificationAttributeOptionName { get; set; }
    }
}
