#region Usings

using Records.Shared.Domain.Exceptions;
using Records.Shared.Domain.Models;
using Throw;

#endregion

namespace Records.Shared.Domain.ValueObjects;

/// <summary>
/// A value object base class for strings value objects (non nullable).
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.OrderingRules",
    "SA1201:Elements should appear in the correct order",
    Justification = "Prefere to have the Build() inside the constructor region.")]
public abstract class StringValueObject : ValueObject
{
    #region Declarations

    /// <summary>Regex always valid (default).</summary>
    private const string RegexValid = ".*";

    private readonly int _maxLength;

    private readonly int _minLength;

    private readonly string _valueName;

    private readonly string _regex;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="StringValueObject"/> value object.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <param name="valueName">The name to show in the exception if its value is invalid.</param>
    /// <param name="maxLength">The maximum length to validate (send 0 to skip the validation).</param>
    /// <param name="minLength">The minimum length to validate (default 0).</param>
    /// <param name="regex">Regular expression to check (default ".*", anything).</param>
    protected StringValueObject(
        string? value,
        string valueName,
        int maxLength,
        int minLength = 0,
        string regex = RegexValid)
    {
        _valueName = valueName;
        _maxLength = maxLength;
        _minLength = minLength;
        _regex = regex;

        EnsureIsValid(value);

        Value = value!; // Null validated.
    }

    // NOTE: Static methods can not be abstracts in C#, so this method must replicate in each derived class.
    ////public static StringValueObject Build(string? value)
    ////{
    ////    StringValueObject valueObject = new StringValueObject(value);

    ////    return valueObject;
    ////}

    #endregion

    #region Properties

    /// <summary>The value.</summary>
    public string Value { get; }

    /// <summary>Gets a value indicating whether the ValueObject has a value other than null or empty.</summary>
    /// <remarks>Empty string is taken as no value.</remarks>
    public bool HasValue => !string.IsNullOrEmpty(Value);

    /// <summary>Gets a value indicating whether the ValueObject has not a value.</summary>
    public bool IsEmpty => string.IsNullOrEmpty(Value);

    #endregion

    #region Methods

    /// <summary>
    /// Implicit operator that returns its value.
    /// </summary>
    /// <param name="valueObject">The value object.</param>
    public static implicit operator string(StringValueObject valueObject)
    {
        return valueObject.Value!;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Value!;
    }

    /// <summary>
    /// Asserts that the arguments to create the <see cref="StringValueObject"/> value object are valid.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <exception cref="DomainValidationException">If some argument is invalid.</exception>
    protected void EnsureIsValid(string? value)
    {
        // INFO: To build regex, ask ChatGpt ;).
        // INFO: To test regex, go: https://regex101.com/.

        try
        {
            // NOTE: Null value not allowed. For null strings value objects use StringValueObjectNullable.
            value.ThrowIfNull(paramName: _valueName)
                .IfShorterThan(_minLength)
                .IfLongerThan(_maxLength)
                .IfNotMatches(_regex);
        }
        catch (Exception ex)
        {
            throw new DomainValidationException(ex.Message, ex);
        }
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value!; // Null validated.
    }

    #endregion
}
