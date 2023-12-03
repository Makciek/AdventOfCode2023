namespace _3_Tests;

using _3;

public class AdjacentContinuousBufferTests
{
    [Fact]
    public void GetNextCol_Returns_Expected_Result()
    {
        var input = new char[][]
        {
            new char[] { '4', '6', '7', '.', '.', '1', '1', '4', '.', '.' },
            new char[] { '.', '.', '.', '*', '.', '.', '.', '.', '.', '.' }
        };

        var buffer = new AdjacentContinuousBuffer(input);

        var result = buffer.GetNextCol();
        Assert.Equal('.', result[0]);
        Assert.Equal('4', result[1]);
        Assert.Equal('.', result[2]);
        
        result = buffer.GetNextCol();
        Assert.Equal('.', result[0]);
        Assert.Equal('6', result[1]);
        Assert.Equal('.', result[2]);
        
        result = buffer.GetNextCol();
        Assert.Equal('.', result[0]);
        Assert.Equal('7', result[1]);
        Assert.Equal('.', result[2]);
        
        result = buffer.GetNextCol();
        Assert.Equal('.', result[0]);
        Assert.Equal('.', result[1]);
        Assert.Equal('*', result[2]);
        
        result = buffer.GetNextCol();
        Assert.Equal('.', result[0]);
        Assert.Equal('.', result[1]);
        Assert.Equal('.', result[2]);
        
        result = buffer.GetNextCol();
        Assert.Equal('.', result[0]);
        Assert.Equal('1', result[1]);
        Assert.Equal('.', result[2]);
        
        result = buffer.GetNextCol();
        Assert.Equal('.', result[0]);
        Assert.Equal('1', result[1]);
        Assert.Equal('.', result[2]);
        
        result = buffer.GetNextCol();
        Assert.Equal('.', result[0]);
        Assert.Equal('4', result[1]);
        Assert.Equal('.', result[2]);
        
        result = buffer.GetNextCol();
        Assert.Equal('.', result[0]);
        Assert.Equal('.', result[1]);
        Assert.Equal('.', result[2]);
        
        result = buffer.GetNextCol();
        Assert.Equal('.', result[0]);
        Assert.Equal('.', result[1]);
        Assert.Equal('.', result[2]);
        
        result = buffer.GetNextCol();
        Assert.Equal('4', result[0]);
        Assert.Equal('.', result[1]);
        Assert.Equal('.', result[2]);
        
        result = buffer.GetNextCol();
        Assert.Equal('6', result[0]);
        Assert.Equal('.', result[1]);
        Assert.Equal('.', result[2]);
        
        result = buffer.GetNextCol();
        Assert.Equal('7', result[0]);
        Assert.Equal('.', result[1]);
        Assert.Equal('.', result[2]);
        
        result = buffer.GetNextCol();
        Assert.Equal('.', result[0]);
        Assert.Equal('*', result[1]);
        Assert.Equal('.', result[2]);

        for (int i = 0; i < 6; i++)
        {
            _ = buffer.GetNextCol();
        }

        Assert.Null(buffer.GetNextCol());
    }
}