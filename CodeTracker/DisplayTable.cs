using System.Collections.Generic;
using Spectre.Console;

namespace code_tracker
{
    internal class DisplayTable
    {
        // Access the List in Other Methods
        internal void ShowTable(List<Sessions> codeSessions)
        {
            // Console.WriteLine("\n\n");

            // Create a table
            var table = new Table();
            table.AddColumn("[red]ID[/]");
            table.AddColumn("[red]Date[/]");
            table.AddColumn("[red]Start[/]");
            table.AddColumn("[red]End[/]");
            table.AddColumn("[red]Duration[/]");

            foreach (var session in codeSessions)
            {
                // Console.WriteLine("DisplayTable class -> ShowTable method");
                // Console.WriteLine(session.id);
                // Console.WriteLine(session.date);
                // Console.WriteLine(session.startTime);
                // Console.WriteLine(session.endTime);
                // Console.WriteLine(session.duration);

                table.AddRow($"{session.id}", $"{session.date}", $"{session.startTime}", $"{session.endTime}", $"{session.duration}");

            }

            Console.WriteLine("\nCurrent Coding Sessions from ShowTable:");

            // Display the List

            AnsiConsole.Write(table);

        }
    }
}

// static void Main()
// {
//     List<string> results = GetResultsFromDatabase();
//     DisplayList(results);
// }