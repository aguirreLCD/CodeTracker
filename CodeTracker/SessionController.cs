
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

            DisplayTable showResults = new();
            showResults.ShowTable(coding);

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

            var durationTotal = "";
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

                durationTotal = formattedDifference;

                codingSessionDuration.Add(
                    new Sessions
                    {
                        date = reader["date"].ToString(),
                        startTime = reader["MinStartTime"].ToString(),
                        endTime = reader["MaxStartTime"].ToString(),
                        duration = formattedDifference,
                    });
            }
            // Console.WriteLine(durationTotal);

            var sqlDuration = $"UPDATE sessions SET duration=@duration WHERE date=@date";

            // 3. we will pass parameters values by providing the customer entity
            var session = new Sessions() { date = formattedDay, duration = durationTotal };

            var rowsAffected = connection.Execute(sqlDuration, session);

            Console.WriteLine($"{rowsAffected} row(s) inserted.");

            var sqlCalculated = @"SELECT * FROM sessions WHERE date=@date";

            List<Sessions> calculatedSessions = connection.Query<Sessions>(sqlCalculated, new { date = formattedDay }).ToList();

            DisplayTable showResults = new();
            showResults.ShowTable(codingSessionDuration);

            Console.WriteLine("\ncalculatedSessions List:");

            DisplayTable showDuration = new();
            showDuration.ShowTable(calculatedSessions);

            return codingSessionDuration;
        }

        internal List<Sessions> DapperInsert(SqliteConnection connection)
        {
            var currentDate = DateTime.Now;

            string formattedDay = currentDate.ToString("dd-MM-yyyy");

            string startHour = currentDate.ToString("HH:mm");

            // 2. We will create an `INSERT` sql statement
            var sql = $"INSERT INTO sessions(date, startTime, endTime) VALUES(@date, @startTime, @endTime)";

            // 3. we will pass parameters values by providing the customer entity
            var session = new Sessions() { date = formattedDay, startTime = startHour, endTime = startHour };

            var rowsAffected = connection.Execute(sql, session);

            Console.WriteLine($"{rowsAffected} row(s) inserted.");

            List<Sessions> insertedSessions = connection.Query<Sessions>("SELECT * FROM sessions").ToList();

            Console.WriteLine("\ndapper: insert table\n");
            DisplayTable showResults = new();
            showResults.ShowTable(insertedSessions);

            return insertedSessions;
        }

        internal List<Sessions> ShowSessionsByDate(SqliteConnection connection, string userInputDate)
        {
            // ADD validation for userInputDate

            var sql = @"SELECT * FROM sessions WHERE date = @date;";

            // Use the Query method to execute the query and return a list of objects    
            List<Sessions> coding = connection.Query<Sessions>(sql, new { date = userInputDate }).ToList();

            DisplayTable showResults = new();
            showResults.ShowTable(coding);

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
            showResults.ShowTable(codingSessionDuration);

            return codingSessionDuration;
        }

        internal void DeleteRecord(SqliteConnection connection)
        {
            var id = AnsiConsole.Prompt(new TextPrompt<int>("What session you want to delete? type id:")
            .PromptStyle("red"));

            var sql = @"DELETE FROM sessions WHERE id = $id;";

            var affectedRows = connection.Execute(sql, new { id });

            Console.WriteLine($"Affected Rows: {affectedRows}");

        }

        internal void UpdateRecord(SqliteConnection connection)
        {
            var date = AnsiConsole.Prompt(new TextPrompt<string>("What session you want to update? type formatted date: 01-10-2024")
            .PromptStyle("red"));

            var sql = @"UPDATE sessions SET endTime = startTime WHERE date=@date;";

            var affectedRows = connection.Execute(sql, new { date });

            Console.WriteLine($"Affected Rows: {affectedRows}");

        }

    }
}