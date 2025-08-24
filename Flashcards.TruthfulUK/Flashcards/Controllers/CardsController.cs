using Flashcards.Database;
using Flashcards.Helpers;
using Flashcards.Models;
using Spectre.Console;
using static Flashcards.Enums.Menus;

namespace Flashcards.Controllers;
internal class CardsController
{
    private readonly CardRepository _cardRepo;
    private readonly StackRepository _stackRepo;
    private readonly List<int> _currentRowIds;

    public CardsController()
    {
        _cardRepo = new CardRepository();
        _stackRepo = new StackRepository();
        _currentRowIds = new List<int>();
    }

    public void DisplayInterface()
    {
        bool exitMenu = false;

        while (!exitMenu)
        {
            InterfaceHelpers.DisplayHeader("Cards");

            var cardMenuOptions = InterfaceHelpers.GetMenuOptions<ManageCardsMenu>();
            var cardMenuChoice = InterfaceHelpers.SelectionPrompt(cardMenuOptions);

            switch (cardMenuChoice)
            {
                case ManageCardsMenu.AddCard:
                    AddCard();
                    break;
                case ManageCardsMenu.DeleteCard:
                    DeleteCard();
                    break;
                case ManageCardsMenu.ViewCardsFromStack:
                    ViewCardsFromStack();
                    break;
                case ManageCardsMenu.BackToMenu:
                    exitMenu = true;
                    break;
            }
        }
    }

    private void AddCard()
    {
        Stack stack = InterfaceHelpers.SelectStackPrompt(_stackRepo.SelectAllStacks());

        InterfaceHelpers.DisplayHeader($"Adding a New Card to {stack.Name}");

        string cardFront = InterfaceHelpers.StringInputPrompt("[blue]Enter the [bold]Card Front text[/]:[/]\n\n");
        string cardBack = InterfaceHelpers.StringInputPrompt("[blue]Enter the [bold]Card Back text[/]:[/]\n\n");

        _cardRepo.Insert(stack.Id, cardFront, cardBack);

        AnsiConsole.MarkupLine($"[green]Card has been successfully added to the {stack.Name} card stack.[/]");
        InterfaceHelpers.PressKeyToContinue(); 
    }

    private void DeleteCard()
    {
        ViewCardsFromStack(deleteCaller: true);

        int rowId = InterfaceHelpers.RowIdPrompt(_currentRowIds);
        _cardRepo.Delete(rowId);
    }

    private void ViewCardsFromStack(bool deleteCaller = false)
    {
        Stack stack = InterfaceHelpers.SelectStackPrompt(_stackRepo.SelectAllStacks());
        List<Card> cards = _cardRepo.GetAllCardsByStackId(stack.Id);

        if (cards.Count == 0)
        {
            AnsiConsole.MarkupLine($"There are no cards currently in the {stack.Name} stack.");
            Console.ReadKey();
            return;
        }

        var cardTable = new Table();
        cardTable
            .AddColumn("[white on blue] Card Row ID [/]")
            .AddColumn("[white on blue] Card Front (Q)[/]")
            .AddColumn("[white on blue] Card Back (A)[/]")
            .ShowRowSeparators()
            .Border(TableBorder.Horizontal)
            .Expand();

        InterfaceHelpers.DisplayHeader($"All Cards in {stack.Name}");

        _currentRowIds.Clear();

        foreach (Card card in cards)
        {
            cardTable.AddRow(
                    $"{card.Id}",
                    $"{card.Front}",
                    $"{card.Back}"
            );
            _currentRowIds.Add(card.Id);
        }

        AnsiConsole.Write(cardTable);

        if (!deleteCaller) InterfaceHelpers.PressKeyToContinue();
    }
}
