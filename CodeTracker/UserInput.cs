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

        public static int AskRange()
        {
            Console.Clear();

            return AnsiConsole.Prompt(
                new TextPrompt<int>("[green]What is your choice?[/]")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]That's not a valid option[/]")
                    .Validate(option =>
                    {
                        return option switch
                        {
                            < 0 => ValidationResult.Error("[red]You must choose between the range: 0 - 9[/]"),
                            >= 9 => ValidationResult.Error("[red]You must choose between the range 0 - 9[/]"),
                            _ => ValidationResult.Success(),
                        };
                    }));
        }

        public static string AskUserName()
        {
            var userName = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]What is your name?[/]")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]That's not a valid name[/]")
                    .Validate(userName =>
                    {
                        if (string.IsNullOrWhiteSpace(userName))
                        {
                            return ValidationResult.Error("userName cannot be empty.");
                        }
                        if (userName.Length < 3)
                        {
                            return ValidationResult.Error("userName must be at least 3 characters long.");
                        }
                        return ValidationResult.Success();
                    }));

            var currentDate = DateTime.Now;

            // Echo the user's name back to the terminal
            AnsiConsole.WriteLine($"Hello, {userName}! Today is {currentDate.Day:d}/{currentDate.Month:d}/{currentDate.Year:d} at {currentDate:t}");

            return userName;
        }

        public static string AskUserInput()
        {
            var choices = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .PageSize(10)
                    .Title("[green]Code Tracker App[/]")
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .InstructionsText("[grey](Press [blue]<space>[/] to toggle a choice, [green]<enter>[/] to accept)[/]")
                    .AddChoiceGroup("Your main menu options are", new[]
                    {
                        "1. To display all current table in database, type 1",
                        "2. To create new session, type 2",
                        "6. To delete one session by ID, type 6",
                        "0. To exit the program, type 0",
                    }));
            // .AddChoices(new[]
            // {
            //     "1. To display all current table in database, type 1",
            //     "2. To create new session, type 2",
            //     "6. To delete one session by Id, type 6",
            //     "3. To create new session, type 2",
            //     "4. To create new session, type 2",
            //     "5. To create new session, type 2",
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

            string extracted = choice.Substring(0, 1);

            // Console.WriteLine(extracted);
            // Console.WriteLine(choice[0]);
            // Console.WriteLine(choices[0]);

            return extracted;
        }
    }
}


// Console.WriteLine("1. To display all current table in database, type: 1");
// Console.WriteLine("2. To create new session, type: 2");
// Console.WriteLine("3. To search specific code session, type: 3");
// Console.WriteLine("4. To search code sessions by date, type: 4");
// Console.WriteLine("5. To update a code session, type: 5");
// Console.WriteLine("6. To delete a code session by ID, type: 6");
// Console.WriteLine("7. To delete all code sessions, type: 7");