using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NopSolutions.NopCommerce.DataAccess.Colors
{
    [DBProviderSectionName("nopDataProviders/ColorProvider")]
    public abstract partial class DBColorsProvider : BaseDBProvider
    {
        #region Methods

        public abstract DBColors GetColorByColorName(string colorName);

        public abstract bool InsertColor(string colorName, int colorArgb);

        public abstract void DeleteColor(string colorName);

        public abstract void UpdateName(string oldName, string newName);
        #endregion Methods
    }
}
