using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Xml;

using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Tasks;
using OpenPOP.MIME;
using OpenPOP.POP3;

namespace Scjaarge.NopTasks
{
    public class StoreGoodsUpdaterTask : ITask
    {
        private const string UpdateProductVariantQuery = "UPDATE Nop_ProductVariant SET Price=@Price, StockQuantity=@StockQuantity WHERE SKU=@Sku";
        private const string UpdateProductQuery = "UPDATE Nop_Product SET ShortDescription=@ShortDescription, FullDescription=@FullDescription WHERE ProductID=@ProductID";
        private const string SelectProductQuery = "SELECT p.ProductID, p.ShortDescription, p.FullDescription FROM Nop_ProductVariant pv INNER JOIN Nop_Product p ON pv.ProductID=p.ProductID WHERE SKU=@Sku";
        private const string CheckSaoQuery = @"SELECT count([ProductSpecificationAttributeID]) FROM [Nop_Product_SpecificationAttribute_Mapping] 
				                          WHERE [ProductID]=@productId AND [SpecificationAttributeOptionID]=@saoID";
        private const string InsertSaoQuery = @"INSERT INTO [Nop_Product_SpecificationAttribute_Mapping]
           ([ProductID] ,[SpecificationAttributeOptionID] ,[AllowFiltering] ,[ShowOnProductPage] ,[DisplayOrder])
           VALUES (@productId, @saoID, 1, 0, 1)";

