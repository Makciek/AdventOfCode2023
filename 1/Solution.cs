namespace _1;

using Common;

public class Solution : SolutionBase
{
    private static readonly Dictionary<string, string> stringDigitsOrg = new()
    {
        {"one", "1"},
        {"two", "2"},
        {"three", "3"},
        {"four", "4"},
        {"five", "5"},
        {"six", "6"},
        {"seven", "7"},
        {"eight", "8"},
        {"nine", "9"},
    };

    private static readonly Dictionary<string, string> stringDigitsReversedKey = stringDigitsOrg.ToDictionary(k => new string(k.Key.Reverse().ToArray()), v => v.Value);

    public Solution(string fileName) : base(fileName)
    {
    }

    protected override string GetResult()
    {
        var result = this.Lines.Sum(this.GetDecodedValueForLine);
        return result.ToString();
    }

    private int GetDecodedValueForLine(string line)
    {
        var replacedCTestDigits = ReplaceTextDigitsToSingleChars(line, stringDigitsOrg);
        var first = replacedCTestDigits[0];

        var reversedLine = line.Reverse();
        var reversedReplacedTextDigits = ReplaceTextDigitsToSingleChars(reversedLine, stringDigitsReversedKey).Reverse();
        var last = reversedReplacedTextDigits[^1];

        var numberString = first.ToString() + last.ToString();
        var number = Convert.ToInt32(numberString);
        return number;
    }

    private string ReplaceTextDigitsToSingleChars(ReadOnlySpan<char> lineSpan, Dictionary<string, string> stringDigits)
    {
        var firstLetters = stringDigits.Keys.Select(k => k[0]).Distinct().ToList();

        var maxLength = lineSpan.Length;

        var newString = string.Empty;
        for (int i = 0; i < lineSpan.Length; i++)
        {
            var c = lineSpan[i];
            if (char.IsDigit(c))
            {
                newString += c;
                continue;
            }

            if (firstLetters.Contains(c))
            {
                var possibleMatches = stringDigits.Keys.Where(k => k.StartsWith(c)).ToList();
                var maxMatchWindows = possibleMatches.Where(k =>
                {
                    var maxIndex = i + k.Length;
                    return maxIndex <= maxLength;
                })
                    .OrderByDescending(w => w.Length)
                    .ToList();

                if (!maxMatchWindows.Any())
                {
                    continue;
                }

                foreach (var maxMatchWindow in maxMatchWindows)
                {
                    var digit = lineSpan.Slice(i, maxMatchWindow.Length).ToString();
                    if (digit == maxMatchWindow)
                    {
                        newString += stringDigits[digit];
                        i += maxMatchWindow.Length - 1;
                        break;
                    }
                }
            }
        }

        return newString;
    }
}