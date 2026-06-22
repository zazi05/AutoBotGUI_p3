using System.Data.SQLite;
using System.Collections.Generic;

namespace AutoBotGUI
{
    public static class DatabaseHelper
    {
        // Connection string tells SQLite where to store the database file
        private const string ConnectionString = "Data Source=tasks.db;Version=3;";

        // Create the table if it does not exist
        public static void Initialize()
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"CREATE TABLE IF NOT EXISTS Tasks (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Title TEXT,
                                Description TEXT,
                                Reminder TEXT,
                                IsCompleted INTEGER)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery(); // Run the SQL command
                }
            }
        }

        // Add a new task into the database
        public static void AddTask(string title, string description, string reminder)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Tasks (Title, Description, Reminder, IsCompleted) VALUES (@t,@d,@r,0)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    // Parameters prevent SQL injection and handle values safely
                    cmd.Parameters.AddWithValue("@t", title);
                    cmd.Parameters.AddWithValue("@d", description);
                    cmd.Parameters.AddWithValue("@r", reminder);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Read all tasks from the database
        public static List<string> GetTasks()
        {
            var tasks = new List<string>();
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT Id, Title, IsCompleted FROM Tasks";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Show task status as Done or Pending
                        string status = reader.GetInt32(2) == 1 ? "[Done]" : "[Pending]";
                        tasks.Add($"{reader.GetInt32(0)} - {reader.GetString(1)} {status}");
                    }
                }
            }
            return tasks;
        }

        // Mark a task as completed
        public static void CompleteTask(int id)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "UPDATE Tasks SET IsCompleted=1 WHERE Id=@id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Delete a task by ID
        public static void DeleteTask(int id)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "DELETE FROM Tasks WHERE Id=@id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
