#region Usings

using Records.Persons.Domain.Persons.ValueObjects;
using Records.Shared.Domain.Exceptions;
using Records.Shared.Domain.Models;
using Records.Shared.Domain.ValueObjects;
using Throw;

#endregion

namespace Records.Persons.Domain.Persons.Models;

/// <summary>
/// Represents a personal asset.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.OrderingRules",
    "SA1201:Elements should appear in the correct order",
    Justification = "Prefere to have the Create() and Load() inside the constructor region.")]
public sealed class PersonalAsset : Entity<int>
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonalAsset"/> class with the specified arguments.
    /// </summary>
    /// <param name="id">The PersonalAsset ID.</param>
    /// <param name="description">The description.</param>
    /// <param name="value">The monetary value.</param>
    private PersonalAsset(int id, PersonalAssetDescription? description, Money? value)
        : base(id)
    {
        EnsureMinimalRequiredData(description, value);

        Id = id;
        Description = description!; // Validated.
        Value = value!; // Validated.
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PersonalAsset"/> class with the specified arguments.
    /// (Instantiates the class, ensure the data and registers the events).
    /// </summary>
    /// <param name="description">The description.</param>
    /// <param name="value">The monetary value.</param>
    /// <returns>The created <see cref="PersonalAsset"/>.</returns>
    public static PersonalAsset Create(PersonalAssetDescription? description, Money? value)
    {
        PersonalAsset personalAsset = new (0, description, value);
        ////personalAsset.RegisterDomainEvent(new PersonalAssetCreatedEvent(null, personalAsset));

        return personalAsset;
    }

    /// <summary>
    /// Loads a new instance of the <see cref="PersonalAsset"/> class with the specified arguments.
    /// (Instantiates the class, ensure the data).
    /// </summary>
    /// <param name="id">The PersonalAsset ID.</param>
    /// <param name="description">The description.</param>
    /// <param name="value">The monetary value.</param>
    /// <returns>The created <see cref="PersonalAsset"/>.</returns>
    public static PersonalAsset Load(int id, PersonalAssetDescription? description, Money? value)
    {
        PersonalAsset personalAsset = new (id, description, value);

        return personalAsset;
    }

    #endregion

    #region Properties

    /// <summary>The description.</summary>
    public PersonalAssetDescription Description { get; private set; }

    /// <summary>The monetary value.</summary>
    public Money Value { get; private set; }

    #endregion

    #region Methods

    private void EnsureMinimalRequiredData(PersonalAssetDescription? description, Money? value)
    {
        try
        {
            description.ThrowIfNull();
            value.ThrowIfNull();
        }
        catch (Exception ex)
        {
            throw new DomainValidationException(ex.Message, ex);
        }
    }

    #endregion
}
