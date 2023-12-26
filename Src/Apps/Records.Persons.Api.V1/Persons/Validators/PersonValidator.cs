using FluentValidation;
using Records.Persons.Dtos.Persons;

namespace Records.Persons.Api.V1.Persons.Validators;

/// <summary>
/// Represents the <see cref="Person"/> validator.
/// </summary>
public class PersonValidator : AbstractValidator<Person>
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonValidator"/> class.
    /// </summary>
    public PersonValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
        RuleFor(x => x.FullName).NotNull().NotEmpty();
        RuleFor(x => x.Email).EmailAddress();

        ////RuleFor(x => x.Gender).NotNull().NotEmpty(); // TODO: Add enum and .IsInEnum().

        RuleFor(x => x.Address).SetValidator(new AddressValidator() !);
        RuleForEach(x => x.PersonalAssets).SetValidator(new PersonalAssetValidator());
    }

    #endregion
}
