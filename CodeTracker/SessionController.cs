
using Microsoft.Data.Sqlite;
using Spectre.Console;
using Dapper;
using System.Data;

namespace code_tracker
{
    internal class SessionController
    {

        DisplayTable showResults = new();
        List<Sessions> dataFromDB = new List<Sessions>();
        List<Sessions> firstLastDataByDay = new List<Sessions>();
        string? durationTotal = "";


        internal List<Sessions> GetFirsSession(SqliteConnection connection)
        {
            var sqlFirstSession = @"
                                    SELECT 
                                    id,
                                    date,
                                    startTime,
                                    endTime
                                    FROM sessions
                                    WHERE date='08-10-2024'
                                    ORDER BY id
                                    LIMIT 1;
                                ";

            dataFromDB = connection.Query<Sessions>(sqlFirstSession).ToList();
            showResults.ShowTable(dataFromDB);
            return dataFromDB;
        }


        internal List<Sessions> Casting(SqliteConnection connection)
        {
            var sqlCast = @"
                                    SELECT CAST(startTime AS integer) AS startTime,
                                    CAST(endTime AS integer) AS endTime,
                                    CAST(duration AS date) AS duration 
                                    FROM sessions
                                    WHERE date = '07-10-2024';
                                ";
            var sqlCastDate = @"
                              SELECT CAST(startTime AS time) AS startTime,
                              CAST(endTime AS time) AS endTime,
                              CAST(endTime - startTime AS time) AS duration 
                              FROM sessions WHERE date = '07-10-2024';
                            ";

            // SELECT CAST(startTime AS dateTime) AS startTime, CAST(endTime AS dateTime) AS endTime, CAST(endTime - startTime AS dateTime) AS duration FROM sessions WHERE date = '07-10-2024';
            // SELECT CAST(startTime AS date) AS startTime, CAST(endTime AS date) AS endTime, CAST(endTime - startTime AS date) AS duration FROM sessions WHERE date = '07-10-2024';
            // SELECT CAST(startTime AS time) AS startTime, CAST(endTime AS time) AS endTime, CAST(endTime - startTime AS time) AS duration FROM sessions WHERE date = '07-10-2024';
            // SELECT CAST(startTime AS time(7)) AS startTime FROM sessions WHERE date = '07-10-2024';
            // SELECT CAST(date AS dateTime2(7)) AS date, CAST(startTime AS dateTime2) AS startTime, CAST(endTime AS dateTime2) AS endTime, CAST(endTime - startTime AS dateTime2) AS duration FROM sessions WHERE date = '07-10-2024';

            dataFromDB = connection.Query<Sessions>(sqlCast).ToList();
            showResults.ShowTable(dataFromDB);
            return dataFromDB;
        }

        internal List<Sessions> GetLastSession(SqliteConnection connection)
        {
            var sqlLastSession = @"
                                    SELECT 
                                    id,
                                    date,
                                    startTime,
                                    endTime
                                    FROM sessions
                                    WHERE date='08-10-2024'
                                    ORDER BY id
                                    DESC
                                    LIMIT 1;
                                ";

            dataFromDB = connection.Query<Sessions>(sqlLastSession).ToList();
            showResults.ShowTable(dataFromDB);
            return dataFromDB;
        }

        internal List<Sessions> GetFirstLastRecord(SqliteConnection connection)
        {
            var sqlFirstLast = @"
                                  SELECT
                                  date,
                                  MIN(startTime) As startTime,
                                  MAX(startTime) As endTime,
                                  MAX(startTime) - MIN(startTime) As duration 
                                  FROM sessions
                                  WHERE date='08-10-2024';
                                ";

            dataFromDB = connection.Query<Sessions>(sqlFirstLast).ToList();
            showResults.ShowTable(dataFromDB);
            return dataFromDB;
        }

        internal List<Sessions> GetDataFromDB(SqliteConnection connection)
        {
            var sqlSelectAll = @"SELECT *FROM sessions;";
            dataFromDB = connection.Query<Sessions>(sqlSelectAll).ToList();
            showResults.ShowTable(dataFromDB);
            return dataFromDB;
        }




        internal List<Sessions> GetLastDataFromDB(SqliteConnection connection)
        {
            var sqlLast = @"
                SELECT id,
                date,
                startTime AS endTime,
                duration
                FROM (
                    SELECT *, ROW_NUMBER() OVER (PARTITION BY date ORDER BY id DESC) as RowNum
                    FROM sessions
                ) as sub
                WHERE sub.RowNum = 1 ORDER BY id ";

            dataFromDB = connection.Query<Sessions>(sqlLast).ToList();
            showResults.ShowTable(dataFromDB);
            return dataFromDB;
        }

