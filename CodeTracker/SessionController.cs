
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace code_tracker
{
    internal class SessionController
    {
        internal void DisplayTable(SqliteConnection connection)
        {
            var displayTableCommand = connection.CreateCommand();

            try
            {
                displayTableCommand.CommandText =
                @"
                    SELECT *
                    FROM sessions;
                ";

                using (var reader = displayTableCommand.ExecuteReader())
                {

                    Console.WriteLine("\nCurrent Coding Sessions:\n");

                    // Create a table
                    var table = new Table();
                    table.AddColumn("[red]ID[/]");
                    table.AddColumn("[red]Date[/]");
                    table.AddColumn("[red]Start[/]");
                    table.AddColumn("[red]End[/]");
                    table.AddColumn("[red]Duration[/]");

                    Console.WriteLine();

                    while (reader.Read())
                    {
                        table.AddRow($"{reader["id"]}", $"{reader["date"]}", $"{reader["startTime"]}", $"{reader["endTime"]}", $"{reader["duration"]}");
                    }

                    table.Expand();
                    AnsiConsole.Write(table);
                }
            }
            catch (SqliteException message)
            {
                Console.WriteLine(message.Message);
                Console.WriteLine(message.ErrorCode);
                throw;
            }
        }

        internal void CreateRecord(SqliteConnection connection)
        {
            var currentDate = DateTime.Now;

            string formattedDay = currentDate.ToString("dd-MM-yyyy");

            string startHour = currentDate.ToString("HH:mm");

            var endHour = currentDate.AddHours(2);

            string formattedEndHour = endHour.ToString("HH:mm");

            TimeSpan difference = endHour - currentDate;

            string formattedDurationHours = difference.TotalHours.ToString();

            string formattedDurationMinutes = difference.TotalMinutes.ToString();

            using (var insertTransaction = connection.BeginTransaction())
            {
                var insertCommand = connection.CreateCommand();

                insertCommand.CommandText =
                $"INSERT INTO sessions(date, startTime, endTime, duration) VALUES('{formattedDay}', '{startHour}', '{formattedEndHour}', '{difference}' )";

                insertCommand.ExecuteNonQuery();
                insertTransaction.Commit();

                Console.WriteLine();
                Console.WriteLine($"Session: {formattedDay}\t{startHour}\t {formattedEndHour}\t {difference}\t  inserted.");
            }
        }
    }

}