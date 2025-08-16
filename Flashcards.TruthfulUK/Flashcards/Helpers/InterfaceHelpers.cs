using Spectre.Console;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Flashcards.Helpers;
internal static class InterfaceHelpers
{
    public static Dictionary<string, TEnum> GetMenuOptions<TEnum>()
        where TEnum : struct, Enum
    {
        return Enum.GetValues<TEnum>()
            .ToDictionary(option => GetEnumDisplayName(option), option => option);
    }

    public static string GetEnumDisplayName(Enum value)
    {
        return value.GetType()
            .GetMember(value.ToString())[0]
            .GetCustomAttribute<DisplayAttribute>()?.Name
            ?? value.ToString();
    }

    public static TEnum SelectionPrompt<TEnum>(Dictionary<string, TEnum> options)
        where TEnum : struct, Enum
    {
        var selectedKey = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Please select an [blue]option[/]:")
            .AddChoices(options.Keys));

        return options[selectedKey];
    }

    public static void DisplayHeader(string title)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Flashcards")
                .Centered()
                .Color(Color.Blue));

        var rule =
            new Rule($"{title}")
                .RuleStyle("blue dim")
                .Centered();

        AnsiConsole.Write(rule);
    }

    public static void PressKeyToContinue()
    {
        var rule = new Rule().RuleStyle("blue dim");
        AnsiConsole.Write(rule);

        var paddedContinueText =
            new Text("Display paused - press any key to continue",
            new Style(Color.Blue));
        var paddedContinue = new Padder(paddedContinueText).PadTop(1);
        AnsiConsole.Write(paddedContinue);
        Console.ReadKey();
    }
}
