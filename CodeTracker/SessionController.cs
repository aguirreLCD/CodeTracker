
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace code_tracker
{
    internal class SessionController
    {
        // To save a database table into a List<> using Microsoft.Data.Sqlite,
        // how to connect to an SQLite database, execute a query, and store the results in a List<Sessions>
        internal List<Sessions> GetResultsFromDatabase(SqliteConnection connection)
        {
            List<Sessions> codeSessions = new List<Sessions>();

            using (var tableCmd = connection.CreateCommand())
            {
                // execute a query
                tableCmd.CommandText =
                @"
                    SELECT *
                    FROM sessions;
                ";

                using (SqliteDataReader reader = tableCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine("\nCurrent Coding Sessions FROM GETDATA:");
                        // Create a table
                        var table = new Table();
                        table.AddColumn("[red]ID[/]");
                        table.AddColumn("[red]Date[/]");
                        table.AddColumn("[red]Start[/]");
                        table.AddColumn("[red]End[/]");
                        table.AddColumn("[red]Duration[/]");

                        while (reader.Read())
                        {
                            // and store the results in a List<Sessions> codeSessions
                            codeSessions.Add(
                                new Sessions
                                {
                                    id = reader.GetInt32(0),
                                    date = reader["date"].ToString(),
                                    startTime = reader["startTime"].ToString(),
                                    endTime = reader["endTime"].ToString(),
                                    duration = reader["duration"].ToString()
                                });

                            table.AddRow($"{reader["id"]}", $"{reader["date"]}", $"{reader["startTime"]}", $"{reader["endTime"]}", $"{reader["duration"]}");
                        }
                        AnsiConsole.Write(table);

                        foreach (var session in codeSessions)
                        {
                            Console.WriteLine(session.id);
                            Console.WriteLine(session.date);
                            Console.WriteLine(session.startTime);
                            Console.WriteLine(session.endTime);
                            Console.WriteLine(session.duration);
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n\nNo rows found.\n\n");
                    }
                }
            }

            DisplayTable showResults = new();
            showResults.ShowTable(codeSessions);

            return codeSessions;
        }


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
                $"INSERT INTO sessions(date, startTime) VALUES('{formattedDay}', '{startHour}')";

                insertCommand.ExecuteNonQuery();
                insertTransaction.Commit();

                Console.WriteLine();
                Console.WriteLine($"Session: {formattedDay}\t{startHour}\t inserted.");
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
        internal void CalculateSessionTime(SqliteConnection connection)
        {

            List<Sessions> calculateSessionsTable = new List<Sessions>();

            var dayToCalculate = DateTime.Now;

            string today = dayToCalculate.ToString("dd-MM-yyyy");

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
                    WHERE date='02-10-2024'
                   ;
                ";
                using (var reader = calculateCommand.ExecuteReader())
                {
                    var table = new Table();
                    table.AddColumn("[red]Date[/]");
                    table.AddColumn("[red]Start[/]");
                    table.AddColumn("[red]End[/]");
                    table.AddColumn("[red]Duration[/]");

                    while (reader.Read())
                    {
                        var endTime = reader["MaxStartTime"];

                        string? formattedEndHour = endTime.ToString();

                        DateTime theEndOfSession = DateTime.Parse(formattedEndHour);

                        var startTime = reader["MinStartTime"];

                        string? formattedStartHour = startTime.ToString();

                        DateTime theStartOfSession = DateTime.Parse(formattedStartHour);

                        TimeSpan difference = theEndOfSession - theStartOfSession;
                        Console.WriteLine(difference);
                        Console.WriteLine(difference.TotalHours);
                        Console.WriteLine(difference.TotalMinutes);

                        string? formattedDifference = difference.ToString();

                        table.AddRow($"{reader["date"]}", $"{reader["MinStartTime"]}", $"{reader["MaxStartTime"]}", $"{difference}");

                        var duration = formattedDifference;
                        Console.WriteLine(formattedDifference);

                        // and store the results in a List<Sessions> codeSessions
                        calculateSessionsTable.Add(
                            new Sessions
                            {
                                date = reader["date"].ToString(),
                                startTime = reader["MinStartTime"].ToString(),
                                endTime = reader["MaxStartTime"].ToString(),
                                duration = formattedDifference,
                            });
                    }
                    AnsiConsole.Write(table);

                    foreach (var session in calculateSessionsTable)
                    {
                        Console.WriteLine(session.date);
                        Console.WriteLine(session.startTime);
                        Console.WriteLine(session.endTime);
                        Console.WriteLine(session.duration);
                    }
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