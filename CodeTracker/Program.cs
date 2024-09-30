using Microsoft.Data.Sqlite;
using System.Configuration;
using System.Diagnostics;
using Spectre.Console;

namespace code_tracker
{
    class Program
    {
        static void Main(string[] args)
        {
            string? readInputResult = "";
            string? menuSelection = "";

            var name = UserInput.AskName();
            var choice = UserInput.AskUserInput();

            Console.WriteLine($"{choice}");


            while (menuSelection != "0")
            {

                readInputResult = Console.ReadLine();

                var acceptableMenuOption = "1 2 3 4 5 6 7 8".Split();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                if (readInputResult != null)
                {
                    // validate for menu options
                    while (!acceptableMenuOption.Contains(readInputResult))
                    {
                        // application should only be terminated when the user inserts 0.
                        if (readInputResult == "0")
                        {
                            stopwatch.Stop();
                            string sessionDuration = String.Format($"{stopwatch.Elapsed.TotalSeconds}");

                            Console.WriteLine("Exiting program...");
                            return;
                        }
                        Console.WriteLine("Enter your option (or type 0 to exit the program)");
                        Console.WriteLine();
                        readInputResult = Console.ReadLine();
                    }
                    if (!String.IsNullOrEmpty(readInputResult))
                    {
                        menuSelection = readInputResult.ToLower();
                    }
                }

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
                        case "1": // display sessions
                            SessionController showTable = new();
                            showTable.DisplayTable(connection);

                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            readInputResult = Console.ReadLine();
                            break;

                        case "2"://  create session
                            SessionController createNewSession = new();
                            createNewSession.CreateRecord(connection);

                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            readInputResult = Console.ReadLine();
                            break;

                        case "3": // search and display info by session
                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            readInputResult = Console.ReadLine();
                            break;

                        case "4": // search and display session by duration
                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            readInputResult = Console.ReadLine();
                            break;

                        case "5": // update session
                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            readInputResult = Console.ReadLine();
                            break;

                        case "6": // delete session
                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            readInputResult = Console.ReadLine();
                            break;

                        case "7": // delete all database 
                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            readInputResult = Console.ReadLine();
                            break;

                        default:
                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            readInputResult = Console.ReadLine();
                            break;
                    }
                }
            }
        }
    }
}