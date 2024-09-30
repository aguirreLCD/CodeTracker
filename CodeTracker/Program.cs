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



            while (menuSelection != "0")
            {

                Console.Clear();

                // Ask the user a couple of simple questions
                var name = AnsiConsole.Ask<string>("What's your name?");
                // var age = AnsiConsole.Ask<int>("What's your age?");

                // Echo the name and age back to the terminal
                // AnsiConsole.WriteLine($"So you're {name} and you're {age} years old");

                var currentDate = DateTime.Now;

                Console.WriteLine($"{Environment.NewLine}Hello {name}! Today is {currentDate.Day:d}/{currentDate.Month:d}/{currentDate.Year:d} at {currentDate:t}");
                // Console.WriteLine("------------------------\n");
        

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Welcome to the Code Tracker App in C#[/]")
                        .PageSize(10)
                        .AddChoices(new[] {
                            "Your main menu options are:",
                            "1. To display all current table in database, type 1",
                            "2. To create new session, type 2",
                            "0. To exit the program, type 0",
                        }));
                // Echo the choice back to the terminal
                Console.WriteLine($"{choice}");

                Console.WriteLine($"{choice.StartsWith("1")}");
                Console.WriteLine($"{choice.StartsWith("2")}");
                Console.WriteLine($"{choice.StartsWith("0")}");



                // Console.WriteLine("Welcome to the Code Tracker App in C#");
                // Console.WriteLine("------------------------\n");

                // Console.WriteLine("Your main menu options are:");
                // Console.WriteLine("------------------------\n");

                // Console.WriteLine("1. To display all current table in database, type: 1");
                // Console.WriteLine("2. To create new session, type: 2");
                // Console.WriteLine("3. To search specific code session, type: 3");
                // Console.WriteLine("4. To search code sessions by date, type: 4");
                // Console.WriteLine("5. To update a code session, type: 5");
                // Console.WriteLine("6. To delete a code session, type: 6");
                // Console.WriteLine("7. To delete all code sessions, type: 7");

                // Console.WriteLine();
                // Console.WriteLine("Enter your option (or type 0 to Exit the program)");
                // Console.WriteLine();


                readInputResult = Console.ReadLine();
                // readInputResult = choice;

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