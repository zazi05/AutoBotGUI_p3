using System;
using System.Windows;
using System.Speech.Synthesis; // Voice greeting
using System.Data.SQLite;      // SQLite database

namespace AutoBotGUI   
{
    public partial class MainWindow : Window
    {
        private SpeechSynthesizer synth;
        private QuizManager quizManager;
        private QuizQuestion currentQuestion;

        // Connection string for SQLite database file
        private const string ConnectionString = "Data Source=tasks.db;Version=3;";

        public MainWindow()
        {
            InitializeComponent();

            // Voice greeting when app starts
            synth = new SpeechSynthesizer();
            synth.SpeakAsync("Welcome to AutoBot Security Assistant. I will help you stay safe online!");

            // Initial message in chat history
            ChatHistory.AppendText("AutoBot: Hello! Type a message to get cybersecurity tips.\n");

            // Initialize quiz
            quizManager = new QuizManager();
            LoadNextQuestion();

            // Ensure database and table exist
            InitializeDatabase();

            // Load tasks from database into UI
            LoadTasks();
        }

        // Create table if not exists
        private void InitializeDatabase()
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
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Load tasks from database into TaskListBox
        private void LoadTasks()
        {
            TaskListBox.Items.Clear();
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT Id, Title, Description, Reminder, IsCompleted FROM Tasks";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string status = reader.GetInt32(4) == 1 ? "(Completed)" : "(Pending)";
                        TaskListBox.Items.Add($"{reader.GetInt32(0)} - {reader.GetString(1)} - {reader.GetString(2)} - {reader.GetString(3)} {status}");
                    }
                }
            }
        }

        // Chat send button
        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageInput.Text;
            if (!string.IsNullOrWhiteSpace(message))
            {
                ChatHistory.AppendText($"You: {message}\n");
                MessageInput.Clear();

                string botReply = GetBotReply(message);
                ChatHistory.AppendText($"AutoBot: {botReply}\n");
                synth.SpeakAsync(botReply);
            }
        }

        // Chatbot reply logic
        private string GetBotReply(string userMessage)
        {
            string msg = userMessage.ToLower();

            if (msg.Contains("phishing"))
                return "Phishing alert: Never click suspicious links or share personal info via email.";
            if (msg.Contains("malware"))
                return "Malware warning: Keep antivirus updated and avoid downloading unknown files.";
            if (msg.Contains("password"))
                return "Password tip: Use strong, unique passwords and enable two-factor authentication.";
            if (msg.Contains("wifi"))
                return "Wi-Fi safety: Always use secure networks and avoid public Wi-Fi for banking.";
            if (msg.Contains("update"))
                return "Update reminder: Regularly update your software to patch vulnerabilities.";
            if (msg.Contains("firewall"))
                return "Firewall advice: Enable your firewall to block unauthorized access.";
            if (msg.Contains("backup"))
                return "Backup tip: Always back up important files to protect against ransomware.";
            if (msg.Contains("hello"))
                return "Hello! I’m AutoBot, your cybersecurity assistant.";
            if (msg.Contains("bye"))
                return "Goodbye. Stay safe online!";

            // Default reply
            return "Cyber tip: Always think before you click!";
        }

        // Quiz: load next question
        private void LoadNextQuestion()
        {
            currentQuestion = quizManager.GetNextQuestion();
            if (currentQuestion != null)
            {
                QuizQuestionText.Text = currentQuestion.Question;
                QuizOptionsList.ItemsSource = currentQuestion.Options;
            }
            else
            {
                QuizScoreText.Text = $"Quiz finished! Score: {quizManager.GetScore()} / {quizManager.GetTotalQuestions()}";
            }
        }

        // Quiz: handle submit answer
        private void SubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (QuizOptionsList.SelectedIndex >= 0)
            {
                quizManager.SubmitAnswer(QuizOptionsList.SelectedIndex);
                LoadNextQuestion();
            }
        }

        // Task Assistant: add task (insert into database)
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string title = TaskTitleBox.Text;
            string description = TaskDescriptionBox.Text;
            string reminder = TaskReminderBox.Text;

            if (!string.IsNullOrWhiteSpace(title))
            {
                using (var conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();
                    string sql = "INSERT INTO Tasks (Title, Description, Reminder, IsCompleted) VALUES (@t,@d,@r,0)";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@t", title);
                        cmd.Parameters.AddWithValue("@d", description);
                        cmd.Parameters.AddWithValue("@r", reminder);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Clear input boxes
                TaskTitleBox.Clear();
                TaskDescriptionBox.Clear();
                TaskReminderBox.Clear();

                // Refresh list
                LoadTasks();
            }
        }

        // Task Assistant: mark task complete (update database)
        private void CompleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem != null)
            {
                int id = int.Parse(TaskListBox.SelectedItem.ToString().Split('-')[0].Trim());
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
                LoadTasks();
            }
        }

        // Task Assistant: delete task (remove from database)
        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem != null)
            {
                int id = int.Parse(TaskListBox.SelectedItem.ToString().Split('-')[0].Trim());
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
                LoadTasks();
            }
        }
    }
}
