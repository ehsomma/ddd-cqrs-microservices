using Records.Shared.Infra.Swagger.Schemas;

namespace Records.Persons.Dtos.Persons;

/// <summary>
/// Represents a Person.
/// </summary>
public class Person
{
    #region Properties

    /// <summary>The Person ID.</summary>
    public Guid Id { get; init; }

    /// <summary>The full name.</summary>
    [SwaggerSchemaExample("Doe, John")]
    public string? FullName { get; init; }

    /// <summary>The email.</summary>
    [SwaggerSchemaExample("johndoe@gmail.com")]
    public string? Email { get; init; }

    /// <summary>The phone number.</summary>
    [SwaggerSchemaExample("")]
    public string? Phone { get; init; }

    /// <summary>The gender ("Male, Female, Other").</summary>
    // TODO: Implement Gender enum.
    [SwaggerSchemaExample("Male")]
    public string? Gender { get; init; }

    /// <summary>The birthdate.</summary>
    public DateTime? Birthdate { get; init; }

    /// <summary>The <see cref="Records.Persons.Dtos.Persons.Address"/> of the person.</summary>
    public Address? Address { get; init; }

    /// <summary>The <see cref="PersonalAsset"/> list.</summary>
    public IList<PersonalAsset>? PersonalAssets { get; init; }

    #endregion
}