        internal List<Sessions> PrintTodayTable(SqliteConnection connection)
        {
            var currentDay = DateTime.Now;

            string? formattedDay = currentDay.ToString("dd-MM-yyyy");

            var sql = @"SELECT id, date, startTime FROM sessions WHERE date = @date;";

            dataFromDB = connection.Query<Sessions>(sql, new { date = formattedDay }).ToList();

            showResults.ShowTable(dataFromDB);

            return dataFromDB;
        }

        internal List<Sessions> CalculateTodaySessionDuration(SqliteConnection connection)
        {
            Console.WriteLine("\nToday's Coding Session Duration:");

            var currentDate = DateTime.Now;

            string formattedDay = currentDate.ToString("dd-MM-yyyy");

            var sql = @"SELECT date, MAX(startTime) As MaxStartTime, MIN(startTime) As MinStartTime, duration FROM sessions WHERE date=@date;";

            List<Sessions> codingSessionDuration = new List<Sessions>();

            var reader = connection.ExecuteReader(sql, new { date = formattedDay });

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
            Console.WriteLine(durationTotal);

            // var sqlDuration = $"UPDATE sessions SET duration=@duration WHERE date=@date";
            // // 3. we will pass parameters values by providing the customer entity
            // var session = new Sessions() { date = formattedDay, duration = durationTotal };
            // var rowsAffected = connection.Execute(sqlDuration, session);
            // Console.WriteLine($"{rowsAffected} row(s) inserted.");
            showResults.ShowTable(codingSessionDuration);

            var sqlCalculated = @"SELECT * FROM sessions WHERE date=@date";

            dataFromDB = connection.Query<Sessions>(sqlCalculated, new { date = formattedDay }).ToList();

            showResults.ShowTable(dataFromDB);

            return codingSessionDuration;
        }

        internal List<Sessions> DapperInsert(SqliteConnection connection)
        {
            var currentDate = DateTime.Now;
            string formattedDay = currentDate.ToString("dd-MM-yyyy");
            string startHour = currentDate.ToString("HH:mm");

            // 2. We will create an `INSERT` sql statement
            var sql = $"INSERT INTO sessions(date, startTime) VALUES(@date, @startTime)";

            // 3. we will pass parameters values by providing the customer entity
            var session = new Sessions() { date = formattedDay, startTime = startHour };
            var rowsAffected = connection.Execute(sql, session);
            Console.WriteLine($"\nNew session started: {formattedDay} at {startHour} inserted.");

            Console.WriteLine($"\n{rowsAffected} row(s) inserted.");

            var sqlInsert = "SELECT id, date, startTime FROM sessions;";

            dataFromDB = connection.Query<Sessions>(sqlInsert).ToList();

            Console.WriteLine("Sessions:");

            showResults.ShowTable(dataFromDB);

            return dataFromDB;
        }

        internal List<Sessions> ShowSessionsByDate(SqliteConnection connection, string userInputDate)
        {
            var sql = @"SELECT * FROM sessions WHERE date = @date;";

            dataFromDB = connection.Query<Sessions>(sql, new { date = userInputDate }).ToList();

            CalculateDuration(connection, userInputDate);

            return dataFromDB;
        }

        internal List<Sessions> CalculateSessionDuration(SqliteConnection connection, string userInputDate)
        {
            Console.WriteLine($"\n{userInputDate} Coding Session Duration:");

            var sql = @"SELECT date, MAX(startTime) As MaxStartTime, MIN(startTime) As MinStartTime, duration FROM sessions WHERE date=@date;";

            List<Sessions> codingSessionDuration = new List<Sessions>();

            var reader = connection.ExecuteReader(sql, new { date = userInputDate });

            var durationSession = "";
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

                durationSession = formattedDifference;

                codingSessionDuration.Add(
                    new Sessions
                    {
                        date = reader["date"].ToString(),
                        startTime = reader["MinStartTime"].ToString(),
                        endTime = reader["MaxStartTime"].ToString(),
                        duration = formattedDifference,
                    });
            }
            Console.WriteLine(durationSession);

            var sqlDuration = $"UPDATE sessions SET duration=@duration WHERE date=@date";
            var session = new Sessions() { date = userInputDate, duration = durationSession };
            var rowsAffected = connection.Execute(sqlDuration, session);
            Console.WriteLine($"{rowsAffected} row(s) inserted.");

            var sqlCalculated = @"SELECT * FROM sessions WHERE date=@date";
            dataFromDB = connection.Query<Sessions>(sqlCalculated, new { date = userInputDate }).ToList();

            showResults.ShowTable(codingSessionDuration);

            showResults.ShowTable(dataFromDB);

            return codingSessionDuration;
        }

