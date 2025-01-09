using System.Text.RegularExpressions;
using System.Data.SQLite;
using Newtonsoft.Json;

namespace com.erlendthune.polpriser
{
    class Program
    {

        static async Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: dotnet run [command]");
                Console.WriteLine("Available commands:");
                Console.WriteLine("- get-products-json-files");
                return;
            }

            string command = args[0];

            switch (command)
            {
                case "create-database":
                    DatabaseInitializer.ExecuteAsync();
                    break;
                case "create-version-file":
                    CreateVersionFileCommand.ExecuteAsync();
                    break;
                case "get-products-json-files":
                    await GetVinmonopoletProductsCommand.ExecuteAsync();
                    break;
                case "extract-product-data":
                     GetVinmonopoletProductsDataCommand.ExecuteAsync();
                    break;
                default:
                    Console.WriteLine($"Unknown command: {command}");
                    break;
            }
        }
    }
}
