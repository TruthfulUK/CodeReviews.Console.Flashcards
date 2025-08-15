using Flashcards.Config;
using Flashcards.Database;

var config  = new AppConfig();
var db      = new Database(config.GetConnectionString());
var seeder  = new SeedData(db);


db.InitializeDatabase();
seeder.InsertSeedData();

Console.ReadKey();

db.DebugDropTables();