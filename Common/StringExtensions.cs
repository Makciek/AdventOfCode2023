namespace Common;

public static class StringExtensions
{
    public static int? ToInt(this string? strInt)
    {
        if (strInt is null)
            return null;

        return Convert.ToInt32(strInt);
    }
}