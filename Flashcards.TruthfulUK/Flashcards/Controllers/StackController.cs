using Flashcards.Database;
using Flashcards.Helpers;
using Flashcards.Models;
using Spectre.Console;
using static Flashcards.Enums.Menus;

namespace Flashcards.Controllers;
internal class StackController
{
    private readonly StackRepository _stackRepo;
    private readonly List<int> _currentRowIds;

    public StackController()
    {
        _stackRepo = new StackRepository();
        _currentRowIds = new List<int>();
    }

    public void DisplayInterface()
    {
        bool exitMenu = false;

        while (!exitMenu)
        {
            InterfaceHelpers.DisplayHeader("Stacks");

            var stackMenuOptions = InterfaceHelpers.GetMenuOptions<ManageStacksMenu>();
            var stackMenuChoice = InterfaceHelpers.SelectionPrompt(stackMenuOptions);

            switch (stackMenuChoice)
            {
                case ManageStacksMenu.AddStack:
                    AddStack();
                    break;
                case ManageStacksMenu.DeleteStack:
                    DeleteStack();
                    break;
                case ManageStacksMenu.ViewAllStacks:
                    ViewAllStacks();
                    break;
                case ManageStacksMenu.BackToMenu:
                    exitMenu = true;
                    break;
            }
        }
    }

    private void AddStack()
    {
        List<Stack> allStacks = _stackRepo.SelectAllStacks();

        InterfaceHelpers.DisplayHeader($"Add a New Stack");

        string stackName = InterfaceHelpers.StringInputPrompt("[blue]Enter the [bold]Stack name[/]:[/]\n\n");

        if (allStacks.Any(s => s.Name.Equals(stackName, StringComparison.OrdinalIgnoreCase))) {
            AnsiConsole.WriteLine("Stack already exists");
            InterfaceHelpers.PressKeyToContinue();
            return;
        }

        _stackRepo.Insert(stackName);

        AnsiConsole.MarkupLine($"[green]{stackName} has been successfully added.[/]");
        InterfaceHelpers.PressKeyToContinue(); 
    }

    private void DeleteStack()
    {
        ViewAllStacks(deleteCaller: true);

        AnsiConsole.MarkupLine($"[bold]Warning:[/] Deleting a Stack will also delete [red]all cards[/] and [red]all recorded Study Sessions[/] with that Stack. This action cannot be reversed.\n");

        int rowId = InterfaceHelpers.RowIdPrompt(_currentRowIds);

        var confirmation = AnsiConsole.Prompt(
            new TextPrompt<bool>($"Confirm deletion of the {_stackRepo.SelectStackById(rowId)} card stack")
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(false)
                .WithConverter(choice => choice ? "y" : "n"));

        if (!confirmation)
        {
            AnsiConsole.MarkupLine("[red]Deletion action cancelled.[/]");
            InterfaceHelpers.PressKeyToContinue();
            return;    
        }

        _stackRepo.Delete(rowId);
        AnsiConsole.MarkupLine("[green]Stack and any paired cards have been deleted.[/]");

        InterfaceHelpers.PressKeyToContinue();
    }

    private void ViewAllStacks(bool deleteCaller = false)
    {
        List<Stack> allStacks = _stackRepo.SelectAllStacks();

        if (allStacks.Count == 0)
        {
            AnsiConsole.MarkupLine($"The database does not currently contain any Flashcard Stacks.");
            Console.ReadKey();
            return;
        }

        var stackTable = new Table();
        stackTable
            .AddColumn("[white on blue] Stack ID [/]")
            .AddColumn("[white on blue] Stack Name [/]")
            .AddColumn("[white on blue] Total Cards in Stack [/]")
            .ShowRowSeparators()
            .Border(TableBorder.Horizontal)
            .Expand();

        InterfaceHelpers.DisplayHeader($"All Stacks");

        _currentRowIds.Clear();

        foreach (Stack stack in allStacks)
        {
            stackTable.AddRow(
                $"{stack.Id}",
                $"{stack.Name}",
                $"{_stackRepo.GetCardCountForStack(stack.Id)}"
            );
            _currentRowIds.Add(stack.Id);
        }

        AnsiConsole.Write(stackTable);

        if (!deleteCaller) InterfaceHelpers.PressKeyToContinue();
    }
}
