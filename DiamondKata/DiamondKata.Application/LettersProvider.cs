using System.Text;
using DiamondKata.Abstraction;
using Serilog;

namespace DiamondKata.Application;

public class LettersProvider : ILettersProvider
{
    private readonly ILogger _logger;

    public LettersProvider(ILogger logger)
    {
        _logger = logger;
    }

    public int ConvertCharToDigit(char character)
    {
        _logger.Information($"Converting letter: '{character}' to number");
        return character - Constants.LastLetterAsciiNumber;
    }

    public char[] CreateLetters()
    {
        _logger.Information("Creating letters");
        return new[]
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
            'V', 'W', 'X', 'Y', 'Z'
        };
    }

    public void AddEmptySpace(ref StringBuilder? value)
    {
        value ??= new StringBuilder();
        _logger.Information($"Adding empty space for value: '{value}'");
        value.Append(' ');
    }

    public void AddLetter(ref StringBuilder? value, char letter)
    {
        value ??= new StringBuilder();
        _logger.Information($"Adding letter '{letter}' for value: {value}");
        value.Append(letter);
    }
}
