using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NopSolutions.NopCommerce.DataAccess.Colors;
using NopSolutions.NopCommerce.DataAccess;
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic.Products.Specs;

namespace NopSolutions.NopCommerce.BusinessLogic.Colors
{
    public partial class ColorManager
    {
        #region Utilities

        private static ColorItem DBMapping(DBColors dbColor)
        {
            if (dbColor == null)
                return null;

            ColorItem color = new ColorItem();
            color.ColorName = dbColor.ColorName;
            color.ColorArgb = dbColor.ColorArgb;
            color.ColorID = dbColor.ColorID;

            return color;
        }

        #endregion

        private const string Color = "цвет";

        #region Methods
        public static void DeleteColor(string colorName, string paletteFolderPath)
        {
            ColorItem color = GetColorByColorName(colorName);
            if (color != null)
            {
                DBProviderManager<DBColorsProvider>.Provider.DeleteColor(colorName);
                string palettePath = String.Format("{0}{1}.jpeg", paletteFolderPath, color.ColorID);
                if (File.Exists(palettePath))
                {
                    File.Delete(palettePath);
                }
            }
        }


        public static ColorItem GetColorByColorName(string colorName)
        {
            DBColors dbColor = DBProviderManager<DBColorsProvider>.Provider.GetColorByColorName(colorName);
            ColorItem color = DBMapping(dbColor);
            return color;
        }

        public static bool InsertColor(string colorName, int colorArgb)
        {
            if (String.IsNullOrEmpty(colorName))
                return false;

            bool result = DBProviderManager<DBColorsProvider>.Provider.InsertColor(colorName, colorArgb);
            return result;
        }

        public static void UpdateColor(string oldName, string newName)
        {
            if (String.IsNullOrEmpty(oldName) || String.IsNullOrEmpty(newName))
                return;

            DBProviderManager<DBColorsProvider>.Provider.UpdateName(oldName, newName);
        }

        public static SpecificationAttribute GetColorSpecificationAttribute()
        {
            SpecificationAttributeCollection sa = SpecificationAttributeManager.GetSpecificationAttributes();
            SpecificationAttribute specAttribute = sa.Find(x => x.Name.ToLower() == Color);
            return specAttribute;
        }

        public static List<ColorItem> GetColorsBySAOID(List<int> specificationAttributeOptionIDs)
        {
            List<ColorItem> colors = new List<ColorItem>();
            foreach(var id in specificationAttributeOptionIDs)
            {
                SpecificationAttributeOption sao = SpecificationAttributeManager.GetSpecificationAttributeOptionByID(id);
                ColorItem colorItem = ColorManager.GetColorByColorName(sao.Name);
				if (colorItem != null)
				{
					colors.Add(colorItem);
				}
            }

            return colors;
        }

        #endregion
    }
}
