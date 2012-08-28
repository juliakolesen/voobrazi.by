

using NopSolutions.NopCommerce.BusinessLogic.Audit;

namespace Scjaarge.NopTasks
{
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Xml;
    using NopSolutions.NopCommerce.BusinessLogic.Caching;
    using NopSolutions.NopCommerce.BusinessLogic.Tasks;

    public class StoreGoodsUnpublisherTask : ITask
    {
        private const string PublishProductQuery = @"UPDATE p SET p.Published=1 FROM Nop_Product p INNER JOIN Nop_ProductVariant pv 
                                                        ON (p.ProductId = pv.ProductId 
                                                            AND pv.StockQuantity > 0 
                                                            AND p.ProductId IN (SELECT DISTINCT ProductID FROM Nop_Product_Category_Mapping WHERE CategoryId IN ({0})))";

        private const string UnpublishProductQuery = @"UPDATE p SET p.Published = 0 FROM Nop_Product p INNER JOIN Nop_ProductVariant pv 
                                                        ON (p.ProductId = pv.ProductId 
                                                        AND pv.StockQuantity = 0
    	                                                AND p.ProductId NOT IN (SELECT DISTINCT ProductID FROM Nop_Product_Category_Mapping WHERE CategoryId IN ({0})))";

        public void Execute(XmlNode node)
        {
            if (node.Attributes == null) return;

            XmlAttribute categoryIDsToSkip = node.Attributes["CategoryIDsToSkip"];
            XmlAttribute connectionStringName = node.Attributes["ConnectionStringName"];

            foreach (string categoryID in categoryIDsToSkip.Value.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                int tmpCategoryID;
                if (!int.TryParse(categoryID, out tmpCategoryID))
                    return;
            }

            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName.Value].ConnectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(string.Format(PublishProductQuery, categoryIDsToSkip.Value.Replace(";", ",")), connection))
                        command.ExecuteNonQuery();
                    //using (var command = new SqlCommand(string.Format(UnpublishProductQuery, categoryIDsToSkip.Value.Replace(";", ",")), connection))
                    //    command.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                LogManager.InsertLog(LogTypeEnum.AdministrationArea, "StoreGoodsUnpublisherTask exception.", exc);
            }

            NopCache.Clear();
        }
    }
}
