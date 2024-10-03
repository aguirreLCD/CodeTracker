
using Microsoft.Data.Sqlite;
using Spectre.Console;

using Dapper;
using System.Data;

namespace code_tracker
{
    internal class SessionController
    {
        internal List<Sessions> GetDataFromDB(SqliteConnection connection)
        {
            var sql = @"SELECT * FROM sessions;";

            List<Sessions> dapperSession = connection.Query<Sessions>(sql).ToList();

            Console.WriteLine("\ndapper session\n");
            DisplayTable showResults = new();
            showResults.ShowTable(dapperSession);

            return dapperSession;
        }

        internal List<Sessions> PrintTodayTable(SqliteConnection connection)
        {
            var sql = @"SELECT * FROM sessions WHERE date = @date;";

            // Use the Query method to execute the query and return a list of objects    
            List<Sessions> coding = connection.Query<Sessions>(sql, new { date = "03-10-2024" }).ToList();

            Console.WriteLine("\ndapper: print table\n");
            DisplayTable showResults = new();
            showResults.ShowTodayTable(coding);

            return coding;
        }

        internal List<Sessions> CalculateTodaySessionDuration(SqliteConnection connection)
        {
            Console.WriteLine("\nToday's Coding Session Duration:");

            var currentDate = DateTime.Now;

            string formattedDay = currentDate.ToString("dd-MM-yyyy");

            var sql = @"SELECT date, MAX(startTime) As MaxStartTime, MIN(startTime) As MinStartTime, duration FROM sessions WHERE date=@date;";

            List<Sessions> codingSessionDuration = new List<Sessions>();

            var reader = connection.ExecuteReader(sql, new { date = formattedDay });

            // DataTable codingSessionDurationS = new DataTable();
            // codingSessionDurationS.Load(reader);

            while (reader.Read())
            {
                var endTime = reader["MaxStartTime"];
                // Console.WriteLine(endTime);

                string? formattedEndHour = endTime.ToString();
                // Console.WriteLine(formattedEndHour);

                DateTime theEndOfSession = DateTime.Parse(formattedEndHour);
                // Console.WriteLine(theEndOfSession);

                var startTime = reader["MinStartTime"];
                // Console.WriteLine(startTime);

                string? formattedStartHour = startTime.ToString();
                // Console.WriteLine(formattedStartHour);

                DateTime theStartOfSession = DateTime.Parse(formattedStartHour);
                // Console.WriteLine(theStartOfSession);

                TimeSpan difference = theEndOfSession - theStartOfSession;
                // Console.WriteLine(difference);

                string? formattedDifference = difference.ToString();
                // Console.WriteLine(formattedDifference);

                // var duration = formattedDifference;
                codingSessionDuration.Add(
                    new Sessions
                    {
                        date = reader["date"].ToString(),
                        startTime = reader["MinStartTime"].ToString(),
                        endTime = reader["MaxStartTime"].ToString(),
                        duration = formattedDifference,
                    });
            }

            // // Use the Query method to execute the query and return a list of objects    
            // // codingSessionDuration = connection.Query<Sessions>(sql, new { date = formattedDay }).ToList();

            // Console.WriteLine("\ndapper: session time table\n");
            DisplayTable showResults = new();
            showResults.ShowSessionDurationTable(codingSessionDuration);

            return codingSessionDuration;
        }

        internal List<Sessions> DapperInsert(SqliteConnection connection)
        {
            var currentDate = DateTime.Now;

            string formattedDay = currentDate.ToString("dd-MM-yyyy");

            string startHour = currentDate.ToString("HH:mm");

            // 2. We will create an `INSERT` sql statement
            var sql = $"INSERT INTO sessions(date, startTime) VALUES(@date, @startTime)";

            {
                // 3. we will pass parameters values by providing the customer entity
                var session = new Sessions() { date = formattedDay, startTime = startHour };
                var rowsAffected = connection.Execute(sql, session);
                Console.WriteLine($"{rowsAffected} row(s) inserted.");
            }
            List<Sessions> insertedSessions = connection.Query<Sessions>("SELECT * FROM sessions").ToList();

            Console.WriteLine("\ndapper: insert table\n");
            DisplayTable showResults = new();
            showResults.ShowTodayTable(insertedSessions);

            return insertedSessions;
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


        internal List<Sessions> ShowSessionsByDate(SqliteConnection connection, string userInputDate)
        {
            // ADD validation for userInputDate

            var sql = @"SELECT * FROM sessions WHERE date = @date;";

            // Use the Query method to execute the query and return a list of objects    
            List<Sessions> coding = connection.Query<Sessions>(sql, new { date = userInputDate }).ToList();

            DisplayTable showResults = new();
            showResults.ShowTodayTable(coding);

            return coding;
        }



        internal List<Sessions> CalculateSessionDuration(SqliteConnection connection, string userInputDate)
        {
            Console.WriteLine($"\n{userInputDate} Coding Session Duration:");

            var sql = @"SELECT date, MAX(startTime) As MaxStartTime, MIN(startTime) As MinStartTime, duration FROM sessions WHERE date=@date;";

            List<Sessions> codingSessionDuration = new List<Sessions>();

            var reader = connection.ExecuteReader(sql, new { date = userInputDate });


            while (reader.Read())
            {
                var endTime = reader["MaxStartTime"];
                // Console.WriteLine(endTime);

                string? formattedEndHour = endTime.ToString();
                // Console.WriteLine(formattedEndHour);

                DateTime theEndOfSession = DateTime.Parse(formattedEndHour);
                // Console.WriteLine(theEndOfSession);

                var startTime = reader["MinStartTime"];
                // Console.WriteLine(startTime);

                string? formattedStartHour = startTime.ToString();
                // Console.WriteLine(formattedStartHour);

                DateTime theStartOfSession = DateTime.Parse(formattedStartHour);
                // Console.WriteLine(theStartOfSession);

                TimeSpan difference = theEndOfSession - theStartOfSession;
                // Console.WriteLine(difference);

                string? formattedDifference = difference.ToString();
                // Console.WriteLine(formattedDifference);

                // var duration = formattedDifference;
                codingSessionDuration.Add(
                    new Sessions
                    {
                        date = reader["date"].ToString(),
                        startTime = reader["MinStartTime"].ToString(),
                        endTime = reader["MaxStartTime"].ToString(),
                        duration = formattedDifference,
                    });
            }

            // // Use the Query method to execute the query and return a list of objects    
            // // codingSessionDuration = connection.Query<Sessions>(sql, new { date = formattedDay }).ToList();

            // Console.WriteLine("\ndapper: session time table\n");
            DisplayTable showResults = new();
            showResults.ShowSessionDurationTable(codingSessionDuration);

            return codingSessionDuration;
        }


    }
}