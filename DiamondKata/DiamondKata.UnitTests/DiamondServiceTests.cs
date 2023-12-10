using System.Text;
using DiamondKata.Abstraction;
using DiamondKata.Application;
using FluentAssertions;
using Moq;
using Xunit;

namespace DiamondKata.UnitTests;

public class DiamondServiceTests
{
    private readonly IDiamondService _diamondService;
    private readonly Mock<ILettersProvider> _lettersProviderMock = new();

    delegate void AddEmptySpaceCallback(ref StringBuilder? builder);

    delegate void AddLetterCallback(ref StringBuilder? builder, char letter);

    public static IEnumerable<object[]> GetLetters()
    {
        return new List<object[]>
        {
            new object[] { 'A' },
            new object[] { 'B' },
            new object[] { 'C' },
            new object[] { 'D' },
            new object[] { 'E' },
            new object[] { 'F' },
            new object[] { 'G' },
            new object[] { 'H' },
            new object[] { 'I' },
            new object[] { 'J' },
            new object[] { 'K' },
            new object[] { 'L' },
            new object[] { 'M' },
            new object[] { 'N' },
            new object[] { 'O' },
            new object[] { 'P' },
            new object[] { 'R' },
            new object[] { 'S' },
            new object[] { 'T' },
            new object[] { 'Q' },
            new object[] { 'U' },
            new object[] { 'W' },
            new object[] { 'X' },
            new object[] { 'Y' },
            new object[] { 'Z' }
        };
    }

    private readonly char[] _letters =
    {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
        'V', 'W', 'X', 'Y', 'Z'
    };

    public DiamondServiceTests()
    {
        _diamondService = new DiamondService(_lettersProviderMock.Object);
    }

    [Theory]
    [MemberData(nameof(GetLetters), MemberType = typeof(DiamondServiceTests))]
    public void When_CreatingDiamondForCorrectLetter_ItCreatesSuccessfully(char letter)
    {
        // Arrange
        var expectedResult = new StringBuilder();
        var expectedBuilders = new StringBuilder[Constants.DiamondLength];

        var letterNumber = letter - Constants.LastLetterAsciiNumber;
        _lettersProviderMock.Setup(lpm => lpm.CreateLetters()).Returns(_letters);
        _lettersProviderMock.Setup(lpm => lpm.ConvertCharToDigit(It.IsAny<char>())).Returns(letterNumber);
        _lettersProviderMock.Setup(lpm => lpm.AddEmptySpace(ref It.Ref<StringBuilder?>.IsAny))
            .Callback(new AddEmptySpaceCallback((ref StringBuilder? builder) =>
            {
                builder ??= new StringBuilder();
                builder.Append(' ');
                expectedResult.Append(' ');
            }));

        _lettersProviderMock.Setup(lpm => lpm.AddLetter(ref It.Ref<StringBuilder?>.IsAny, It.IsAny<char>()))
            .Callback(new AddLetterCallback((ref StringBuilder? builder, char c) =>
            {
                builder ??= new StringBuilder();
                builder.Append(c);
                if (expectedResult.ToString().Contains(c) || !expectedResult.ToString().Contains('A'))
                {
                    expectedResult.Append(c);
                    expectedResult.AppendLine();
                    expectedBuilders[c - Constants.LastLetterAsciiNumber] = builder;
                }
                else
                {
                    expectedResult.Append(c);
                }
            }));

        // Act
        var result = _diamondService.DrawDiamondBasedOnLetter(letter.ToString());

        // Assert
        for (var i = letterNumber - 1; i >= 0; i--)
        {
            expectedResult.AppendLine(expectedBuilders[i].ToString());
        }

        expectedResult.ToString().Should().Be(result.ToString());
    }

    [Fact]
    public void When_CreatingDiamondForWrongCharacter_ItReturnsEmptyResult()
    {
        // Arrange
        var result = new StringBuilder();

        var letter = "%";

        _lettersProviderMock.Setup(lpm => lpm.CreateLetters()).Returns(_letters);
        _lettersProviderMock.Setup(lpm => lpm.ConvertCharToDigit(It.IsAny<char>())).Returns(-1);
        _lettersProviderMock.Setup(lpm => lpm.AddEmptySpace(ref It.Ref<StringBuilder?>.IsAny))
            .Callback(new AddEmptySpaceCallback((ref StringBuilder? builder) =>
            {
                builder ??= new StringBuilder();
                builder.Append(' ');
                result.Append(' ');
            }));

        _lettersProviderMock.Setup(lpm => lpm.AddLetter(ref It.Ref<StringBuilder?>.IsAny, It.IsAny<char>()))
            .Callback(new AddLetterCallback((ref StringBuilder? builder, char c) =>
            {
                builder ??= new StringBuilder();
                builder.Append(c);
                result.Append(c);
            }));

        // Act
        _diamondService.DrawDiamondBasedOnLetter(letter);

        // Assert
        result.ToString().Should().Be(string.Empty);
    }

    [Fact]
    public void When_CreatingDiamondWithEmptyLetters_ItReturnsEmptyResult()
    {
        // Arrange
        var result = new StringBuilder();

        var letter = "%";

        _lettersProviderMock.Setup(lpm => lpm.CreateLetters()).Returns(Array.Empty<char>());
        _lettersProviderMock.Setup(lpm => lpm.ConvertCharToDigit(It.IsAny<char>())).Returns(0);
        _lettersProviderMock.Setup(lpm => lpm.AddEmptySpace(ref It.Ref<StringBuilder?>.IsAny))
            .Callback(new AddEmptySpaceCallback((ref StringBuilder? builder) =>
            {
                builder ??= new StringBuilder();
                builder.Append(' ');
                result.Append(' ');
            }));

        _lettersProviderMock.Setup(lpm => lpm.AddLetter(ref It.Ref<StringBuilder?>.IsAny, It.IsAny<char>()))
            .Callback(new AddLetterCallback((ref StringBuilder? builder, char c) =>
            {
                builder ??= new StringBuilder();
                builder.Append(c);
                result.Append(c);
            }));

        // Act & Assert
        Assert.Throws<IndexOutOfRangeException>(() => _diamondService.DrawDiamondBasedOnLetter(letter));
    }

    [Fact]
    public void When_CreatingDiamondWithAddEmptySpaceDoingNothing_ItFails()
    {
        // Arrange
        var letter = "A";

        _lettersProviderMock.Setup(lpm => lpm.CreateLetters()).Returns(_letters);
        _lettersProviderMock.Setup(lpm => lpm.ConvertCharToDigit(It.IsAny<char>())).Returns(0);
        _lettersProviderMock.Setup(lpm => lpm.AddEmptySpace(ref It.Ref<StringBuilder?>.IsAny));

        _lettersProviderMock.Setup(lpm => lpm.AddLetter(ref It.Ref<StringBuilder?>.IsAny, It.IsAny<char>()));

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => _diamondService.DrawDiamondBasedOnLetter(letter));
    }
}
