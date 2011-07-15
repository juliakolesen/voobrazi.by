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
        private const string UpdateQuery = "UPDATE Nop_ProductVariant SET Price=@Price, StockQuantity=@StockQuantity WHERE SKU=@Sku";

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

                            decimal height = -1;
                            decimal diameter = -1;
                            if (cols.Length == 5)
                            {
                                height = int.Parse(cols[3]);
                                diameter = int.Parse(cols[4]);
                            }

                            using (var command = new SqlCommand(UpdateQuery, connection))
                            {
                                command.Parameters.Add(new SqlParameter("@Sku", sku));
                                command.Parameters.Add(new SqlParameter("@Price", price));
                                command.Parameters.Add(new SqlParameter("@StockQuantity", quantity));

                                command.ExecuteNonQuery();

                                if (height != -1)
                                {
                                    int attributeId;
                                    if (height < 11) attributeId = 1;
                                    else if (height >= 11 && height < 16) attributeId = 2;
                                    else if (height >= 16 && height < 21) attributeId = 3;
                                    else if (height >= 21 && height < 26) attributeId = 4;
                                    else if (height >= 26 && height < 31) attributeId = 5;
                                    else if (height >= 31 && height < 50) attributeId = 6;
                                    else if (height >= 50) attributeId = 7;
                                }

                                if (diameter != -1)
                                {
                                    int attributeId;
                                    if (height < 11) attributeId = 1;
                                    else if (height >= 11 && height < 16) attributeId = 1;
                                    else if (height >= 16 && height < 21) attributeId = 1;
                                    else if (height >= 21 && height < 26) attributeId = 1;
                                    else if (height >= 26 && height < 31) attributeId = 1;
                                    else if (height >= 31 && height < 50) attributeId = 1;
                                    else if (height >= 50) attributeId = 1;
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
    }
}
