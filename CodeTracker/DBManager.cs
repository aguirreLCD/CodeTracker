using Microsoft.Data.Sqlite;

namespace code_tracker
{
    internal class DBManager
    {
        internal void CreateDBTable(SqliteConnection connection)
        {
            var createTableCommand = connection.CreateCommand();

            createTableCommand.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS sessions (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    date TEXT, 
                    startTime TEXT,
                    endTime TEXT,
                    duration TEXT);   
            ";

            createTableCommand.ExecuteNonQuery();
        }
    }
}
