using Microsoft.Data.Sqlite;
using System.Configuration;
using Spectre.Console;

namespace code_tracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();

            string? menuSelection = "";

            while (menuSelection != "0")
            {
                var choice = UserInput.AskUserInput();

                if (!String.IsNullOrEmpty(choice))
                {
                    menuSelection = choice.ToLower();
                }
                if (menuSelection == "0")
                {
                    Console.WriteLine("Exiting program...");
                    return;
                }
                Console.Clear();

                // create DB, Table, Open connection
                string? connectionString = ConfigurationManager.AppSettings["ConnectionString"];

                using (var connection = new SqliteConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        DBManager databaseManager = new();
                        databaseManager.CreateDBTable(connection);
                    }
                    catch (SqliteException message)
                    {
                        Console.WriteLine(message.Message);
                        Console.WriteLine(message.ErrorCode);
                    }

                    switch (menuSelection)
                    {
                        case "1": // display Today's sessions
                            SessionController showTable = new();
                            showTable.PrintTodayTable(connection);

                            break;

                        case "2"://  create session
                            SessionController insertSession = new();
                            insertSession.DapperInsert(connection);

                            break;

                        case "3": // calculate today's  session duration
                            SessionController calculateSession = new();
                            calculateSession.CalculateTodaySessionDuration(connection);

                            break;

                        case "4": // 
                            SessionController showSessionsDay = new();
                            showSessionsDay.ShowSessionsByDate(connection, "07-10-2024");

                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            break;

                        case "5": // show DAPPER data
                            SessionController showDapperList = new();
                            showDapperList.GetDuration(connection);

                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            break;

                        case "6": // delete session
                            SessionController deleteSession = new();
                            deleteSession.DeleteRecord(connection);

                            break;

                        case "7": // CalculateSessionDuration
                            SessionController sessionDuration = new();
                            sessionDuration.CalculateSessionDuration(connection, "02-10-2024");

                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            break;

                        case "8": // Update Record
                            SessionController sessionUpdate = new();
                            sessionUpdate.UpdateRecord(connection);

                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            break;

                        case "9": //  calculate
                            SessionController calculate = new();
                            calculate.CalculateDuration(connection, "07-10-2024");

                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            break;



                        case "c": //  calendar
                            var calendar = new Calendar(2024, 10);
                            calendar.AddCalendarEvent(2024, 10, 7);
                            calendar.HighlightStyle(Style.Parse("yellow bold"));
                            AnsiConsole.Write(calendar);

                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            break;

                        default:
                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            break;
                    }
                }
            }
        }
    }
}
