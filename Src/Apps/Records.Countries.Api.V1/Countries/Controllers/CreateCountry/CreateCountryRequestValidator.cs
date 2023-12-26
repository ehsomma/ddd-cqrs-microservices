#region Usings

using FluentValidation;
using Records.Countries.Api.V1.Countries.Validators;
using Records.Countries.Contracts.Countries;

#endregion

namespace Records.Countries.Api.V1.Countries.Controllers.CreateCountry;

/// <summary>
/// Represents the <see cref="CreateCountryRequest"/> validator.
/// </summary>
public class CreateCountryRequestValidator : AbstractValidator<CreateCountryRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCountryRequestValidator"/> class.
    /// </summary>
    public CreateCountryRequestValidator()
    {
        RuleFor(x => x.Country).NotNull().SetValidator(new CountryValidator());
    }
}
