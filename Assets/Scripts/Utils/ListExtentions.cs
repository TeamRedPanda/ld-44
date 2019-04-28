using System.Collections.Generic;

public static class ListExtentions
{
    public static T RandomElement<T>(this IList<T> list)
    {
        if (list.Count == 0)
            throw new System.IndexOutOfRangeException();

        var index = UnityEngine.Random.Range(0, list.Count);
        return list[index];
    }

    public static T PopRandom<T>(this IList<T> list)
    {
        if (list.Count == 0)
            throw new System.IndexOutOfRangeException();

        var index = UnityEngine.Random.Range(0, list.Count);
        T item = list[index];
        list.RemoveAt(index);
        return item;
    }
}
