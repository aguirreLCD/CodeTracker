using System.Collections.Generic;
using Spectre.Console;

namespace code_tracker
{
    internal class DisplayTable
    {
        // Access the List in Other Methods
        internal void ShowTodayTable(List<Sessions> coding)
        {
            // Create a table from Spectre Console:
            var table = new Table();

            table.AddColumn("[red]Date[/]");
            table.AddColumn("[red]Start[/]");

            foreach (var session in coding)
            {
                table.AddRow($"{session.date}", $"{session.startTime}");
            }
            // Display table from Spectre Console:
            Console.WriteLine("\nToday's Coding Sessions:");
            AnsiConsole.Write(table);
        }


        internal void ShowDurationTable(List<Sessions> calculateSessionsTable)
        {
            var table = new Table();
            table.AddColumn("[red]Date[/]");
            table.AddColumn("[red]Start[/]");
            table.AddColumn("[red]End[/]");
            table.AddColumn("[red]Duration[/]");

            foreach (var session in calculateSessionsTable)
            {
                table.AddRow($"{session.date}", $"{session.startTime}", $"{session.endTime}", $"{session.duration}");
            }
            // Display the List
            // Console.WriteLine("\nShowDurationTable:");
            AnsiConsole.Write(table);
        }


        internal void ShowTable(List<Sessions> dapperSession)
        {
            var table = new Table();
            table.AddColumn("[red]ID[/]");
            table.AddColumn("[red]Date[/]");
            table.AddColumn("[red]Start[/]");

            foreach (var session in dapperSession)
            {
                table.AddRow($"{session.id}", $"{session.date}", $"{session.startTime}");
            }
            // Display the List
            // Console.WriteLine("");
            AnsiConsole.Write(table);
        }



        internal void ShowInsertTable(List<Sessions> insertedSessions)
        {
            var table = new Table();

            table.AddColumn("[red]Date[/]");
            table.AddColumn("[red]Start[/]");

            foreach (var session in insertedSessions)
            {
                table.AddRow($"{session.date}", $"{session.startTime}");
            }
            // Display the List
            // Console.WriteLine("");
            AnsiConsole.Write(table);
        }
    }
}

// static void Main()
// {
//     List<string> results = GetResultsFromDatabase();
//     DisplayList(results);
// }