        public void Execute(XmlNode node)
        {
            if (node.Attributes == null) return;

            XmlAttribute server = node.Attributes["Server"];
            XmlAttribute port = node.Attributes["Port"];
            XmlAttribute username = node.Attributes["Username"];
            XmlAttribute password = node.Attributes["Password"];
            XmlAttribute connectionStringName = node.Attributes["ConnectionStringName"];

            var popClient = new POPClient();
            popClient.Connect(server.Value, int.Parse(port.Value), false);
            popClient.Authenticate(username.Value, password.Value);
            int count = popClient.GetMessageCount();

            if (count <= 0)
            {
                popClient.Disconnect();
                return;
            }

            for (int i = count; i >= 1; i -= 1)
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName.Value].ConnectionString))
                {
                    connection.Open();
                    Message message = popClient.GetMessage(i);
                    string[] rows = message.MessageBody[0].Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string row in rows)
                    {
                        try
                        {
                            string[] cols = row.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                            if (cols[2].Contains("."))
                                continue;
                            string sku = cols[0];
                            decimal price = decimal.Parse(cols[1]);
                            int quantity = int.Parse(cols[2]);

                            using (var command = new SqlCommand(UpdateProductVariantQuery, connection))
                            {
                                command.Parameters.Add(new SqlParameter("@Sku", sku));
                                command.Parameters.Add(new SqlParameter("@Price", price));
                                command.Parameters.Add(new SqlParameter("@StockQuantity", quantity));

                                command.ExecuteNonQuery();
                            }

                            if (cols.Length == 5)
                            {
                                int height;
                                int diameter;
                                bool heightParsed = int.TryParse(cols[3], out height);
                                bool diameterParsed = int.TryParse(cols[4], out diameter);

                                using (var command = new SqlCommand(UpdateProductVariantQuery, connection))
                                {
                                    command.Parameters.Add(new SqlParameter("@Sku", sku));
                                    Product product = null;

                                    int saoHeightId = 0;
                                    if (heightParsed && height != 0)
                                    {
                                        product = GetProduct(connection, sku);

                                        if (height < 11) saoHeightId = 54;
                                        else if (height >= 11 && height < 16) saoHeightId = 55;
                                        else if (height >= 16 && height < 21) saoHeightId = 56;
                                        else if (height >= 21 && height < 26) saoHeightId = 57;
                                        else if (height >= 26 && height < 31) saoHeightId = 58;
                                        else if (height >= 31 && height < 50) saoHeightId = 64;
                                        else if (height >= 50) saoHeightId = 60;

                                        if (product != null && !SaoExists(connection, product.Id, saoHeightId))
                                        {
                                            InsertSao(connection, product.Id, saoHeightId);
                                        }

                                    }

                                    int saoDiameterId = 0;
                                    if (diameterParsed && diameter != 0)
                                    {
                                        if (product == null)
                                            product = GetProduct(connection, sku);

                                        if (diameter < 11) saoDiameterId = 16;
                                        else if (diameter >= 11 && diameter < 16) saoDiameterId = 18;
                                        else if (diameter >= 16 && diameter < 21) saoDiameterId = 48;
                                        else if (diameter >= 21 && diameter < 26) saoDiameterId = 49;
                                        else if (diameter >= 26 && diameter < 31) saoDiameterId = 50;
                                        else if (diameter >= 31 && diameter < 50) saoDiameterId = 51;
                                        else if (diameter >= 50) saoDiameterId = 52;

                                        if (product != null && !SaoExists(connection, product.Id, saoDiameterId))
                                        {
                                            InsertSao(connection, product.Id, saoDiameterId);
                                        }
                                    }

                                    if (product != null)
                                    {
                                        string oldShortDescription = product.ShortDescription;
                                        string oldFullDescription = product.FullDescription;

                                        if (diameterParsed && heightParsed)
                                        {
                                            if (!product.ShortDescription.ToLower().Contains("(см)"))
                                                product.ShortDescription += (product.ShortDescription == string.Empty ? "" : "<br />") + string.Format("{0}X{1}(см)", height, diameter);
                                            if (!product.FullDescription.ToLower().Contains("высота"))
                                                product.FullDescription += (product.FullDescription == string.Empty ? "" : "<br />") + string.Format("Высота - {0} см", height);
                                            if (!product.FullDescription.ToLower().Contains("диаметр"))
                                                product.FullDescription += (product.FullDescription == string.Empty ? "" : "<br />") + string.Format("Диаметр - {0} см", diameter);
                                        }
                                        else if (heightParsed)
                                        {
                                            if (!product.ShortDescription.Contains(string.Format("{0}X-(см)", height)))
                                                product.ShortDescription += (product.ShortDescription == string.Empty ? "" : "<br />") + string.Format("{0}X-(см)", height);
                                            if (!product.FullDescription.ToLower().Contains("высота"))
                                                product.FullDescription += (product.FullDescription == string.Empty ? "" : "<br />") + string.Format("Высота - {0} см", height);
                                        }
                                        else if (diameterParsed)
                                        {
                                            if (!product.ShortDescription.Contains(string.Format("-X{0}(см)", diameter)))
                                                product.ShortDescription += (product.ShortDescription == string.Empty ? "" : "<br />") + string.Format("-X{0}(см)", diameter);
                                            if (!product.FullDescription.ToLower().Contains("диаметр"))
                                                product.FullDescription += (product.FullDescription == string.Empty ? "" : "<br />") + string.Format("Диаметр - {0} см", diameter);
                                        }

                                        if (oldShortDescription != product.ShortDescription || oldFullDescription != product.FullDescription)
                                            UpdateProduct(connection, product);
                                    }
                                }
                            }
                        }
                        catch (Exception exc)
                        {
                            LogManager.InsertLog(LogTypeEnum.AdministrationArea, string.Format("Error while sync with 1C. The line is '{0}'.", row), exc);
                        }
                    }
                }

                popClient.DeleteMessage(i);
                popClient.Disconnect();
            }
            NopCache.Clear();
        }

        private static Product GetProduct(SqlConnection connection, string sku)
        {
            using (var command = new SqlCommand(SelectProductQuery, connection))
            {
                command.Parameters.Add(new SqlParameter("@Sku", sku));
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new Product { Id = reader.GetInt32(0), ShortDescription = reader.GetString(1), FullDescription = reader.GetString(2) };
                    }
                }
            }
            return null;
        }

        private static void UpdateProduct(SqlConnection connection, Product product)
        {
            using (var command = new SqlCommand(UpdateProductQuery, connection))
            {
                command.Parameters.Add(new SqlParameter("@ProductID", product.Id));
                command.Parameters.Add(new SqlParameter("@ShortDescription", product.ShortDescription));
                command.Parameters.Add(new SqlParameter("@FullDescription", product.FullDescription));

                command.ExecuteNonQuery();
            }
        }

        private static bool SaoExists(SqlConnection connection, int productId, int saoId)
        {
            using (var command = new SqlCommand(CheckSaoQuery, connection))
            {
                command.Parameters.Add(new SqlParameter("@productId", productId));
                command.Parameters.Add(new SqlParameter("@saoID", saoId));

                return Convert.ToInt32(command.ExecuteScalar()) == 0 ? false : true;
            }
        }

        private static void InsertSao(SqlConnection connection, int productId, int saoId)
        {
            using (var command = new SqlCommand(InsertSaoQuery, connection))
            {
                command.Parameters.Add(new SqlParameter("@productId", productId));
                command.Parameters.Add(new SqlParameter("@saoID", saoId));
                command.ExecuteNonQuery();
            }
        }

        private class Product
        {
            public int Id;
            public string ShortDescription;
            public string FullDescription;
        }
    }
}
