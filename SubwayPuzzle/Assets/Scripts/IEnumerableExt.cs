using System;
using System.Collections.Generic;

public static class IEnumerableExt
{
    /// <summary>
    /// Returns all consecutive pairs in the enumerable.
    /// </summary>
    public static IEnumerable<(T, T)> Pairs<T>(this IEnumerable<T> e)
    {
        var enumerator = e.GetEnumerator();

        if (!enumerator.MoveNext())
            yield break;

        var prev = enumerator.Current;
        while (enumerator.MoveNext())
        {
            var curr = enumerator.Current;
            yield return (prev, curr);
            prev = curr;
        }
    }
}
