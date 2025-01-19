using System;
using System.Data.SQLite;
using System.IO;

public class CreateVersionFileCommand
{
    private static StreamWriter? file;

    public static void OpenFile(string filename)
    {
        file = new StreamWriter(filename);
    }

    public static void CloseFile()
    {
        file?.Close();
    }

    public static void MyPuts(string? s)
    {
        if(s is null)
            return;
            
        file?.Write(s);
    }

    public static void ExecuteAsync()
    {
        OpenFile("vino.txt");

        using (var connection = new SQLiteConnection($"Data Source={Config.databaseFilePath};Version=3;"))
        {
            connection.Open();

            string selectQuery = "SELECT * FROM vinodate";
            using (var command = new SQLiteCommand(selectQuery, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    MyPuts(reader[0].ToString());
                    MyPuts("\n");
                }
            }
        }

        CloseFile();
    }
}