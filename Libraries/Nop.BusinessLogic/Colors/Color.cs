using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NopSolutions.NopCommerce.BusinessLogic.Colors
{
    public partial class ColorItem : BaseEntity
    {
          #region Ctor
        /// <summary>
        /// Creates a new instance of the color class
        /// </summary>
        public ColorItem()
        {
        }
        #endregion

        public string ColorName { get; set; }

        public long ColorArgb { get; set; }

        public int ColorID { get; set; }
    }
}
