using Microsoft.Azure.Cosmos.Table;
using System;
using System.Threading.Tasks;

namespace DemoAzureTable
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Azure Table - Exemplo");

            var storageConnectionString = "ConnectionString here";
            var tableName = "Customer";

            CloudStorageAccount storageAccount;
            storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tableClient.GetTableReference(tableName);

            var customer = new CustomerEntity("Prado", "Gabriel")
            {
                Email = "gab.silva@reply.com",
                Telefone = "11 999999999"
            };

            MergeUser(table, customer).Wait();
        }

        public static async Task MergeUser(CloudTable table, CustomerEntity customer)
        {
            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(customer);

            TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
            var insertedCustomer = result.Result as CustomerEntity;

            Console.WriteLine("Usuário adicionado - {0}", insertedCustomer.RowKey);
        }
    }

    public class CustomerEntity : TableEntity
    {
        public CustomerEntity() {} 

        public CustomerEntity(string lastName, string firstName)
        {
            PartitionKey = lastName;
            RowKey = firstName;
        }

        public string Email { get; set; }
        public string Telefone { get; set; }
    }
}
