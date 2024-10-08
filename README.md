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
- [x] Create a model class: Session.cs;
- [] Create CRUD Methods:

  - [x] Create session;
  - [x] Display sessions;

    - PrintTable() method => connect to SQLite DB, execute a query, create a Table from Spectre.Console, reader Add table rows, Display table from Spectre.Console;

    - GetResultsFromDatabase() method => create new List that use a model class: Sessions.cs, connect to SQLite DB, execute a query, use SQLiteDataReader to Add (store) rows into List (using ToString()), call ShowTable method to display table using Spectre.Console;

  - [] Update session;
  - [x] Delete session;

- [x] Implement Lists;
- [] Validation for user input;
- [] Handle errors:

- [x] Use Spectre.Console:  
       `dotnet add package Spectre.Console`

  `dotnet add package Spectre.Console.Cli`

- [x] Create a git branch:  
- [] Install Dapper (micro) ORM: Object Relational Mapping library:    
  `dotnet add package Dapper`

- [] Use Dapper ORM to data access;

- [] Keep practicing;
- [] Improve README file;
- [] Improve documentation;

- [x] Export git log history to a text file:

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
- Retrieve data from the database: <>List;
- Use the Microsoft.Data.Sqlite library to query the database and populate a List<Session>;

- Dapper is an open-source object-relational mapping (ORM) library for.NET and.NET Core applications. The library allows developers quickly and easily access data from databases without the need to write tedious code.
<!--

git add .

git commit -m " "

git push -u origin main

git log --pretty=format:"%ad - %an: %s" > git_log.txt

git log --pretty=format:'%h was %an, %ar, message: %s' > log.log

-->

<!--

sqlite3

.open codesessions.db

.mode column

.headers on

.tables
sqlite> SELECT * FROM sessions;
INSERT INTO sessions (date, duration) VALUES ('today', 'now');
sqlite>.exit
 -->

<!--


/Users/lilian/Dev/cSharpProjects/CodeTracker/CodeTracker/SessionController.cs(213,51): warning CS8600: Converting null literal or possible null value to non-nullable type. [/Users/lilian/Dev/cSharpProjects/CodeTracker/CodeTracker/CodeTracker.csproj]
/Users/lilian/Dev/cSharpProjects/CodeTracker/CodeTracker/SessionController.cs(216,67): warning CS8604: Possible null reference argument for parameter 's' in 'DateTime DateTime.Parse(string s)'. [/Users/lilian/Dev/cSharpProjects/CodeTracker/CodeTracker/CodeTracker.csproj]
/Users/lilian/Dev/cSharpProjects/CodeTracker/CodeTracker/SessionController.cs(223,53): warning CS8600: Converting null literal or possible null value to non-nullable type. [/Users/lilian/Dev/cSharpProjects/CodeTracker/CodeTracker/CodeTracker.csproj]
/Users/lilian/Dev/cSharpProjects/CodeTracker/CodeTracker/SessionController.cs(226,69): warning CS8604: Possible null reference argument for parameter 's' in 'DateTime DateTime.Parse(string s)'. [/Users/lilian/Dev/cSharpProjects/CodeTracker/CodeTracker/CodeTracker.csproj]


 -->

<!--
01/10=>
To save a database table into a List<> using Microsoft.Data.Sqlite,
- how to connect to an SQLite database, execute a query, and store the results in a List<Sessions> -->

<!--

03/10=> 
To access the List in Other Methods

 -->

<!-- Internal error in the debugger.
Exception = System.Net.Sockets.SocketException
Message = Broken pipe
Code = 80004005
at System.Net.Sockets.Socket.Send(ReadOnlySpan`1 buffer, SocketFlags socketFlags)
   at System.IO.Pipes.PipeStream.WriteCore(ReadOnlySpan`1 buffer)
Internal error in the debugger. -->


<!-- 

 -->