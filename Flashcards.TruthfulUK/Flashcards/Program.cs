using Flashcards.Database;
using Flashcards.Views;

var seeder  = new SeedData();
var ui      = new UserInterface();

seeder.InsertSeedData();
ui.Display();