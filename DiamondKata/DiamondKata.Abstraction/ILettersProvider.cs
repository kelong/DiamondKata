using System.Text;

namespace DiamondKata.Abstraction;

public interface ILettersProvider
{
    int ConvertCharToDigit(char character);

    char[] CreateLetters();

    void AddEmptySpace(ref StringBuilder? value);
    
    void AddLetter(ref StringBuilder? value, char letter);
}
