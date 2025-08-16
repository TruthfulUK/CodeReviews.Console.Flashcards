using Flashcards.Helpers;
using static Flashcards.Enums.Menus;

namespace Flashcards.Views;
public class UserInterface
{
    private readonly bool runApp = true;

    public void Display()
    {
        while (runApp)
        {
            InterfaceHelpers.DisplayHeader("Main Menu");

            var mainMenuOptions = InterfaceHelpers.GetMenuOptions<MainMenu>();
            var mainMenuChoice = InterfaceHelpers.SelectionPrompt(mainMenuOptions);

            switch (mainMenuChoice)
            {
                case MainMenu.ManageCards:
                    break;
                case MainMenu.ManageStacks:
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
