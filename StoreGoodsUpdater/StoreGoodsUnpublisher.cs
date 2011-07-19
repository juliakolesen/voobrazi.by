

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
        private const string PublishQuery = @"UPDATE Nop_ProductVariant SET Published=1 WHERE StockQuantity > 0 OR
	                                                ProductId IN (SELECT ProductID FROM Nop_Product_Category_Mapping WHERE CategoryId IN ({0}))";
        private const string UnpublishQuery = @"UPDATE Nop_ProductVariant SET Published = 0 
                                                WHERE StockQuantity = 0 AND 
	                                                ProductId NOT IN (SELECT ProductID FROM Nop_Product_Category_Mapping WHERE CategoryId IN ({0}))";

        public void Execute(XmlNode node)
        {
            if (node.Attributes == null) return;

            XmlAttribute categoryIDsToSkip = node.Attributes["CategoryIDsToSkip"];
            XmlAttribute connectionStringName = node.Attributes["ConnectionStringName"];

            int tmpCategoryID;
            foreach (string categoryID in categoryIDsToSkip.Value.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                if (!int.TryParse(categoryID, out tmpCategoryID))
                    return;
            }

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName.Value].ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(string.Format(PublishQuery, categoryIDsToSkip.Value.Replace(";", ",")), connection))
                    command.ExecuteNonQuery();
                using (var command = new SqlCommand(string.Format(UnpublishQuery, categoryIDsToSkip.Value.Replace(";", ",")), connection))
                    command.ExecuteNonQuery();
            }

            NopCache.Clear();
        }
    }
}
