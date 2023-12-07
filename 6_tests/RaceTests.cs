namespace _6_tests;

using _6;
using Microsoft.VisualBasic;
using Xunit;

public class RaceTests
{
    [Theory]
    [InlineData(7, 9, 4, 2, 3, 4, 5)]
    [InlineData(15, 40, 8)]
    [InlineData(30, 200, 9)]
    public void GetWiningDurations_Returns_Correct_Result(int duration, int recordDistance, int expectedResultCount, params int[] expectedResult)
    {
        var game = new Race()
        {
            RaceDuration = duration,
            RecordDistance = recordDistance
        };

        var result = game.GetWiningDurations();

        Assert.Equal(expectedResultCount, result.Length);

        if (expectedResult.Any())
        {
            for (int i = 0; i < expectedResult.Length; i++)
            {
                Assert.Equal(expectedResult[i], result[i]);
            }
        }
    }
}