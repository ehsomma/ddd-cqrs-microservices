#region Usings

using Records.Shared.Domain.Exceptions;
using System.Reflection;

#endregion

namespace Records.Shared.Domain.Models;

/// <summary>
/// Represents an enumeration type.
/// </summary>
/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1000:Do not declare static members on generic types",
    Justification = "Checked.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1036:Override methods on comparable types",
    Justification = "<, > not used.")]
public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>, IComparable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    #region Declarations

    private static readonly Lazy<Dictionary<int, TEnum>> EnumerationsDictionary =
        new (() => GetAllEnumerationOptions().ToDictionary(item => item.Value));

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Enumeration{TEnum}"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="name">The name.</param>
    protected Enumeration(int value, string name)
    {
        Value = value;
        Name = name;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the enumeration values.
    /// </summary>
    /// <returns>The read-only collection of enumeration values.</returns>
    public static IReadOnlyCollection<TEnum> List => EnumerationsDictionary.Value.Values.ToList();

    /// <summary>
    /// Gets the value.
    /// </summary>
    public int Value { get; private set; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; private set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Compares two enumerators.
    /// </summary>
    /// <param name="left">The left one.</param>
    /// <param name="right">The right one.</param>
    /// <returns>True, if they are equals.</returns>
    public static bool operator ==(Enumeration<TEnum>? left, Enumeration<TEnum>? right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    /// <summary>
    /// Compares two enumerators.
    /// </summary>
    /// <param name="left">The left one.</param>
    /// <param name="right">The right one.</param>
    /// <returns>True, if they are not equals.</returns>
    public static bool operator !=(Enumeration<TEnum> left, Enumeration<TEnum> right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Creates an enumeration of the specified type based on the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The enumeration value.</param>
    /// <returns>The enumeration instance that matches the specified value.</returns>
    public static TEnum FromValue(int value)
    {
        if (!EnumerationsDictionary.Value.TryGetValue(value, out TEnum? enumeration))
        {
            throw new DomainValidationException($"Possible values for {typeof(TEnum).Name}: {string.Join(", ", List.Select(s => s.Name))}");
        }

        return enumeration;
    }

    /// <summary>
    /// Creates an enumeration of the specified type based on the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The enumeration name.</param>
    /// <returns>The enumeration instance that matches the specified <paramref name="name"/>.</returns>
    public static TEnum FromName(string? name)
    {
        TEnum? enumeration = List.SingleOrDefault(s => s.Name == name); // name.ToLowerInvariant()

        if (enumeration is null)
        {
            throw new DomainValidationException($"Possible values for {typeof(TEnum).Name}: {string.Join(", ", List.Select(s => s.Name))}");
        }

        return enumeration;
    }

    /// <summary>
    /// Checks if the there is an enumeration with the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>True if there is an enumeration with the specified <paramref name="value"/>, otherwise false.</returns>
    //// ReSharper disable once UnusedMember.Global
    public static bool ContainsValue(int value)
    {
        return EnumerationsDictionary.Value.ContainsKey(value);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Name;
    }

    /// <inheritdoc />
    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null)
        {
            return false;
        }

        return GetType() == other.GetType() && other.Value.Equals(Value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

#pragma warning disable CA1508 // Avoid dead conditional code
        if (!(obj is Enumeration<TEnum> otherValue))
        {
            return false;
        }
#pragma warning restore CA1508 // Avoid dead conditional code

        return GetType() == obj.GetType() && otherValue.Value.Equals(Value);
    }

    /// <inheritdoc />
    public int CompareTo(Enumeration<TEnum>? other)
    {
        return other is null ? 1 : Value.CompareTo(other.Value);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return Value.GetHashCode();
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Gets all of the defined enumeration options.
    /// </summary>
    /// <returns>The enumerable collection of enumerations.</returns>
    private static IEnumerable<TEnum> GetAllEnumerationOptions()
    {
        Type enumType = typeof(TEnum);

        IEnumerable<Type> enumerationTypes = Assembly
            .GetAssembly(enumType) !
            .GetTypes()
            .Where(type => enumType.IsAssignableFrom(type));

        List<TEnum> enumerations = new ();

        foreach (Type enumerationType in enumerationTypes)
        {
            List<TEnum> enumerationTypeOptions = GetFieldsOfType<TEnum>(enumerationType);

            enumerations.AddRange(enumerationTypeOptions);
        }

        return enumerations;
    }

    /// <summary>
    /// Gets the fields of the specified <paramref name="type"/>.
    /// </summary>
    /// <typeparam name="TFieldType">The field type.</typeparam>
    /// <param name="type">The type whose fields are being retrieved.</param>
    /// <returns>The fields of the specified <paramref name="type"/>.</returns>
    private static List<TFieldType> GetFieldsOfType<TFieldType>(Type type)
    {
        return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fieldInfo => type.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo => (TFieldType)fieldInfo.GetValue(null) !)
            .ToList();
    }

    #endregion
}
