#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
namespace System.Collections.Generic;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Extension method for IEnumerable.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// If the IEnumerable is null, returns an "Empty" IEnumerable so you can use ForEach without to
    /// check if it is null.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <param name="source">The IEnumerable.</param>
    /// <returns>The same IEnumerable or an empty IEnumerable if it is null.</returns>
    public static IEnumerable<T> NotNull<T>(this IEnumerable<T> source)
    {
        return source ?? Enumerable.Empty<T>();
    }
}
