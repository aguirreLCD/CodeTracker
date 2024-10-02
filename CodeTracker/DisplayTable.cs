using System.Collections.Generic;
using Spectre.Console;

namespace code_tracker
{
    internal class DisplayTable
    {
        // Access the List in Other Methods
        internal void ShowTable(List<Sessions> codeSessions)
        {
            // Create a table from Spectre Console:
            var table = new Table();
            table.AddColumn("[red]ID[/]");
            table.AddColumn("[red]Date[/]");
            table.AddColumn("[red]Start[/]");
            table.AddColumn("[red]End[/]");
            table.AddColumn("[red]Duration[/]");

            foreach (var session in codeSessions)
            {
                // Console.WriteLine(session.id);
                // Console.WriteLine(session.date);
                // Console.WriteLine(session.startTime);
                // Console.WriteLine(session.endTime);
                // Console.WriteLine(session.duration);

                table.AddRow($"{session.id}", $"{session.date}", $"{session.startTime}", $"{session.endTime}", $"{session.duration}");
            }
            // Display table from Spectre Console:
            Console.WriteLine("\nCurrent Coding Sessions:");
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
    }
}

// static void Main()
// {
//     List<string> results = GetResultsFromDatabase();
//     DisplayList(results);
// }