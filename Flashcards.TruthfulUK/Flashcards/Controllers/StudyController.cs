using Flashcards.Database;
using Flashcards.Helpers;
using Flashcards.Models;
using Spectre.Console;
using static Flashcards.Enums.Menus;

namespace Flashcards.Controllers;
internal class StudyController
{
    private readonly CardRepository _cardRepo;
    private readonly StackRepository _stackRepo;
    private readonly StudyRepository _studyRepo;

    public StudyController()
    {
        _cardRepo = new CardRepository();
        _stackRepo = new StackRepository();
        _studyRepo = new StudyRepository();
    }

    public void DisplayInterface()
    {
        bool exitMenu = false;

        while (!exitMenu)
        {
            InterfaceHelpers.DisplayHeader("Study Sessions");

            var studyMenuOptions = InterfaceHelpers.GetMenuOptions<StudySessionMenu>();
            var studyMenuChoice = InterfaceHelpers.SelectionPrompt(studyMenuOptions);

            switch (studyMenuChoice)
            {
                case StudySessionMenu.StartStudy:
                    StudySession();
                    break;
                case StudySessionMenu.ViewStudyHistory:
                    ViewStudyHistory();
                    break;
                case StudySessionMenu.BackToMenu:
                    exitMenu = true;
                    break;
            }
        }  
    }

    private void StudySession()
    {
        InterfaceHelpers.DisplayHeader($"Select a Flashcard Stack to Study");
        Stack selectedStack = InterfaceHelpers.SelectStackPrompt(_stackRepo.SelectAllStacks());

        var random = new Random();
        int currentCard = 1;
        int score = 0;

        List<Card> cards = _cardRepo
            .GetAllCardsByStackId(selectedStack.Id)
            .OrderBy(x => random.Next()).ToList();

        foreach (Card card in cards)
        {
            InterfaceHelpers.DisplayHeader($"{selectedStack.Name}: {currentCard}/{cards.Count}");

            string userAnswer = InterfaceHelpers.StringInputPrompt($"{card.Front}?: ");

            if (userAnswer.Equals(card.Back, StringComparison.OrdinalIgnoreCase)) 
            {
                score++;
                AnsiConsole.MarkupLine($"[green]Correct![/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Wrong, the correct answer is: [/]{card.Back}");
            }

            InterfaceHelpers.PressKeyToContinue();
            currentCard++;
        }

        decimal percentScore = ((decimal)score / cards.Count) * 100;
        _studyRepo.Insert(percentScore, selectedStack.Id);

        InterfaceHelpers.DisplayHeader($"{selectedStack.Name} Study Session");

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"You answered {score} out of {cards.Count} Flashcards correctly!");
        AnsiConsole.MarkupLine($"Your recorded Study Session score is [bold]{percentScore:F2}%[/].");
        AnsiConsole.WriteLine();

        InterfaceHelpers.PressKeyToContinue();
    }

    private void ViewStudyHistory()
    {
        InterfaceHelpers.DisplayHeader($"Study Session History");
        Stack selectedStack = InterfaceHelpers.SelectStackPrompt(_stackRepo.SelectAllStacks());
        List<StudySession> sessions = _studyRepo.GetAllStudySessionsByStackId(selectedStack.Id);

        if (sessions.Count == 0)
        {
            AnsiConsole.MarkupLine($"There are no logged Study Sessions for the {selectedStack.Name} stack.");
            Console.ReadKey();
            return;
        }

        var sessionTable = new Table();
        sessionTable
            .AddColumn("[white on blue] Study Session Time [/]")
            .AddColumn("[white on blue] Accuracy Score (%) [/]")
            .ShowRowSeparators()
            .Border(TableBorder.Horizontal)
            .Expand();

        InterfaceHelpers.DisplayHeader($"Study Sessions for {selectedStack.Name}");

        foreach (StudySession session in sessions)
        {
            sessionTable.AddRow(
                $"{session.SessionTime}",
                $"{session.Score:F2}%"
            );
        }

        AnsiConsole.Write(sessionTable);

        InterfaceHelpers.PressKeyToContinue();
    }
}
