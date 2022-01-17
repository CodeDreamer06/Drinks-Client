using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;

namespace Drinks
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            await GetCategories();
        }

        static async Task GetCategories()
        {
            Console.WriteLine("Choose a category:");
            string json = await client.GetStringAsync("https://www.thecocktaildb.com/api/json/v1/1/list.php?c=list");

            var drinks = JObject.Parse(json)["drinks"];

            for (int i = 0; i < drinks!.Count(); i++)
            {
                Console.WriteLine(new String('-', 26));
                Console.WriteLine($"| {i} {drinks?[i]?["strCategory"]}".PadRight(25) + "|");
            }

            Console.WriteLine(new String('-', 26));
        }
    }
}