using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Newtonsoft.Json.Linq;

public static class GetVinmonopoletProductsDataCommand
{
    public static void Erlend()
    {
        var aromatisert_vin = 0;
        var bare = $"{aromatisert_vin}";
    }
    public static void ExecuteAsync()
    {
        if (Config.jsonProductsDirectory == null)
        {
            throw new InvalidOperationException("Config.jsonProductsDirectory is not set.");
        }

        if (Config.Categories == null || Config.Categories.Count == 0)
        {
            throw new InvalidOperationException("Config.Categories is not set or is empty.");
        }

        Console.WriteLine("Config.Categories:");
        var categoriesList = new List<KeyValuePair<string, int>>(Config.Categories);
        for (int i = 0; i < categoriesList.Count; i++)
        {
            var category = categoriesList[i];
            Console.WriteLine($"Key: {category.Key}, Value: {category.Value}");
        }

        for (int i = 0; i < categoriesList.Count; i++)
        {
            var category = categoriesList[i];

            if (category.Key == null)
            {
                throw new InvalidOperationException("Category key is null.");
            }
            int fileNo = 0;

            while (true)
            {
                var jsonFilePath = $"{Config.jsonProductsDirectory}/{category.Key}_{fileNo}.json";
                if (!File.Exists(jsonFilePath))
                {
                    break;
                }
                Console.WriteLine(jsonFilePath);

                // Read and parse the JSON file
                string jsonData = File.ReadAllText(jsonFilePath);
                JObject jsonObject = JObject.Parse(jsonData);
                JArray products = (JArray)jsonObject["productSearchResult"]["products"];

                int noOfProducts = ExtractProducts(category.Value, products);

                fileNo++;
            }
        }
    }

    private static int ExtractProducts(int type, JArray products)
    {
        // Extract product details
        var productList = new List<VinoProduct>();
        foreach (var product in products)
        {
            var productCode = product["code"];
            var id = long.Parse(productCode.ToString());

            var productName = product["name"].ToString();
            var volume = product["volume"]["formattedValue"].ToString();
            var price = ((int)(decimal.Parse(product["price"]["value"].ToString()) * 100)).ToString();

            productList.Add(new VinoProduct
            {
                id = id,
                type = type,
                name = productName,
                volume = volume,
                price = price
            });
        }
        InsertProductsIntoDatabase(productList);
        return productList.Count;
    }

    private static void InsertProductsIntoDatabase(List<VinoProduct> products)
    {
        using (var connection = new SQLiteConnection($"Data Source={Config.databaseFilePath};Version=3;"))
        {
            connection.Open();

            foreach (var product in products)
            {
                string insertQuery = @"
                    INSERT OR REPLACE INTO vino (id, type, name, volume, price)
                    VALUES (@id, @type, @name, @volume, @price)";

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", product.id);
                    command.Parameters.AddWithValue("@type", product.type);
                    command.Parameters.AddWithValue("@name", product.name);
                    command.Parameters.AddWithValue("@volume", product.volume);
                    command.Parameters.AddWithValue("@price", product.price);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

public class VinoProduct
{
    public long id { get; set; }
    public int type { get; set; }
    public string name { get; set; }
    public string volume { get; set; }
    public string price { get; set; }
}