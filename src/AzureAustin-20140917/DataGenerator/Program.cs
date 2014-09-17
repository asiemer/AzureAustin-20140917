using System;
using System.Linq;
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
            RunAsync().Wait();
            Console.ReadLine();
        }

        private static async Task RunAsync()
        {
            int numberOfListingsToCreate = 10;
            DataFactory f = new DataFactory();
            Listing[] listings = f.CreateListings(numberOfListingsToCreate);

            //Console.WriteLine("Creating the database");
            //CreateDatabase(listings).Wait();
            //Console.WriteLine("Database created");

            var rubicons = await GetRubicons();
            foreach (Listing rubicon in rubicons)
            {
                Console.WriteLine(String.Format("Package: {0} Color: {1} Options: {2} Dealer: {3}",
                    rubicon.Package, rubicon.Color, rubicon.Options, rubicon.Dealer.name));
            }

            //DeleteDatabase(Keys.ListingsRegistry);
        }
        public static async Task<Listing[]> GetRubicons()
        {
            //get the client
            var client = GetClient();

            //get the database
            var db = await RetrieveOrCreateDatabaseAsync(Keys.ListingRegistryName);
            Console.WriteLine("DB SelfLink: " + db.SelfLink);

            //get the collection
            var collection = await RetrieveOrCreateCollectionAsync(db.SelfLink, Keys.ListingCollectionName);

            var query = client.CreateDocumentQuery<Listing>(collection.SelfLink, "SELECT * FROM Listings l").ToArray();
            var rubicons = query.ToArray();

            return rubicons;
        }

        public static DocumentClient GetClient()
        {
            var client = new DocumentClient(new Uri(Keys.Uri), Keys.PrimaryKey);
            return client;
        }

        private static async Task<Database> RetrieveOrCreateDatabaseAsync(string id)
        {
            // Try to retrieve the database (Microsoft.Azure.Documents.Database) whose Id is equal to databaseId            
            var database = GetClient().CreateDatabaseQuery().Where(db => db.Id == id).AsEnumerable().FirstOrDefault();

            // If the previous call didn't return a Database, it is necessary to create it
            if (database == null)
            {
                database = await GetClient().CreateDatabaseAsync(new Database { Id = id });
                Console.WriteLine("Created Database: id - {0} and selfLink - {1}", database.Id, database.SelfLink);
            }

            return database;
        }

        private static async Task<DocumentCollection> RetrieveOrCreateCollectionAsync(string databaseSelfLink, string id)
        {
            // Try to retrieve the collection (Microsoft.Azure.Documents.DocumentCollection) whose Id is equal to collectionId
            var collection = GetClient().CreateDocumentCollectionQuery(databaseSelfLink).Where(c => c.Id == id).ToArray().FirstOrDefault();

            // If the previous call didn't return a Collection, it is necessary to create it
            if (collection == null)
            {
                collection = await GetClient().CreateDocumentCollectionAsync(databaseSelfLink, new DocumentCollection { Id = id });
            }

            return collection;
        }

        public static async Task DeleteDatabase(string selfLink)
        {
            var client = GetClient();
            await client.DeleteDatabaseAsync(selfLink);
        }

        public static async Task CreateDatabase(Listing[] listings)
        {
            var client = GetClient();
            
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
