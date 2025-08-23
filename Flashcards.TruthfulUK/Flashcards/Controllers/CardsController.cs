using Flashcards.Database;
using Flashcards.Helpers;
using Flashcards.Models;

namespace Flashcards.Controllers;
internal class CardsController
{
    private readonly CardRepository _cardRepo;
    private readonly StackRepository _stackRepo;

    public CardsController()
    {
        _cardRepo = new CardRepository();
        _stackRepo = new StackRepository();
    }

    public void DisplayInterface()
    {
        InterfaceHelpers.DisplayHeader("Cards");

        List<Card> cards = _cardRepo.GetAllCardsByStackId(1);

        foreach (Card card in cards)
        {
            Console.WriteLine($"Question: {card.Front}");
            Console.WriteLine($"Answer: {card.Back}\n");
        }

        Console.ReadKey();
    }
}
