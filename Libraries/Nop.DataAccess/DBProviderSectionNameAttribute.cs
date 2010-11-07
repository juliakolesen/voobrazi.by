using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NopSolutions.NopCommerce.DataAccess
{
    public partial class DBProviderSectionNameAttribute : Attribute
    {
        #region Ctor

        /// <summary>
        /// Creates a new instance of DBProviderSectionNameAttribute 
        /// </summary>
        /// <param name="sectionName">The name of the provider configuration section</param>
        public DBProviderSectionNameAttribute(string sectionName)
        {
            this.SectionName = sectionName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the section name
        /// </summary>
        public string SectionName { get; private set; }

        #endregion
    }
}
