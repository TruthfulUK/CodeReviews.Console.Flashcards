using Spectre.Console;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Flashcards.Models;
using Microsoft.IdentityModel.Tokens;

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

    public static Stack SelectStackPrompt(List<Stack> stacks)
    {
        var selectedStack = AnsiConsole.Prompt(
        new SelectionPrompt<Stack>()
            .Title("Please select an [blue]option[/]:")
            .AddChoices(stacks)
            .UseConverter(stack => stack.Name));
        return selectedStack;
    }

    public static string StringInputPrompt(string prompt)
    {
        var input = AnsiConsole.Prompt(
            new TextPrompt<string>(prompt)
            .Validate(text => !text.IsNullOrEmpty() 
                ? Spectre.Console.ValidationResult.Success()
                : Spectre.Console.ValidationResult.Error("\n[red]Input cannot be null or empty.[/]\n"))
        );
        return input.Trim();
    }

    public static int RowIdPrompt(List<int> rowIds)
    {
        var inputId = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter an ID # from the above table:")
            .Validate(input =>
            {
                return rowIds.Contains(input)
                ? Spectre.Console.ValidationResult.Success()
                : Spectre.Console.ValidationResult.Error("\n[red]Invalid ID # - please enter an ID # from the table displayed above[/]\n");
            })
        );

        return inputId;
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
