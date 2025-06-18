
public static class Config 
{
    public static Dictionary<string, int> Categories = new Dictionary<string, int>
    {
        { "r%C3%B8dvin", 0 },
        { "hvitvin", 1 },
        { "ros%C3%A9vin", 2 },
        { "sterkvin", 3 },
        { "musserende_vin", 4 },
        { "fruktvin", 5 },
        { "brennevin", 6 },
        { "%C3%B8l", 7 },
        { "perlende_vin", 8 },
        { "aromatisert_vin", 9 },
        { "sider", 10 },
        { "alkoholfritt", 11 },
        { "sake", 12 },
        { "mj%C3%B8d", 13 }
    };
    // public static string baseUrl = "https://www.vinmonopolet.no/vmpws/v2/vmp/search?fields=FULL&pageSize=24&searchType=product&currentPage={0}&q=%3Arelevance%3AmainCategory%3A{1}";

    //18.6.2025
    public static string baseUrl = "https://www.vinmonopolet.no/vmpws/v2/vmp/products/search?fields=FULL&pageSize=24&currentPage={0}&q=%3Arelevance%3AmainCategory%3A{1}";

    public static string jsonProductsDirectory = "priser";
    public static string databaseFilePath = "vino.db";

    public static string GetSafeFileName(string fileName)
    {
        return string.Concat(fileName.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c));
    }
    public static void RenameFilesInCurrentDirectory()
    {
        string directoryPath = Config.jsonProductsDirectory;

        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine($"Directory {directoryPath} does not exist.");
            return;
        }

        var files = Directory.GetFiles(directoryPath);
        foreach (var filePath in files)
        {
            var fileName = Path.GetFileName(filePath);
            var safeFileName = GetSafeFileName(fileName);

            if (fileName != safeFileName)
            {
                var safeFilePath = Path.Combine(directoryPath, safeFileName);
                File.Move(filePath, safeFilePath);
                Console.WriteLine($"Renamed {fileName} to {safeFileName}");
            }
        }
    }

    public static void CreateDirectories() 
    {
        CreateDirectory(jsonProductsDirectory);
    }

    public static void CreateDirectory(string directoryPath) 
    {
       if (!Directory.Exists(directoryPath))
        {
            try
            {
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine($"Directory {directoryPath} created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating directory: {ex.Message}");
            }
        }
    }
}



