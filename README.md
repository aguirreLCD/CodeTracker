# C# .NET Console Application

This is a simple project to learn how to:

- perform CRUD operations against a real database;
- create and run a .NET console application by using Visual Studio Code and the .NET CLI.
- use SQLite with C# + .NET in VSCode;
- handle Dates and Times;
- use external library;
- create / access Configuration file;
- implement Model / Entity;
- improve CRUD operations -> Controller;
- improve validation;
- improve debug;
- improve documentation;

In Summary, it is a small Application to track some activity. It can be for example, a coding session or even a study session.

# Set Up Your Environment:

### Create a Console Application: Code Tracker App

Create a new folder => CodeTracker;

In Visual Studio Code, choose:

    Create new project

Search for:

    Console Application

Choose a Project Name;

Run your application by running:

`dotnet build`

`dotnet run`

||

Open Visual Studio Code;

Open the terminal in VS Code (Ctrl+ or Cmd+ on Mac);

Run the following command to create a new console app:

`dotnet new console -n CodeTracker`

`cd CodeTracker`

## Create .vscode files to configure build and debug:

```
Command + P:
.NET: Generate Assets for Build and Debug
```

To handle terminal input while debugging, you can use the integrated terminal:  
 In launch.json file, configure console:

    "console": "integratedTerminal",

### Create a .gitignore template

`dotnet new gitignore`

### Using SQLite with C# in VSCode

- Installing Necessary Package:

Microsoft.Data.Sqlite is a lightweight ADO NET provider for SQLite and you can install it with:

`dotnet add package Microsoft.Data.Sqlite`

---

---

# Work in progress...

- [x] Create README file;
- [x] In the root of the project, create a new file named App.config and set the key for DB Connection string;
- [x] Create Database and open Database connection;
- [x] Create Table;
- [] Create CRUD Methods:
  - [] Create session;
  - [] Display sessions;
  - [] Update session;
  - [] Delete session;
- [] Implement Lists;
- [] Validation for user input;
- [] Handle errors:

- [] Use Spectre.Console:
  `dotnet add package Spectre.Console`

  `dotnet add package Spectre.Console.Cli`

- [] Keep practicing;
- [] Improve README file;
- [] Improve documentation;

- [] Export git log history to a text file:

  - `git log -p --all > git_log.txt`

  - `git log --pretty=format:"%ad - %an: %s" > git_log.txt`

  - `git log --pretty=format:'%h was %an, %ar, message: %s' > log.log`

- [] Publish the app  
  `dotnet publish --configuration Release`

---

---

# Learning topics:

- ADO.NET;
- .NET data access;
- Microsoft Data SQLite;
- DB Design;
- SQL, raw SQL, SQLite, SQL Server;
- System Configuration;
- namespace;
- internal classes;
- TimeSpan;
- Stopwatch;

<!--
  - git add .
  - git commit -m " "
  - git push -u origin main -->

<!--
sqlite3
sqlite> .open codesessions.db
sqlite> .mode column
sqlite> .headers on
sqlite> .tables
sqlite> SELECT * FROM sessions;
INSERT INTO sessions (date, duration) VALUES ('today', 'now');
sqlite>.exit
 -->
