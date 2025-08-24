using Flashcards.Controllers;
using Flashcards.Helpers;
using static Flashcards.Enums.Menus;

namespace Flashcards.Views;
public class UserInterface
{
    private readonly CardsController _cardsController = new CardsController();
    private readonly StackController _stackController = new StackController();
    public void Display()
    {
        while (true)
        {
            InterfaceHelpers.DisplayHeader("Main Menu");

            var mainMenuOptions = InterfaceHelpers.GetMenuOptions<MainMenu>();
            var mainMenuChoice = InterfaceHelpers.SelectionPrompt(mainMenuOptions);

            switch (mainMenuChoice)
            {
                case MainMenu.ManageCards:
                    _cardsController.DisplayInterface();
                    break;
                case MainMenu.ManageStacks:
                    _stackController.DisplayInterface();
                    break;
                case MainMenu.StartStudy:
                    break;
                case MainMenu.ViewStudyHistory:
                    break;
                case MainMenu.ExitApplication:
                    Environment.Exit(0);
                    break;
            }

        }
    }
}
