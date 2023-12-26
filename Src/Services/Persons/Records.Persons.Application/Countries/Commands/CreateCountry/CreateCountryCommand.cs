#region Usings

using Records.Shared.Application.Messaging;
using Dto = Records.Persons.Dtos.Countries; // Using aliases.

#endregion

namespace Records.Persons.Application.Countries.Commands.CreateCountry;

/// <inheritdoc cref="ICommand"/>
public sealed class CreateCountryCommand : ICommand
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCountryCommand"/> class.
    /// </summary>
    /// <param name="appKey">The appKey (just an example).</param>
    /// <param name="country">The <see cref="Dto.Country"/> object as content data.</param>
    public CreateCountryCommand(string appKey, Dto.Country country)
    {
        AppKey = appKey;
        Country = country;
    }

    #endregion

    #region Properties

    /// <summary>The <see cref="Dto.Country"/> object as content data.</summary>
    public Dto.Country Country { get; init; }

    /// <summary>The appKey (just an example).</summary>
    public string AppKey { get; init; }

    #endregion
}
