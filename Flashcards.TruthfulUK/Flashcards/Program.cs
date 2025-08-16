using Flashcards.Config;
using Flashcards.Database;
using Flashcards.Views;

var config  = new AppConfig();
var db      = new Database(config.GetConnectionString());
var seeder  = new SeedData(db);
var ui      = new UserInterface();

db.InitializeDatabase();
seeder.InsertSeedData();
ui.Display();