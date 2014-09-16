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
        private Dealer[] _dealers;
        private string[] _colors;
        private string[] _types;
        private string[] _options;
        private string[] _packages;
        private Dictionary<string, string> _images;
        private static Random _colorMeRandom;
        private static Random _typeMeRandom;
        private static Random _dealerMeRandom;
        private static Random _optionsMeRandom;
        private static Random _packageMeRandom;
        private static bool _createdDb;

        static void Main(string[] args)
        {
            Program p = new Program();
            p.InitData();

            List<Listing> listings = new List<Listing>();

            for (int i = 0; i < 10000; i++)
            {
                //get a Listing
                var listing = p.GetListing();
                listings.Add(listing);

                //save to documentdb

                //save to search

                Console.WriteLine(listing.Image);
            }

            var client = new DocumentClient(new Uri(Keys.Uri), Keys.PrimaryKey);
            if (!_createdDb)
            {
                ResourceResponse<Database> database = await client.CreateDatabaseAsync(
                    new Database
                    {
                        Id = "FamilyRegistry"
                    });
            }

            Console.ReadLine();
        }

        public static void WriteDB(Listing listing)
        {
            var client = new DocumentClient(new Uri(Keys.Uri), Keys.PrimaryKey);   
            if (!_createdDb)
            {
                ResourceResponse<Database> database = await client.CreateDatabaseAsync(
                    new Database
                    {
                        Id = "FamilyRegistry"
                    });
            }
             
        }

        public Listing GetListing()
        {
            Listing l = new Listing();
            l.Id = Guid.NewGuid();
            l.Color = GetColor();
            l.Options = GetOptions();
            l.Package = GetPackage();
            l.Type = GetType();
            l.Image = GetImage(l.Color, l.Type, l.Package);
            l.Dealer = GetDealer();

            return l;
        }

        public void InitData()
        {
            _createdDb = false;

            List<Dealer> dealers = new List<Dealer>();
            dealers.Add(new Dealer() { url = "http://www.amazon.com", name = "Amazon Motors" });
            dealers.Add(new Dealer() { url = "http://www.craigslist.com", name = "Craigslist" });
            dealers.Add(new Dealer() { url = "http://www.monstermotors.com", name = "Monster Motors" });
            dealers.Add(new Dealer() { url = "http://www.carsdirect.com", name = "Cars Direct" });
            dealers.Add(new Dealer() { url = "http://www.autotrader.com", name = "Auto Trader" });
            dealers.Add(new Dealer() { url = "http://www.cars.com", name = "Cars.com" });
            _dealers = dealers.ToArray();

            List<String> options = new List<string>();
            options.Add("soft top, half metal doors, 5 speed transmission, rock-trac part-time 4wd, normal duty suspension");
            options.Add("hard top, full metal doors, sway bar disconnect, fuel tank skid plate, transfer plate skid plate, hill start assist, electronic stability control, electronic roll mitigation, 6 speed transmission, command-trac shift on the fly 4wd, heavy duty suspension");
            options.Add("soft top, half metal doors, sway bar disconnect, fuel tank skid plate, transfer plate skid plate, hill start assist, electronic stability control, electronic roll mitigation, 6 speed transmission, command-trac shift on the fly 4wd, heavy duty suspension");
            options.Add("hard top, full metal doors, 5 speed transmission, rock-trac part-time 4wd, normal duty suspension");
            _options = options.ToArray();

            _colors = "ampd,anvil,black,billet,copperhead,dune,firecracker red,flame red,granite,silver,white".Split(',');
            _types = "wrangler,wrangler unlimited".Split(',');
            _packages = "sport,rubicon,sahara,sport s".Split(',');
            _colorMeRandom = new Random();
            _dealerMeRandom = new Random();
            _typeMeRandom = new Random();
            _optionsMeRandom = new Random();
            _packageMeRandom = new Random();

        }
        public string GetColor()
        {
            
            return _colors[_colorMeRandom.Next(0,_colors.Length)];
        }

        public string GetOptions()
        {
            return _options[_optionsMeRandom.Next(0,_options.Length)];
        }

        public string GetPackage()
        {
            return _packages[_packageMeRandom.Next(0,_packages.Length)];
        }

        public string GetType()
        {
            return _types[_typeMeRandom.Next(0, _types.Length)];
        }

        public string GetImage(string color, string type, string package)
        {
            return type.Replace(" ", "-") + "-" + package.Replace(" ", "-") + "-" + color.Replace(" ", "-") + ".jpg";
        }

        public Dealer GetDealer()
        {
            return _dealers[_dealerMeRandom.Next(0, _dealers.Length)];
        }
    }

    public class Listing
    {
        public string Color { get; set; }
        public string Options { get; set; }
        public string Package { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
        public Dealer Dealer { get; set; }
        public Guid Id { get; set; }
    }

    public class Dealer
    {
        public string url { get; set; }
        public string name { get; set; }
    }
}
