using Spectre.Console;

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
                        "0. To exit the program, type 0",
                        "1. To show Today's sessions, type 1",
                        "2. To start a new session, type 2",
                        "3. To calculate today's session duration, type 3",
                        "4. To show sessions by date, type 4",
                        "5. To Display->ShowTable from DAPPER, type 5",
                        "6. To delete one session by ID, type 6",
                        "7. To calculate session duration by date, type 7",
                    })
            .AddChoices(new[]
            {
                "8. To update session, type 8",
                "9. To calculate , type 9",
                "c. To show a calendar, type c",
            }));

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

            return extracted;
        }
    }
}

