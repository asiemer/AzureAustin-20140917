using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DataGenerator
{
    public class Program
    {
        

        static void Main(string[] args)
        {
            int numberOfListingsToCreate = 10;
            DataFactory f = new DataFactory();
            Listing[] listings = f.CreateListings(numberOfListingsToCreate);
            
            var createDatabase = Task.Factory.StartNew(async () => await CreateDatabase(listings)).Unwrap();
            Console.WriteLine("Creating the database");
            createDatabase.Wait();
            Console.WriteLine("Database created");


            Console.ReadLine();
        }

        public static async Task CreateDatabase(Listing[] listings)
        {
            var client = new DocumentClient(new Uri(Keys.Uri), Keys.PrimaryKey);
            
            Database database = await client.CreateDatabaseAsync(
               new Database
               {
                   Id = "ListingsRegistry"
               });

            Console.WriteLine("Self link for new database");
            Console.WriteLine(database.SelfLink);

            DocumentCollection documentCollection = new DocumentCollection
            {
                Id = "ListingCollection"
            };
            documentCollection = await client.CreateDocumentCollectionAsync(database.SelfLink, documentCollection);

            foreach (Listing listing in listings)
            {
                await client.CreateDocumentAsync(documentCollection.SelfLink, listing);
            }
        }
    }
}
