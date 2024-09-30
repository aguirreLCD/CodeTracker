using Spectre.Console;
using Microsoft.Data.Sqlite;

namespace code_tracker
{
    internal class UserInput
    {

        public static string AskName()
        {
            Console.Clear();

            var name = AnsiConsole.Ask<string>("[red]What's your name?[/]");

            var currentDate = DateTime.Now;

            Console.WriteLine($"{Environment.NewLine}Hello {name}! Today is {currentDate.Day:d}/{currentDate.Month:d}/{currentDate.Year:d} at {currentDate:t}");
            Console.WriteLine();

            return name;
        }


        public static string AskUserInput()
        {
            var choices = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .PageSize(10)
                    .Title("[green]Welcome to the Code Tracker App in C#.[/]")
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .InstructionsText("[grey](Press [blue]<space>[/] to toggle a choice, [green]<enter>[/] to accept)[/]")
                    .AddChoiceGroup("Your main menu options are", new[]
                    {
                        "1. To display all current table in database, type 1",
                        "2. To create new session, type 2",
                        "0. To exit the program, type 0",
                    }));
            // .AddChoices(new[]
            // {
            //     "1. To display all current table in database, type 1",
            //     "2. To create new session, type 2",
            //     "0. To exit the program, type 0",
            // }));

            var choice = choices.Count == 1 ? choices[0] : null;
            if (string.IsNullOrWhiteSpace(choice))
            {
                choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .EnableSearch()
                        .Title("Ok, but if you could only choose [green]one[/]?")
                        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                        .AddChoices(choices));
            }

            AnsiConsole.MarkupLine("You selected: [yellow]{0}[/]", choice);
            return choice;
        }
    }
}



// Console.WriteLine("1. To display all current table in database, type: 1");
// Console.WriteLine("2. To create new session, type: 2");
// Console.WriteLine("3. To search specific code session, type: 3");
// Console.WriteLine("4. To search code sessions by date, type: 4");
// Console.WriteLine("5. To update a code session, type: 5");
// Console.WriteLine("6. To delete a code session, type: 6");
// Console.WriteLine("7. To delete all code sessions, type: 7");