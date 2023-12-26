using FluentValidation;
using Records.Persons.Dtos.Persons;

namespace Records.Persons.Api.V1.Persons.Validators;

/// <summary>
/// Represents the <see cref="PersonalAsset"/> validator.
/// </summary>
public class PersonalAssetValidator : AbstractValidator<PersonalAsset>
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonalAssetValidator"/> class.
    /// </summary>
    public PersonalAssetValidator()
    {
        RuleFor(x => x.Description).NotNull().NotEmpty();
        RuleFor(x => x.Value).GreaterThan(0);
    }

    #endregion
}
