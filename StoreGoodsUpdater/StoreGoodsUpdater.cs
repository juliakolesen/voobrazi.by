using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Xml;
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
                        string[] cols = row.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        if (cols[2].Contains("."))
                            continue;
                        string sku = cols[0];
                        decimal price = decimal.Parse(cols[1]);
                        int quantity = int.Parse(cols[2]);

                        using (var command = new SqlCommand(UpdateQuery, connection))
                        {
                            command.Parameters.Add(new SqlParameter("@Sku", sku));
                            command.Parameters.Add(new SqlParameter("@Price", price));
                            command.Parameters.Add(new SqlParameter("@StockQuantity", quantity));

                            command.ExecuteNonQuery();
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
