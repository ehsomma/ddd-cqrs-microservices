#region Usings

using Records.Shared.Domain.Exceptions;
using Records.Shared.Domain.Models;
using Throw;

#endregion

namespace Records.Shared.Domain.ValueObjects;

/// <summary>
/// Represents the Money value object.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.OrderingRules",
    "SA1201:Elements should appear in the correct order",
    Justification = "Prefere to have the Build() inside the constructor region.")]
public sealed class Money : ValueObject
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Money"/> value object.
    /// </summary>
    /// <param name="amount">The amount of money.</param>
    private Money(decimal amount)
    {
        EnsureIsValid(amount);

        Value = amount;
    }

    /// <summary>
    /// Build a new <see cref="Money"/> instance based on the specified <paramref name="amount"/>.
    /// </summary>
    /// <param name="amount">The amount of money.</param>
    /// <returns>The value object.</returns>
    public static Money Build(decimal amount)
    {
        Money money = new (amount);

        return money;
    }

    #endregion

    #region Properties

    /// <summary>The value.</summary>
    public decimal Value { get; private set; } // = decimal.Zero;

    #endregion

    #region Methods

    /// <summary>
    /// Implicit operator that returns its value.
    /// </summary>
    /// <param name="money">The value object.</param>
    public static implicit operator decimal(Money money)
    {
        ArgumentNullException.ThrowIfNull(money);

        return money.Value;
    }

    /// <summary>
    /// Rounds the current value to the specified number of decimal places.
    /// </summary>
    /// <param name="decimals">Amount of decimals places (between 0 and 28 inclusive).</param>
    /// <returns>The same value object.</returns>
    public Money Round(int decimals)
    {
        Value = Math.Round(Value, decimals);

        return this;
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Asserts that the arguments to create the <see cref="Money"/> value object are valid.
    /// </summary>
    /// <param name="amount">The amount of money.</param>
    /// <exception cref="DomainValidationException">If some argument is invalid.</exception>
    private static void EnsureIsValid(decimal amount)
    {
        try
        {
            amount.ThrowIfNull(paramName: "Amount/Value (Money)").IfGreaterThan(decimal.MaxValue).IfLessThan(decimal.MinValue);
        }
        catch (Exception ex)
        {
            throw new DomainValidationException(ex.Message, ex);
        }
    }

    #endregion
}
