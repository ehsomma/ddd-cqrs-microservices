using FluentValidation;
using Records.Persons.Dtos.Persons;

namespace Records.Persons.Api.V1.Persons.Validators;

/// <summary>
/// Represents the <see cref="Address"/> validator.
/// </summary>
public class AddressValidator : AbstractValidator<Address>
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="AddressValidator"/> class.
    /// </summary>
    public AddressValidator()
    {
        RuleFor(x => x.StreetLine1).NotNull().NotEmpty();

        // TODO: Implement the remaining validations.
    }

    #endregion
}
