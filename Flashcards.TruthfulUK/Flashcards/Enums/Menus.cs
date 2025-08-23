using System.ComponentModel.DataAnnotations;

namespace Flashcards.Enums;
internal static class Menus
{
    internal enum MainMenu
    {
        [Display(Name = "Manage Cards")]
        ManageCards,

        [Display(Name = "Manage Stacks")]
        ManageStacks,

        [Display(Name = "Start a Study Session")]
        StartStudy,

        [Display(Name = "View Historic Study Sessions")]
        ViewStudyHistory,

        [Display(Name = "Exit Application")]
        ExitApplication
    }

    internal enum ManageCardsMenu
    {
        [Display(Name = "Add a Card")]
        AddCard,

        [Display(Name = "Delete a Card")]
        DeleteCard,

        [Display(Name = "View all Cards within a Stack")]
        ViewCardsFromStack,

        [Display(Name = "Back to Main Menu")]
        BackToMenu
    }
}
