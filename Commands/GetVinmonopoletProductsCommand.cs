using Newtonsoft.Json.Linq;

class GetVinmonopoletProductsCommand
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task ExecuteAsync()
    {
        foreach (var category in Config.Categories)
        {
            await GetProducts(category.Key, category.Value);
        }
    }

    public static async Task GetProducts(string productCategory, int type)
    {
        Console.WriteLine("Fetching product files for category: " + productCategory);
        int currentPage = 0;
        var noOfProducts = 0;
        do
        {
            var url = string.Format(Config.baseUrl, currentPage, productCategory);
            string responseBody = await FetchDataAsync(url);

            // Parse the JSON string
            JObject parsedJson = JObject.Parse(responseBody);

            // Extract the pagination object
            var pageInfo = parsedJson["pagination"];

            if (pageInfo != null)
            {
                JToken? productsToken = parsedJson["products"];
                if (productsToken is JArray products)
                {
                    noOfProducts = products.Count;
                    var currentPageToken = pageInfo.Value<int>("currentPage");
                    var currentPageNumber = currentPageToken;

                    Console.WriteLine($"\rCurrent Page: {currentPageNumber}, No of products: {noOfProducts}");

                    SaveToFile(productCategory, responseBody, currentPage);
                }
                else
                {
                    Console.WriteLine("Products data is missing or invalid.");
                    throw new InvalidOperationException("Products data is missing or invalid.");
                }
            }
            else
            {
                Console.WriteLine("Pagination info not found.");
                throw new InvalidOperationException("Pagination info not found.");
            }
            currentPage++;
        } while (noOfProducts > 0);
    }
    private static async Task<string> FetchDataAsync(string url)
    {
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    private static void SaveToFile(string category, string data, int pageNumber)
    {
        string fileName = Config.GetSafeFileName($"{category}_{pageNumber}.json");
        File.WriteAllText($"{Config.jsonProductsDirectory}/{fileName}", data);
    }
}

