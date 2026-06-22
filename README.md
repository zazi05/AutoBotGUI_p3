# AutoBotGUI PART 3

## Overview
AutoBotGUI is a Windows Presentation Foundation (WPF) application built on .NET Framework 4.8.  
It integrates multiple features including:
- Chat assistant
- Task manager with CRUD functionality (SQLite database)
- Quiz manager

The goal of this project is to demonstrate practical skills in GUI design, database integration, and C# programming.

---

## Features
- **Chat Assistant**: Provides interactive responses to user input.
- **Task Manager**: Add, complete, and delete tasks with persistence using SQLite.
- **Quiz Manager**: Simple quiz functionality for practice and learning.

---

## Requirements
- Visual Studio 2022 (v18.7.0)
- .NET Framework 4.8
- NuGet package: `System.Data.SQLite`

---

## Setup Instructions
1. Clone or download the project into Visual Studio.
2. Open the solution file (`AutoBotGUI.sln`).
3. Install required NuGet packages:
   - Open **Tools → NuGet Package Manager → Package Manager Console**.
   - Run:
     ```
     Install-Package System.Data.SQLite
     ```
4. Build the solution to restore dependencies.
5. Run the application (F5). A `tasks.db` file will be created automatically in the `bin/Debug` folder.

---

## Usage
- **Add Task**: Enter title, description, and reminder, then click "Add".
- **Complete Task**: Select a task from the list and click "Complete".
- **Delete Task**: Select a task and click "Delete".
- **Quiz Manager**: Launch quizzes from the menu to test knowledge.

---

## Database
- SQLite database file: `tasks.db`
- Table: `Tasks`
  - `Id` (INTEGER, Primary Key)
  - `Title` (TEXT)
  - `Description` (TEXT)
  - `Reminder` (TEXT)
  - `IsCompleted` (INTEGER, 0 = pending, 1 = done)

---

## Notes
- The database file (`tasks.db`) is stored in the output folder (`bin/Debug`).
- Tasks persist between sessions.
- This project is intended for academic demonstration purposes.
