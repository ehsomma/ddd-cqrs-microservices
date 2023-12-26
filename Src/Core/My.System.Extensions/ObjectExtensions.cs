using System.Text.Json;

#pragma warning disable IDE0130 // Namespace does not match folder structure
//// ReSharper disable once CheckNamespace
namespace System;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Extension method for object.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Converts the value of a type into a JSON string.
    /// </summary>
    /// <param name="obj">The object.           </param>
    /// <returns>A JSON string representation of the value.</returns>
    public static string ToJson(this object obj)
    {
        JsonSerializerOptions serializerOptions = new ()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        return JsonSerializer.Serialize(obj, serializerOptions);
    }
}
