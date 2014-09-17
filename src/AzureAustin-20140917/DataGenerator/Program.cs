using System;
using System.Threading.Tasks;

namespace DataGenerator
{
    public class Program
    {
        

        static void Main(string[] args)
        {
            try
            {
                RunAsync().Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException.Message);
                Console.WriteLine(e.InnerException.InnerException.Message);
            }
            Console.ReadLine();
        }

        private static async Task RunAsync()
        {
            //generate X number of listings locally
            //int numberOfListingsToCreate = 100;
            //DataFactory f = new DataFactory();
            //Listing[] listings = f.CreateListings(numberOfListingsToCreate);

            //create the datastore and persist the listings
            //Console.WriteLine("Creating the database");
            //DocumentDbOperations.CreateDatabase(listings).Wait();
            //Console.WriteLine("Database created");

            //var result = await DocumentDbOperations.GetRubicons();
            var result = await DocumentDbOperations.GetHardTops();
            foreach (Listing rubicon in result)
            {
                Console.WriteLine(String.Format("Package: {0} Color: {1} Dealer: {2}",
                    rubicon.Package, rubicon.Color, rubicon.Dealer.name));
                foreach (Option option in rubicon.Options)
                {
                    Console.WriteLine("\t" + option.Name);
                }
                Console.WriteLine("");
            }

            //await DocumentDbOperations.DeleteDatabase(Keys.ListingsDatabaseName);
        }
    }
}
