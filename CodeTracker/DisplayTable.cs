using Spectre.Console;

namespace code_tracker
{
    internal class DisplayTable
    {
        internal void ShowTable(List<Sessions> dapperSession)
        {
            var table = new Table();

            table.AddColumn("[red]ID[/]");
            table.AddColumn("[red]Date[/]");
            table.AddColumn("[red]Start[/]");
            table.AddColumn("[red]End[/]");
            table.AddColumn("[red]Duration[/]");

            foreach (var session in dapperSession)
            {
                table.AddRow($"{session.id}", $"{session.date}", $"{session.startTime}", $"{session.endTime}", $"{session.duration}");
            }

            AnsiConsole.Write(table);
        }


        internal void ShowTableData(List<Sessions> dapperSession)
        {
            var table = new Table();

            table.AddColumn("[red]ID[/]");
            table.AddColumn("[red]Date[/]");
            table.AddColumn("[red]Start[/]");
            table.AddColumn("[red]Duration[/]");

            foreach (var session in dapperSession)
            {
                table.AddRow($"{session.id}", $"{session.date}", $"{session.startTime}", $"{session.duration}");
            }

            AnsiConsole.Write(table);
        }
    }
}

