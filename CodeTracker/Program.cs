using Microsoft.Data.Sqlite;
using System.Configuration;

namespace code_tracker
{
    class Program
    {
        static void Main(string[] args)
        {
            // var name = UserInput.AskName();
            // var option = UserInput.AskRange();
            // var userName = UserInput.AskUserName();

            Console.Clear();

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
                        case "1": // display Today's sessions
                            SessionController showTable = new();
                            showTable.PrintTodayTable(connection);

                            break;

                        case "2"://  create session
                                 // SessionController createNewSession = new();
                                 // createNewSession.CreateRecord(connection);

                            SessionController insertSession = new();
                            insertSession.DapperInsert(connection);


                            break;

                        case "3": // calculate session duration
                            SessionController calculateSession = new();
                            calculateSession.CalculateSessionTime(connection);

                            break;

                        case "4": // 


                            Console.WriteLine("\n\rPress the Enter key to continue.");
                            break;

                        case "5": // DAPPER

                            SessionController showDapperList = new();
                            showDapperList.GetDataFromDB(connection);

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
