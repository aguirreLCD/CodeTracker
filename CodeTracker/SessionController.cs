
using Microsoft.Data.Sqlite;

namespace code_tracker
{
    internal class SessionController
    {
        internal void DisplayTable(SqliteConnection connection)
        {
            var displayTableCommand = connection.CreateCommand();

            try
            {
                displayTableCommand.CommandText =
                @"
                    SELECT *
                    FROM sessions;
                ";

                using (var reader = displayTableCommand.ExecuteReader())
                {
                    Console.WriteLine("\nCurrent Coding Sessions:\n");
                    Console.Write("{0,-20}", "ID");
                    Console.Write("{0,-20}", "Date");
                    Console.Write("{0,-20}", "Start");
                    Console.Write("{0,-20}", "End");
                    Console.Write("{0,-20}", "Duration");
                    Console.WriteLine();

                    const int FieldWidthLeftAligned = -20;
                    Console.WriteLine();

                    while (reader.Read())
                    {
                        Console.Write($"{reader["id"],FieldWidthLeftAligned}");
                        Console.Write($"{reader["date"],FieldWidthLeftAligned}");
                        Console.Write($"{reader["startTime"],FieldWidthLeftAligned}");
                        Console.Write($"{reader["endTime"],FieldWidthLeftAligned}");
                        Console.Write($"{reader["duration"],FieldWidthLeftAligned}\n");
                        Console.WriteLine();

                    }
                    Console.WriteLine();
                }
            }
            catch (SqliteException message)
            {
                Console.WriteLine(message.Message);
                Console.WriteLine(message.ErrorCode);
                throw;
            }
        }

        internal void CreateRecord(SqliteConnection connection)
        {
            var currentDate = DateTime.Now;

            string formattedDay = currentDate.ToString("dd-MM-yyyy");
            // Console.WriteLine(formattedDay);

            string startHour = currentDate.ToString("HH:mm");
            // Console.WriteLine(startHour);

            var endHour = currentDate.AddHours(2);

            string formattedEndHour = endHour.ToString("HH:mm");
            // Console.WriteLine(formattedEndHour);

            TimeSpan difference = endHour - currentDate;
            // Console.WriteLine($"Difference: {difference}");

            string formattedDurationHours = difference.TotalHours.ToString();
            // Console.WriteLine($"Difference: {formattedDurationHours} hours");

            string formattedDurationMinutes = difference.TotalMinutes.ToString();
            // Console.WriteLine($"Difference: {formattedDurationMinutes} minutes");

            using (var insertTransaction = connection.BeginTransaction())
            {
                var insertCommand = connection.CreateCommand();

                insertCommand.CommandText =
                $"INSERT INTO sessions(date, startTime, endTime, duration) VALUES('{formattedDay}', '{startHour}', '{formattedEndHour}', '{difference}' )";

                insertCommand.ExecuteNonQuery();
                insertTransaction.Commit();

                Console.WriteLine();
                Console.WriteLine($"Session: {formattedDay}\t{startHour}\t {formattedEndHour}\t {difference}\t  inserted.");
            }
        }
    }

}