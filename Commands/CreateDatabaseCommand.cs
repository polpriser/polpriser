using System;
using System.Data.SQLite;

public class DatabaseInitializer
{
    public static void ExecuteAsync()
    {
        using (var connection = new SQLiteConnection($"Data Source={Config.databaseFilePath};Version=3;"))
        {
            connection.Open();

            string createVinoTableQuery = @"
                CREATE TABLE IF NOT EXISTS vino (
                    id INT UNIQUE, 
                    type TINYINT, 
                    name VARCHAR(60), 
                    volume VARCHAR(10), 
                    alcohol DECIMAL(4, 2),
                    price INT
                )";

            using (var command = new SQLiteCommand(createVinoTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string createUsageTableQuery = @"
                CREATE TABLE IF NOT EXISTS usage (
                    noOfTimesUsed INT
                )";
            using (var command = new SQLiteCommand(createUsageTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string insertUsageQuery = "INSERT INTO usage VALUES (0)";
            using (var command = new SQLiteCommand(insertUsageQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string createVinoDateTableQuery = @"
                CREATE TABLE IF NOT EXISTS vinodate (
                    datecreated INT
                )";
            using (var command = new SQLiteCommand(createVinoDateTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string insertVinoDateQuery = "INSERT INTO vinodate VALUES (strftime('%s','now'))";
            using (var command = new SQLiteCommand(insertVinoDateQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string createPriceIndexQuery = "CREATE INDEX IF NOT EXISTS priceindex ON vino (price ASC)";
            using (var command = new SQLiteCommand(createPriceIndexQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string createNameIndexQuery = "CREATE INDEX IF NOT EXISTS nameindex ON vino (name ASC)";
            using (var command = new SQLiteCommand(createNameIndexQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string createTypeIndexQuery = "CREATE INDEX IF NOT EXISTS typeindex ON vino (type ASC)";
            using (var command = new SQLiteCommand(createTypeIndexQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}