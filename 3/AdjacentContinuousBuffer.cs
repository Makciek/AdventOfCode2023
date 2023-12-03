namespace _3;

// The idea is as allows:
// Ex input:
// 467..114..
// ...*......
// 
// I insert first nad last lines as a lines full of '.'
// So the input becomes:
// ..........
// 467..114..
// ...*......
// ..........
//
// Then I'm consuming the input line by line where line is in fact 3 lines, the [1] is the one with part number
// and the [0] and [2] are the ones with adjacent symbols(as well as the char before and after the part number)
// so the input becomes:
// ..........467..114..
// 467..114.....*......
// ...*................
public class AdjacentContinuousBuffer
{
    private char[][] schema;
    private int colCount;
    private int linesCount;

    private int currentLineIndex = 0;
    private int currentColIndex = 0;

    public AdjacentContinuousBuffer(char[][] originalInput)
    {
        this.schema = new char[originalInput.Length + 2][];
        this.schema[0] = Enumerable.Repeat('.', originalInput[0].Length).ToArray();
        originalInput.CopyTo(this.schema, 1);
        this.schema[^1] = Enumerable.Repeat('.', originalInput[0].Length).ToArray();

        this.colCount = this.schema[0].Length;
        this.linesCount = this.schema.Length;
    }

    public char[]? GetNextCol()
    {
        if (this.currentLineIndex + 2 == this.linesCount)
        {
            return null;
        }

        var nextCol = new char[]
        {
            schema[this.currentLineIndex][this.currentColIndex],
            schema[this.currentLineIndex + 1][this.currentColIndex], // PartNumber
            schema[this.currentLineIndex + 2][this.currentColIndex],
        };

        this.currentColIndex++;
        if (this.currentColIndex >= this.colCount)
        {
            this.currentLineIndex++;
            this.currentColIndex = 0;
        }

        return nextCol;
    }

    public List<char[]> ReadAll()
    {
        var result = new List<char[]>();

        while (true)
        {
            var nextCol = this.GetNextCol();
            if (nextCol == null)
            {
                return result;
            }

            result.Add(nextCol);
        }
    }
}