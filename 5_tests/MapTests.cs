namespace _5_Tests;

using _5;
using Xunit;

public class MapTests
{
    [Theory]
    [InlineData("50 98 2", 1, 1)]
    [InlineData("50 98 2", 10, 10)]
    [InlineData("50 98 2", 98, 50)]
    [InlineData("50 98 2", 99, 51)]
    [InlineData("52 50 48", 53, 55)]
    [InlineData("52 50 48", 10, 10)]
    public void GetTargetValue_Returns_Expected_Value_For_Single_MapInput(string mapInput, int source, int expectedTarget)
    {
        var map = new Map(new[] { mapInput }, "x", "Y");

        var actualTarget = map.GetTargetValue(source);

        Assert.Equal(expectedTarget, actualTarget);
    }

    [Theory]
    [InlineData("50 98 2|52 50 48", 1, 1)]
    [InlineData("50 98 2|52 50 48", 79, 81)]
    [InlineData("50 98 2|52 50 48", 14, 14)]
    [InlineData("50 98 2|52 50 48", 55, 57)]
    [InlineData("50 98 2|52 50 48", 13, 13)]
    [InlineData("88 18 7|18 25 70", 81, 74)]
    public void GetTargetValue_Returns_Expected_Value_For_MultiLine_MapInput(string mapInput, int source, int expectedTarget)
    {
        var lines = mapInput.Split('|');
        var map = new Map(lines, "x", "Y");

        var actualTarget = map.GetTargetValue(source);

        Assert.Equal(expectedTarget, actualTarget);
    }
}