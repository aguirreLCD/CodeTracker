using Microsoft.Data.Sqlite;
using System.Configuration;
using System.Diagnostics;
using Spectre.Console;
using System.Collections.Generic;

namespace code_tracker
{
    class Program
    {
        static void Main(string[] args)
        {
            // var name = UserInput.AskName();

            // var option = UserInput.AskRange();

            // var userName = UserInput.AskUserName();








            string? menuSelection = "";

            while (menuSelection != "0")
            {
                var choice = UserInput.AskUserInput();

                if (!String.IsNullOrEmpty(choice))
                {
                    menuSelection = choice.ToLower();
                }

                // application should only be terminated when the user inserts 0.
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
                        case "1": // display sessions
                            SessionController showTable = new();
                            showTable.DisplayTable(connection);

                            break;

                        case "2"://  create session
                            SessionController createNewSession = new();
                            createNewSession.CreateRecord(connection);

                            break;

                        case "3": // search and display info by session
                            SessionController calculateSession = new();
                            calculateSession.CalculateSessionTime(connection);

                            break;

                        case "4": // search and display session by duration
                            SessionController shooooowww = new();
                            shooooowww.GetData();


                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            break;

                        case "5": // update session
                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            break;

                        case "6": // delete session
                            SessionController deleteSession = new();
                            deleteSession.DeleteRecord(connection);

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
