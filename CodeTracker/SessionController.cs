
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

                    Console.WriteLine("\nCurrent Coding Sessions:");
                    // Create a table
                    var table = new Table();
                    table.AddColumn("[red]ID[/]");
                    table.AddColumn("[red]Date[/]");
                    table.AddColumn("[red]Start[/]");
                    table.AddColumn("[red]End[/]");
                    table.AddColumn("[red]Duration[/]");

                    while (reader.Read())
                    {
                        table.AddRow($"{reader["id"]}", $"{reader["date"]}", $"{reader["startTime"]}", $"{reader["endTime"]}", $"{reader["duration"]}");
                    }

                    // table.Expand();
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
                insertCommand.CommandText =
                $"INSERT INTO sessions(date, startTime, endTime) VALUES('{formattedDay}', '{startHour}', '{formattedEndHour}' )";

                insertCommand.ExecuteNonQuery();
                insertTransaction.Commit();

                Console.WriteLine();
                Console.WriteLine($"Session: {formattedDay}\t{startHour}\t {formattedEndHour}\t inserted.");
                Console.WriteLine();

            }
        }


        internal void DeleteRecord(SqliteConnection connection)
        {
            try
            {
                var id = AnsiConsole.Prompt(new TextPrompt<int>("What session you want to delete? type id:")
                .PromptStyle("red"));

                using (var deleteTransaction = connection.BeginTransaction())
                {
                    var deleteCommand = connection.CreateCommand();

                    deleteCommand.CommandText =
                    @"
                        DELETE FROM sessions
                        WHERE id = $id;
                    ";

                    deleteCommand.Parameters.AddWithValue("$id", id);
                    deleteCommand.ExecuteNonQuery();
                    deleteTransaction.Commit();

                    Console.WriteLine();
                    AnsiConsole.MarkupLine("Deleted: [yellow]{0}[/]", id);

                }
            }
            catch (SqliteException message)
            {
                Console.WriteLine(message.Message);
                Console.WriteLine(message.ErrorCode);
                throw;
            }
        }

        internal void EndSessionTime(SqliteConnection connection)
        {

            var dayToCalculate = DateTime.Now;

            string today = dayToCalculate.ToString("dd-MM-yyyy");

            // Console.WriteLine(today);

            var calculateCommand = connection.CreateCommand();

            try
            {
                calculateCommand.CommandText =
                @"
                   SELECT date, startTime, endTime FROM sessions WHERE date='30-09-2024' ORDER BY id DESC LIMIT 1;
                ";

                using (var reader = calculateCommand.ExecuteReader())
                {
                    var calcTable = new Table();
                    calcTable.AddColumn("[red]Date[/]");
                    calcTable.AddColumn("[red]Start[/]");
                    calcTable.AddColumn("[red]End[/]");
                    // table.AddColumn("[red]Duration[/]");

                    while (reader.Read())
                    {
                        // var endTime = reader["startTime"];
                        // Console.WriteLine(endTime);

                        // table.AddRow($"{reader["id"]}", $"{reader["date"]}", $"{reader["startTime"]}", $"{reader["endTime"]}", $"{reader["duration"]}");
                        calcTable.AddRow($"{reader["date"]}", $"{reader["startTime"]}", $"{reader["endTime"]}");
                    }

                    // table.Expand();
                    AnsiConsole.Write(calcTable);

                }
            }
            catch (SqliteException message)
            {
                Console.WriteLine(message.Message);
                Console.WriteLine(message.ErrorCode);
                throw;
            }
        }



        internal void CalculateSessionTime(SqliteConnection connection)
        {

            var dayToCalculate = DateTime.Now;

            string today = dayToCalculate.ToString("dd-MM-yyyy");

            // Console.WriteLine(today);

            var calculateCommand = connection.CreateCommand();

            try
            {
                calculateCommand.CommandText =
                @"
                   SELECT 
                    date,
                    MAX(startTime) As MaxStartTime,
                    MIN(startTime) As MinStartTime,
                    duration
                    FROM sessions
                    WHERE date='30-09-2024'
                   ;
                ";
                using (var reader = calculateCommand.ExecuteReader())
                {

                    // Console.WriteLine("\nCurrent Coding Sessions:");
                    // Create a table
                    var table = new Table();
                    // table.AddColumn("[red]ID[/]");
                    table.AddColumn("[red]Date[/]");
                    table.AddColumn("[red]Start[/]");
                    table.AddColumn("[red]End[/]");
                    table.AddColumn("[red]Duration[/]");


                    while (reader.Read())
                    {
                        var endTime = reader["MaxStartTime"];
                        Console.WriteLine(endTime);

                        string formattedEndHour = endTime.ToString();
                        Console.WriteLine(formattedEndHour);

                        DateTime theEndOfSession = DateTime.Parse(formattedEndHour);
                        Console.WriteLine(theEndOfSession);


                        var startTime = reader["MinStartTime"];
                        Console.WriteLine(startTime);

                        string formattedStartHour = startTime.ToString();
                        Console.WriteLine(formattedStartHour);

                        DateTime theStartOfSession = DateTime.Parse(formattedStartHour);
                        Console.WriteLine(theStartOfSession);

                        TimeSpan difference = theEndOfSession - theStartOfSession;
                        Console.WriteLine(difference);
                        Console.WriteLine(difference.TotalHours);
                        Console.WriteLine(difference.TotalMinutes);

                        // table.AddRow($"{reader["id"]}", $"{reader["date"]}", $"{reader["startTime"]}", $"{reader["endTime"]}", $"{reader["duration"]}");
                        table.AddRow($"{reader["date"]}", $"{reader["MinStartTime"]}", $"{reader["MaxStartTime"]}", $"{difference}");
                    }

                    // table.Expand();
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






    }
}