using FluentValidation;
using Records.Countries.Dtos.Countries;

namespace Records.Countries.Api.V1.Countries.Validators;

/// <summary>
/// Represents the <see cref="Country"/> validator.
/// </summary>
public class CountryValidator : AbstractValidator<Country>
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryValidator"/> class.
    /// </summary>
    public CountryValidator()
    {
        RuleFor(x => x.IataCode).NotNull().NotEmpty();
        RuleFor(x => x.Name).NotNull().NotEmpty();
    }

    #endregion
}
