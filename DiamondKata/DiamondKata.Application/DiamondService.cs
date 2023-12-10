using System.Text;
using DiamondKata.Abstraction;

namespace DiamondKata.Application;

public class DiamondService : IDiamondService
{
    private readonly ILettersProvider _lettersProvider;

    public DiamondService(ILettersProvider lettersProvider)
    {
        _lettersProvider = lettersProvider;
    }
    
    public StringBuilder DrawDiamondBasedOnLetter(string letter)
    {
        var letters = _lettersProvider.CreateLetters();
        
        //array of strings
        var diamond = new StringBuilder[Constants.DiamondLength];
        //get the letter
        var userLetter = letter.ToUpperInvariant()[0];
        //search for letter number in the array letter
        var letterNumber = _lettersProvider.ConvertCharToDigit(userLetter);
        
        var result = new StringBuilder();
        
        //construct diamond 
        for (var i = 0; i <= letterNumber; i++)
        {
            //add initial spaces
            for (var j = 0; j < letterNumber - i; j++)
            {
                _lettersProvider.AddEmptySpace(ref diamond[i]);
            }
        
            //add letter (first time)
            _lettersProvider.AddLetter(ref diamond[i], letters[i]);
        
            //add space between letters
            if (letters[i] != 'A')
            {
                for (var j = 0; j < 2 * i - 1; j++)
                {
                    _lettersProvider.AddEmptySpace(ref diamond[i]);
                }
                //add letter (second time)
                _lettersProvider.AddLetter(ref diamond[i], letters[i]);
            }
            // Draw the first part of the diamond as it's composing.
            result.AppendLine(diamond[i].ToString());
        }

        for (var i = letterNumber - 1; i >= 0; i--)
        {
            // Draw the second part of the diamond
            // Writing the diamondArray in reverse order.
            result.AppendLine(diamond[i].ToString());
        }
        
        Console.WriteLine(result.ToString());

        return result;
    }
}
