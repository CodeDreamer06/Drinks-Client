using Newtonsoft.Json.Linq;

namespace Drinks
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static string baseUrl = "https://www.thecocktaildb.com/api/json/v1/1/";

        static async Task Main(string[] args)
        {
            var actions = new Dictionary<int, Func<Task>>
            {
                { 1, GetCategories },
                { 2, GetGlasses },
                { 3, GetIngredients },
                { 4, GetAlcoholicContent }
            };

            Console.WriteLine(HelperMethods.helpMessage);
            var command = Convert.ToInt32(Console.ReadLine());
            await actions[command]();
        }

        private static Task GetAlcoholicContent()
        {
            throw new NotImplementedException();
        }

        private static Task GetIngredients()
        {
            throw new NotImplementedException();
        }

        private static Task GetGlasses()
        {
            throw new NotImplementedException();
        }

        static async Task GetCategories()
        {
            string json = await client.GetStringAsync(baseUrl + "list.php?c=list");

            var drinks = JObject.Parse(json)["drinks"]?
                .Select(drink => drink["strCategory"]).ToArray();

            Console.WriteLine("Choose a category:");
            HelperMethods.DisplayDataAsTable(drinks!);

            try
            {
                var categoryId = Convert.ToInt32(Console.ReadLine()) - 1;
                var categoryName = drinks?[categoryId]?.ToString().Replace(" ", "_");
                await GetCocktailsByCategory(categoryName!);
            }

            catch (FormatException)
            {
                Console.WriteLine("Please enter a valid number.");
            }

            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Please enter a number from the given list.");
            }
        }

        static async Task GetCocktailsByCategory(string category)
        {
            string json = await client.GetStringAsync(baseUrl + "filter.php?c=" + category);

            var cocktails = JObject.Parse(json)["drinks"]?
                .Select(drink => drink["strDrink"]).ToArray();

            Console.WriteLine("Displaying cocktails for " + category.Replace("_", " "));
            HelperMethods.DisplayDataAsTable(cocktails!);
        }
    }
}