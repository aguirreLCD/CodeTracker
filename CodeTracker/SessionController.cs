
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Collections.Generic;

namespace code_tracker
{
    internal class SessionController
    {
        internal List<Session> GetData()
        {
            List<Session> codeSessions = new List<Session>();

            // var codeSessions = new List<Session>();

            string connectionString = "Data Source=codesessions.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string query =
                @"
                    SELECT *
                    FROM sessions;
                ";

                using (var command = new SqliteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var item = new Session
                            {
                                id = reader.GetInt32(0),
                                date = reader.GetString(0),
                                startTime = reader.GetString(0),
                                endTime = reader.GetString(0),
                                duration = reader.GetString(0)
                            };

                            codeSessions.Add(item);

                            Console.Write($"{reader["id"]}\t");
                            Console.Write($"{reader["date"]}\t");
                            Console.Write($"{reader["startTime"]}\t");
                            Console.Write($"{reader["endTime"]}\t");
                            Console.Write($"{reader["duration"]}\t");
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n\nNo rows found.\n\n");
                    }
                }
            }
            Shooow(codeSessions);
            return codeSessions;
        }

        internal void Shooow<Session>(List<Session> codeSessions)
        {
            foreach (var code in codeSessions)
            {
                Console.WriteLine();
                Console.WriteLine(code);
                // Console.WriteLine(code?.ToString());
                Console.WriteLine();
            }
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
                    WHERE date='30-09-2024'
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

                        string formattedEndHour = endTime.ToString();

                        DateTime theEndOfSession = DateTime.Parse(formattedEndHour);

                        var startTime = reader["MinStartTime"];

                        string formattedStartHour = startTime.ToString();

                        DateTime theStartOfSession = DateTime.Parse(formattedStartHour);

                        TimeSpan difference = theEndOfSession - theStartOfSession;
                        Console.WriteLine(difference);
                        Console.WriteLine(difference.TotalHours);
                        Console.WriteLine(difference.TotalMinutes);

                        table.AddRow($"{reader["date"]}", $"{reader["MinStartTime"]}", $"{reader["MaxStartTime"]}", $"{difference}");
                    }
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