using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NopSolutions.NopCommerce.DataAccess.Colors
{
    public partial class DBColors
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of the DBColors class
        /// </summary>
        public DBColors()
        {
        }
        #endregion

        #region Properties

        public string ColorName { get; set; }

        public long ColorArgb { get; set; }

        public int ColorID { get; set; }

        #endregion Properties
    }
}
