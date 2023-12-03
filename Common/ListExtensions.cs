namespace Common;

public static class ListExtensions
{
    public static List<T> AddIfNotNull<T>(this List<T> list, T? item)
    {
        if (item != null)
        {
            list.Add(item);
        }

        return list;
    }
}