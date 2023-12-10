using System.Text;
using DiamondKata.Abstraction;
using DiamondKata.Application;
using FluentAssertions;
using Moq;
using Serilog;
using Xunit;

namespace DiamondKata.UnitTests;

public class LettersProviderTests
{
    private readonly ILettersProvider _lettersProvider;
    private readonly Mock<ILogger> _loggerMock = new();

    public LettersProviderTests()
    {
        _lettersProvider = new LettersProvider(_loggerMock.Object);
    }
        
    [Fact]
    public void When_ConvertCharToDigitWithCorrectChar_Then_ItConvertsSuccessfully()
    {
        // Arrange
        var correctChar = 'A';
        
        // Act
        var result = _lettersProvider.ConvertCharToDigit(correctChar);
        
        // Assert
        result.Should().Be(0);
        _loggerMock.Verify(logger => logger.Information(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public void When_CreatingLetters_Then_ItCreatesSuccessfully()
    {
        // Act
        var result = _lettersProvider.CreateLetters();
        
        // Assert
        result.Should().HaveCount(26);
        _loggerMock.Verify(logger => logger.Information(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public void When_AddingLetter_Then_ItIsAddingSuccessfully()
    {
        // Arrange
        var builder = new StringBuilder();
        var letter = 'A';
        
        // Act
        _lettersProvider.AddLetter(ref builder, letter);
        
        // Assert
        builder.ToString().Should().Be("A");
        _loggerMock.Verify(logger => logger.Information(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public void When_AddingLetterToNull_Then_ItIsAddingSuccessfully()
    {
        // Arrange
        StringBuilder? builder = null;
        var letter = 'A';
        
        // Act
        _lettersProvider.AddLetter(ref builder, letter);
        
        // Assert
        builder.ToString().Should().Be("A");
        _loggerMock.Verify(logger => logger.Information(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public void When_AddingEmptySpace_Then_ItIsAddingSuccessfully()
    {
        // Arrange
        var builder = new StringBuilder();
        
        // Act
        _lettersProvider.AddEmptySpace(ref builder);
        
        // Assert
        builder.ToString().Should().Be(" ");
        _loggerMock.Verify(logger => logger.Information(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public void When_AddingEmptySpaceToNull_Then_ItIsAddingSuccessfully()
    {
        // Arrange
        StringBuilder? builder = null;
        
        // Act
        _lettersProvider.AddEmptySpace(ref builder);
        
        // Assert
        builder.ToString().Should().Be(" ");
        _loggerMock.Verify(logger => logger.Information(It.IsAny<string>()), Times.Once);
    }
}
