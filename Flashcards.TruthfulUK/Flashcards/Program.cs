using Flashcards.Database;
using Flashcards.Views;

var db      = Database.Instance;
var seeder  = new SeedData(db);
var ui      = new UserInterface();

db.InitializeDatabase();
seeder.InsertSeedData();
ui.Display();