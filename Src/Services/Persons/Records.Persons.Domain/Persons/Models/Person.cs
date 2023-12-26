#region Usings

using Records.Persons.Domain.Persons.Enumerators;
using Records.Persons.Domain.Persons.Events;
using Records.Persons.Domain.Persons.ValueObjects;
using Records.Shared.Domain.Exceptions;
using Records.Shared.Domain.Models;
using Records.Shared.Domain.ValueObjects;
using Throw;

#endregion

namespace Records.Persons.Domain.Persons.Models;

/// <summary>
/// Represents a Person.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.OrderingRules",
    "SA1201:Elements should appear in the correct order",
    Justification = "Prefere to have the Create() and Load() inside the constructor region.")]
public sealed class Person : AggregateRoot<Guid>, IEntityAuditable
{
    #region Declarations

    // NOTE: DDD Patterns comment: Using a private collection field, better for DDD Aggregate's
    // encapsulation so Items cannot be added from "outside the AggregateRoot" directly to the
    // collection, but only through the method .AddXxxxItem() which includes behaviour or
    // AggregateRoot constructor.
    private readonly IList<PersonalAsset> _personalAssets;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Person"/> class with the specified arguments.
    /// </summary>
    /// <param name="id">The Person ID.</param>
    /// <param name="address">The address.</param>
    /// <param name="fullName">The full name.</param>
    /// <param name="email">The email.</param>
    /// <param name="phone">The phone number.</param>
    /// <param name="gender">The gender.</param>
    /// <param name="personalAssets">The <see cref="PersonalAsset"/> list.</param>
    /// <param name="birthdate">The birthdate.</param>
    /// <param name="createdOnUtc">The date and time (UTC) the entity was created on.</param>
    /// <param name="updatedOnUtc">The date and time (UTC) the entity was modified on.</param>
    private Person(
        Guid id,
        Address? address,
        FullName? fullName,
        Email? email,
        PhoneNumber? phone,
        Gender? gender,
        IList<PersonalAsset>? personalAssets,
        DateTime? birthdate,
        DateTime? createdOnUtc,
        DateTime? updatedOnUtc)
        : base(id)
    {
        EnsureMinimalRequiredData(address, fullName, email, gender);

        Id = id;
        Address = address!; // Validated.
        FullName = fullName!; // Validated.
        Email = email!; // Validated.
        Phone = phone!; // Validated.
        Gender = gender!; // Validated.
        Birthdate = birthdate;
        _personalAssets = personalAssets ?? new List<PersonalAsset>();
        CreatedOnUtc = createdOnUtc;
        UpdatedOnUtc = updatedOnUtc;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Person"/> class with the specified arguments.
    /// (Instantiates the class, ensure the data and registers the events).
    /// </summary>
    /// <param name="id">The Person ID.</param>
    /// <param name="address">The address.</param>
    /// <param name="fullName">The full name.</param>
    /// <param name="email">The email.</param>
    /// <param name="phone">The phone number.</param>
    /// <param name="gender">The gender.</param>
    /// <param name="personalAssets">The <see cref="PersonalAsset"/> list.</param>
    /// <param name="birthdate">The birthdate.</param>
    /// <returns>The created <see cref="Person"/>.</returns>
    public static Person Create(
        Guid id,
        Address? address,
        FullName? fullName,
        Email? email,
        PhoneNumber? phone,
        Gender? gender,
        IList<PersonalAsset>? personalAssets,
        DateTime? birthdate)
    {
        DateTime utcNow = DateTime.UtcNow;

        Person person = new (
            id,
            address,
            fullName,
            email,
            phone,
            gender,
            personalAssets,
            birthdate,
            utcNow,
            utcNow);

        person.RegisterDomainEvent(new PersonCreatedEvent(person));

        return person;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Person"/> class with the specified arguments.
    /// (Instantiates the class, ensure the data).
    /// </summary>
    /// <param name="id">The Person ID.</param>
    /// <param name="address">The address.</param>
    /// <param name="fullName">The full name.</param>
    /// <param name="email">The email.</param>
    /// <param name="phone">The phone number.</param>
    /// <param name="gender">The gender.</param>
    /// <param name="personalAssets">The <see cref="PersonalAsset"/> list.</param>
    /// <param name="birthdate">The birthdate.</param>
    /// <param name="createdOnUtc">The date and time (UTC) the entity was created on.</param>
    /// <param name="updatedOnUtc">The date and time (UTC) the entity was modified on.</param>
    /// <returns>The created <see cref="Person"/>.</returns>
    public static Person Load(
        Guid id,
        Address? address,
        FullName? fullName,
        Email? email,
        PhoneNumber? phone,
        Gender? gender,
        IList<PersonalAsset>? personalAssets,
        DateTime? birthdate,
        DateTime? createdOnUtc = null,
        DateTime? updatedOnUtc = null)
    {
        Person person = new (id, address, fullName, email, phone, gender, personalAssets, birthdate, createdOnUtc, updatedOnUtc);

        return person;
    }

    #endregion

    #region Properties

    /// <summary>The address.</summary>
    public Address Address { get; private set; }

    /// <summary>The full name.</summary>
    public FullName FullName { get; private set; }

    /// <summary>The email.</summary>
    public Email Email { get; private set; }

    /// <summary>The phone number.</summary>
    public PhoneNumber? Phone { get; private set; }

    /// <summary>The gender.</summary>
    public Gender Gender { get; private set; }

    /// <summary>The birthdate.</summary>
    public DateTime? Birthdate { get; private set; }

    /// <summary>The <see cref="PersonalAsset"/> list.</summary>
    public IReadOnlyCollection<PersonalAsset> PersonalAssets => (IReadOnlyCollection<PersonalAsset>)_personalAssets;

    /// <inheritdoc />
    public DateTime? CreatedOnUtc { get; }

    /// <inheritdoc />
    public DateTime? UpdatedOnUtc { get; private set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Updates the current (this) <see cref="Person"/> with the specified data.
    /// </summary>
    /// <param name="updatedData">The data to update.</param>
    public void Update(Person updatedData)
    {
        FullName = updatedData.FullName;
        Email = updatedData.Email;
        Phone = updatedData.Phone;
        Gender = updatedData.Gender;
        Birthdate = updatedData.Birthdate;
        UpdatedOnUtc = DateTime.UtcNow;

        UpdateAddress(updatedData.Address);

        RegisterDomainEvent(new PersonUpdatedEvent(this));

        ////_personalAssets = ... // TODO: Update personalAssets separately by its own Update, not from here.
    }

    /// <summary>
    /// Delete the current (this) <see cref="Person"/>.
    /// </summary>
    public void Delete()
    {
        // Do some validation/enseres.
        // Update status if you need logic delete.

        RegisterDomainEvent(new PersonDeletedEvent(this));
    }

    /// <summary>
    /// Adds a new <see cref="Records.Persons.Domain.Persons.Models.PersonalAsset"/> to the <see cref="Person"/>.
    /// (Registers the events).
    /// </summary>
    /// <param name="personalAsset">The <see cref="Records.Persons.Domain.Persons.Models.PersonalAsset"/> to add.</param>
    public void AddPersonalAsset(PersonalAsset personalAsset)
    {
        _personalAssets.Add(personalAsset);

        RegisterDomainEvent(new PersonalAssetAddedEvent(this, personalAsset));
    }

    /// <summary>
    /// Loads a new <see cref="Records.Persons.Domain.Persons.Models.PersonalAsset"/> to the <see cref="Person"/>.
    /// </summary>
    /// <param name="personalAsset">The <see cref="Records.Persons.Domain.Persons.Models.PersonalAsset"/> to add.</param>
    public void LoadPersonalAsset(PersonalAsset personalAsset)
    {
        _personalAssets.Add(personalAsset);
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Ensures the minimum required data are valid.
    /// </summary>
    /// <param name="address">The address.</param>
    /// <param name="fullName">The full name.</param>
    /// <param name="email">The email.</param>
    /// <param name="gender">The gender.</param>
    /// <exception cref="DomainValidationException">If some argument is invalid.</exception>
    private void EnsureMinimalRequiredData(Address? address, FullName? fullName, Email? email, Gender? gender)
    {
        try
        {
            address.ThrowIfNull();
            fullName.ThrowIfNull();
            email.ThrowIfNull();
            ////phone.ThrowIfNull(); // Allow null.
            gender.ThrowIfNull();
            ////birthdate.ThrowIfNull(); // Allow null.
        }
        catch (Exception ex)
        {
            throw new DomainValidationException(ex.Message, ex);
        }
    }

    private void UpdateAddress(Address updatedAddress)
    {
        Address = Address.Load(
            Address.Id, // Not update the ID.
            updatedAddress.StreetLine1,
            updatedAddress.StreetLine2,
            updatedAddress.City,
            updatedAddress.State,
            updatedAddress.Country,
            updatedAddress.LatLng);
    }

    #endregion
}
