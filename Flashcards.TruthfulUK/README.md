# Flashcards

[The Flashcards Project](https://thecsharpacademy.com/project/13/coding-tracker) is a CSharpAcademy roadmap project that introduces you to using SQL Server.

Developed with C#, Dapper, Spectre Console and SQL Server 2022 Express with a Local DB.

# Requirements:
- :white_check_mark: This is an application where the users will create Stacks of Flashcards.
- :white_check_mark: You'll need two different tables for stacks and flashcards. The tables should be linked by a foreign key.
- :white_check_mark: Stacks should have an unique name.
- :white_check_mark: Every flashcard needs to be part of a stack. If a stack is deleted, the same should happen with the flashcard.
- :white_check_mark: You should use DTOs to show the flashcards to the user without the Id of the stack it belongs to.
- :white_check_mark: When showing a stack to the user, the flashcard Ids should always start with 1 without gaps between them. If you have 10 cards and number 5 is deleted, the table should show Ids from 1 to 9.
- :white_check_mark: After creating the flashcards functionalities, create a "Study Session" area, where the users will study the stacks. All study sessions should be stored, with date and score.
- :white_check_mark: The study and stack tables should be linked. If a stack is deleted, it's study sessions should be deleted.
- :white_check_mark: The project should contain a call to the study table so the users can see all their study sessions. This table receives insert calls upon each study session, but there shouldn't be update and delete calls to it.

# Dependencies

- Microsoft.Data.SqlClient (6.1.1)
- Microsoft.Extensions.Configuration.Json (9.0.8)
- Spectre.Console (0.50.0)
- Dapper (2.1.66)

# Features

### :floppy_disk: Database Initialization

Provide the connection string to the SQL Server in `appsettings.json`. You will need to create a Database within your server. Upon first starting the application the application will create the necessary tables. 

For debugging / testing purposes it will seed it with 2 Flashcard Stacks (CSharp and SQL) and 5 cards for each Stack. It will also generate 5 Study Sessions for each Flashcard Stack.

### :flower_playing_cards: Manage Cards

**Add a Card:** User is to create a new card for an existing Flashcard Stack.

**Delete a Card:** User is able to delete a specific card from a Flashcard Stack.

**View all Cards within a Stack:** User is able to view all cards paired to the selected Stack.
  
### :briefcase: Manage Stacks

**Add a New Stack:** User is able to create a new Flashcard Stack.

**Delete a Stack:** User is able to delete a Flashcard Stack.

> [!CAUTION]
> Deleting a Stack also removes all associated cards and study sessions. You will be prompted to confirm deletion.

**View all Stacks:** User is able to view all Flashcard Stacks and the number of cards associated with that stack.

### :man_teacher: My Study Sessions

**Start a Study Session:** User is able to select a Flashcard Stack to start a Study Session with. Flashcard questions will be presented to the user in a randomized order and answers will be matched to the Flashcard answer (non-case sensitive). Results of the session will be recorded with the current date, time and total answer accuracy in a percentage (e.g, 80% correct answers).

**View Study Session History** User is able to view all past Study Sessions for the selected stack. 

# Resources Used

- [Microsoft Learn - Application Configuration File](https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/store-custom-information-config-file)
- [Microsoft Learn - DateTime Struct](https://learn.microsoft.com/en-us/dotnet/api/system.datetime?view=net-9.0)
- [Spectre Console Docs](https://spectreconsole.net/)
- [Learn Dapper](https://www.learndapper.com/)
- [SQLBolt - Interactive SQL Excercises](https://sqlbolt.com)
- [W3Schools SQL Data Types](https://www.w3schools.com/sql/sql_datatypes.asp)
- [DoableDanny - Singleton Pattern - C# Design Patterns](https://www.youtube.com/watch?v=5Po6QPy0-lw&list=PLxkN9e3dfloHD01FzTGVYWYgGgtepuQIg&index=41)
- ChatGPT (various models / GPTs)