        internal void DeleteRecord(SqliteConnection connection)
        {
            var id = AnsiConsole.Prompt(new TextPrompt<int>("What session you want to delete? type id:")
            .PromptStyle("red"));

            var sql = @"DELETE FROM sessions WHERE id = $id;";

            var affectedRows = connection.Execute(sql, new { id });

            // var sqlDeleteAll = @"DELETE FROM sessions;";

            // var affectedRows = connection.Execute(sqlDeleteAll);

            Console.WriteLine($"Affected Rows: {affectedRows}");

        }

        internal List<Sessions> UpdateRecord(SqliteConnection connection)
        {
            var date = AnsiConsole.Prompt(new TextPrompt<string>("What session you want to update? type formatted date: 01-10-2024")
            .PromptStyle("red"));

            var sqlUpdate = @"UPDATE sessions SET duration=@duration WHERE date=@date;";

            var session = new Sessions() { date = date, duration = durationTotal };

            var rowsAffected = connection.Execute(sqlUpdate, session);

            Console.WriteLine($"{rowsAffected} row(s) affected.");

            var sqlSelect = @"SELECT * FROM sessions;";
            dataFromDB = connection.Query<Sessions>(sqlSelect).ToList();

            showResults.ShowTable(dataFromDB);

            return dataFromDB;
        }

        // TODO: Calculate and show duration -> foreach day
        internal List<Sessions> CalculateDuration(SqliteConnection connection, string userInputDate)
        {
            // var currentDate = DateTime.Now;
            // string formattedDay = currentDate.ToString("dd-MM-yyyy");

            var sql = @"SELECT date, MAX(startTime) As MaxStartTime, MIN(startTime) As MinStartTime, duration FROM sessions WHERE date=@date;";

            var sessions = connection.Query(sql, new { date = userInputDate });

            foreach (var sessiondata in sessions)
            {
                dataFromDB.Add(
                                    new Sessions
                                    {
                                        date = sessiondata.date.ToString(),
                                        startTime = sessiondata.MinStartTime.ToString(),
                                        endTime = sessiondata.MaxStartTime.ToString(),
                                        duration = sessiondata.duration.ToString(),
                                    });
            }
            showResults.ShowTable(dataFromDB);
            return dataFromDB;
        }

        internal List<Sessions> GetDuration(SqliteConnection connection)
        {
            List<Sessions> firstRowByDate = new List<Sessions>();

            var sqlFirst = @"
                SELECT id, date, startTime FROM (
                    SELECT *, ROW_NUMBER() OVER (PARTITION BY date ORDER BY id) as RowNum
                    FROM sessions
                ) as sub
                WHERE sub.RowNum = 1 ORDER BY id";

            firstRowByDate = connection.Query<Sessions>(sqlFirst).ToList();
            Console.WriteLine("\nFirstRowByDate:");
            showResults.ShowTable(firstRowByDate);

            List<Sessions> lastRowByDate = new List<Sessions>();

            var sqlLast = @"
                SELECT id, date, endTime, duration FROM (
                    SELECT *, ROW_NUMBER() OVER (PARTITION BY date ORDER BY id DESC) as RowNum
                    FROM sessions
                ) as sub
                WHERE sub.RowNum = 1 ORDER BY id";

            lastRowByDate = connection.Query<Sessions>(sqlLast).ToList();
            Console.WriteLine("\nLastRowByDate:");
            showResults.ShowTable(lastRowByDate);

            var sqlSelect = @"SELECT * FROM sessions;";
            dataFromDB = connection.Query<Sessions>(sqlSelect).ToList();
            showResults.ShowTable(dataFromDB);



            // var sqlExpression = @"SELECT id, date, endTime - startTime AS duration FROM sessions;";
            // data = connection.Query<Sessions>(sqlExpression).ToList();
            // showResults.ShowTable(data);
            // CalculateDuration(connection, date);
            //    return dataFromDB;
            return firstLastDataByDay;
        }

        internal List<Sessions> InsertRecord(SqliteConnection connection)
        {
            // 2. We will create an `INSERT` sql statement
            var sqlInsertRecord = $"INSERT INTO sessions(date, startTime, endTime) VALUES(@date, @startTime, @endTime)";

            // 3. we will pass parameters values by providing the customer entity
            var session = new Sessions() { date = "07-10-2024", startTime = "08:44", endTime = "17:55" };
            var rowsAffected = connection.Execute(sqlInsertRecord, session);
            // Console.WriteLine($"\nNew session started: {formattedDay} at {startHour} inserted.");

            Console.WriteLine($"\n{rowsAffected} row(s) inserted.");

            var sqlInsert = "SELECT id, date, startTime FROM sessions;";

            dataFromDB = connection.Query<Sessions>(sqlInsert).ToList();

            Console.WriteLine("Sessions:");

            showResults.ShowTable(dataFromDB);

            return dataFromDB;
        }
    }
}