using System.Collections.Generic;
using Spectre.Console;

namespace code_tracker
{
    internal class DisplayTable
    {
        internal void ShowTable(List<Sessions> codeSessions)
        {
            Console.WriteLine("\n\n");

            foreach (var session in codeSessions)
            {
                Console.WriteLine("FOREACH");

                Console.WriteLine(session.id);
                Console.WriteLine(session.date);
                Console.WriteLine(session.startTime);
                Console.WriteLine(session.endTime);
                Console.WriteLine(session.duration);
            }
        }
    }
}



// static void Main()
// {
//     List<string> results = GetResultsFromDatabase();
//     DisplayList(results);
// }