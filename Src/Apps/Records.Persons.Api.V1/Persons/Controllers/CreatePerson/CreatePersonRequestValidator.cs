#region Usings

using FluentValidation;
using Records.Persons.Api.V1.Persons.Validators;
using Records.Persons.Contracts.Persons;

#endregion

namespace Records.Persons.Api.V1.Persons.Controllers.CreatePerson;

/// <summary>
/// Represents the <see cref="CreatePersonRequest"/> validator.
/// </summary>
public class CreatePersonRequestValidator : AbstractValidator<CreatePersonRequest>
{
    #region Public methods

    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePersonRequestValidator"/> class.
    /// </summary>
    public CreatePersonRequestValidator()
    {
        RuleFor(x => x.Person).NotNull().SetValidator(new PersonValidator());
    }

    #endregion
}
