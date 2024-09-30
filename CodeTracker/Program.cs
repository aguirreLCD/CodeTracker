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
            Console.Clear();

            string? menuSelection = "";

            var name = UserInput.AskName();

            while (menuSelection != "0")
            {
                var choice = UserInput.AskUserInput();
                
                if (!String.IsNullOrEmpty(choice))
                {
                    menuSelection = choice.ToLower();
                }

                var acceptableMenuOption = "1 2 3 4 5 6 7 8 0".Split();

                while (!acceptableMenuOption.Contains(menuSelection))
                {
                    // application should only be terminated when the user inserts 0.
                    if (menuSelection.StartsWith("0"))
                    {
                        Console.WriteLine("Exiting program...");
                        return;
                    }

                    if (menuSelection.StartsWith("1"))
                    {
                        menuSelection = "1";
                    }

                    if (menuSelection.StartsWith("2"))
                    {
                        menuSelection = "2";
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
                            break;

                        case "2"://  create session
                            SessionController createNewSession = new();
                            createNewSession.CreateRecord(connection);
                            break;

                        case "3": // search and display info by session
                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            break;

                        case "4": // search and display session by duration
                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            break;

                        case "5": // update session
                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            break;

                        case "6": // delete session
                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            break;

                        case "7": // delete all database 
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
