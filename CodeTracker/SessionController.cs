
using Microsoft.Data.Sqlite;
using Spectre.Console;

using Dapper;

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



        internal List<Sessions> ShowSessionTime(SqliteConnection connection)
        {
            Console.WriteLine("Coding Session Duration:");


            var sql = @"SELECT * FROM sessions WHERE date = @date;";

            // Use the Query method to execute the query and return a list of objects    
            List<Sessions> coding = connection.Query<Sessions>(sql, new { date = "03-10-2024" }).ToList();

            Console.WriteLine("\ndapper: print table\n");
            DisplayTable showResults = new();
            showResults.ShowTodayTable(coding);

            return coding;
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

        // TODO:
        // use today WHERE date='02-10-2024';
        // check for null references on formatted hours;
        internal List<Sessions> CalculateSessionTime(SqliteConnection connection)
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
                    WHERE date='03-10-2024'
                   ;
                ";
                using (var reader = calculateCommand.ExecuteReader())
                {
                    // var table = new Table();
                    // table.AddColumn("[red]Date[/]");
                    // table.AddColumn("[red]Start[/]");
                    // table.AddColumn("[red]End[/]");
                    // table.AddColumn("[red]Duration[/]");

                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            var endTime = reader["MaxStartTime"];

                            string? formattedEndHour = endTime.ToString();

                            DateTime theEndOfSession = DateTime.Parse(formattedEndHour);

                            var startTime = reader["MinStartTime"];

                            string? formattedStartHour = startTime.ToString();

                            DateTime theStartOfSession = DateTime.Parse(formattedStartHour);

                            TimeSpan difference = theEndOfSession - theStartOfSession;
                            // Console.WriteLine(difference);
                            // Console.WriteLine(difference.TotalHours);
                            // Console.WriteLine(difference.TotalMinutes);

                            string? formattedDifference = difference.ToString();

                            // table.AddRow($"{reader["date"]}", $"{reader["MinStartTime"]}", $"{reader["MaxStartTime"]}", $"{difference}");

                            var duration = formattedDifference;
                            // Console.WriteLine(formattedDifference);

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
                    }
                    // AnsiConsole.Write(table);

                    // foreach (var session in calculateSessionsTable)
                    // {
                    //     Console.WriteLine(session.date);
                    //     Console.WriteLine(session.startTime);
                    //     Console.WriteLine(session.endTime);
                    //     Console.WriteLine(session.duration);
                    // }
                }
            }
            catch (SqliteException message)
            {
                Console.WriteLine(message.Message);
                Console.WriteLine(message.ErrorCode);
                throw;
            }

            Console.WriteLine("\nToday's Coding Session Duration:");

            // call to ShowTable Method from DisplayTable class
            DisplayTable showResults = new();
            showResults.ShowDurationTable(calculateSessionsTable);

            return calculateSessionsTable;
        }






    }
}