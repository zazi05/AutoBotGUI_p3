using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AutoBotGUI
{
    // Represents a single quiz question
    public class QuizQuestion
    {
        public string Question { get; set; }          // The question text
        public List<string> Options { get; set; }     // Multiple choice options
        public int CorrectIndex { get; set; }         // Index of the correct answer
    }

    // Manages the quiz logic: questions, scoring, progression
    public class QuizManager
    {
        private List<QuizQuestion> _questions;        // List of all quiz questions
        private int _currentIndex;                    // Tracks which question is active
        private int _score;                           // Tracks correct answers

        public QuizManager()
        {
            // Initialize quiz with 10 cybersecurity questions
            _questions = new List<QuizQuestion>
            {
                new QuizQuestion { Question = "What does HTTPS stand for?",
                    Options = new List<string>{ "HyperText Transfer Protocol Secure", "High Transfer Text Protocol Standard", "Hyper Transfer Text Process Secure" },
                    CorrectIndex = 0 },

                new QuizQuestion { Question = "Which of these is a strong password?",
                    Options = new List<string>{ "123456", "Password!", "T!m3$ecure2026" },
                    CorrectIndex = 2 },

                new QuizQuestion { Question = "What is phishing?",
                    Options = new List<string>{ "A type of firewall", "A cyber attack tricking users into revealing information", "Encrypting data with a key" },
                    CorrectIndex = 1 },

                new QuizQuestion { Question = "Which device is commonly used as a hardware security token?",
                    Options = new List<string>{ "USB key", "Smartphone", "Printer" },
                    CorrectIndex = 0 },

                new QuizQuestion { Question = "What does a VPN provide?",
                    Options = new List<string>{ "Faster internet speed", "Secure encrypted connection over public networks", "Free access to websites" },
                    CorrectIndex = 1 },

                new QuizQuestion { Question = "Which of these is NOT malware?",
                    Options = new List<string>{ "Trojan", "Worm", "Firewall" },
                    CorrectIndex = 2 },

                new QuizQuestion { Question = "What is two-factor authentication?",
                    Options = new List<string>{ "Using two passwords", "Using password plus another verification method", "Logging in twice" },
                    CorrectIndex = 1 },

                new QuizQuestion { Question = "Which protocol is used for secure email transfer?",
                    Options = new List<string>{ "SMTP", "IMAP", "SMTPS" },
                    CorrectIndex = 2 },

                new QuizQuestion { Question = "What is ransomware?",
                    Options = new List<string>{ "Software that encrypts files and demands payment", "A tool for recovering lost data", "A type of antivirus program" },
                    CorrectIndex = 0 },

                new QuizQuestion { Question = "Which of these is a common sign of a compromised system?",
                    Options = new List<string>{ "Unexpected pop-ups and slow performance", "Stable performance and no alerts", "Automatic updates running normally" },
                    CorrectIndex = 0 }
            };

            _currentIndex = 0;
            _score = 0;
        }

        // Get the next question in the quiz
        public QuizQuestion GetNextQuestion()
        {
            if (_currentIndex < _questions.Count)
            {
                return _questions[_currentIndex++];
            }
            return null; // No more questions
        }

        // Submit an answer for the current question
        public void SubmitAnswer(int chosenIndex)
        {
            if (_currentIndex > 0)
            {
                var lastQuestion = _questions[_currentIndex - 1];
                if (chosenIndex == lastQuestion.CorrectIndex)
                {
                    _score++;
                }
            }
        }

        // Get the current score
        public int GetScore()
        {
            return _score;
        }

        // Get the total number of questions
        public int GetTotalQuestions()
        {
            return _questions.Count;
        }
    }
}


