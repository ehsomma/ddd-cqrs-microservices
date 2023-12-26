#region Usings

using FluentValidation;
using Records.Persons.Api.V1.Persons.Validators;
using Records.Persons.Contracts.Persons;

#endregion

namespace Records.Persons.Api.V1.Persons.Controllers.UpdatePerson;

/// <summary>
/// Represents the <see cref="UpdatePersonRequest"/> validator.
/// </summary>
public class UpdatePersonRequestValidator : AbstractValidator<UpdatePersonRequest>
{
    #region Public methods

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePersonRequestValidator"/> class.
    /// </summary>
    public UpdatePersonRequestValidator()
    {
        RuleFor(x => x.Person).NotNull().SetValidator(new PersonValidator());
    }

    #endregion
}
