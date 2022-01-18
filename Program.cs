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

        static async Task GetAlcoholicContent()
        {
            await ObtainCocktails("Alcoholic", "Choose an option:");
        }

        static async Task GetIngredients()
        {
            await ObtainCocktails("Ingredient1", "Choose an Ingredient:");
        }

        static async Task GetGlasses()
        {
            await ObtainCocktails("Glass", "Choose a glass:");
        }

        static async Task GetCategories()
        {
            await ObtainCocktails("Category", "Choose a category:");
        }

        static async Task ObtainCocktails(string requestType, string message)
        {
            var drinks = await GetAllCockTails(requestType);

            Console.WriteLine(message);
            HelperMethods.DisplayDataAsTable(drinks!);

            try
            {
                var itemId = Convert.ToInt32(Console.ReadLine()) - 1;
                var itemName = drinks?[itemId]?.ToString().Replace(" ", "_");

                Console.WriteLine($"Displaying cocktails for {drinks?[itemId]}:");
                await GetCockTailsByFilter($"filter.php?{requestType.ToLower()[0]}={itemName!}");
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

        static async Task<JToken[]> GetAllCockTails(string listType)
        {
            string filterUrl = $"{baseUrl}list.php?{listType.ToLower()[0]}=list";
            string json = await client.GetStringAsync(filterUrl);

            var drinks = JObject.Parse(json)["drinks"]?
                .Select(drink => drink["str" + listType]).ToArray();

            return drinks!;
        }

        static async Task GetCockTailsByFilter(string filter)
        {
            string json = await client.GetStringAsync(baseUrl + filter);

            var cocktails = JObject.Parse(json)["drinks"]?
                .Select(drink => drink["strDrink"]).ToArray();

            HelperMethods.DisplayDataAsTable(cocktails!);
        }
    }
}