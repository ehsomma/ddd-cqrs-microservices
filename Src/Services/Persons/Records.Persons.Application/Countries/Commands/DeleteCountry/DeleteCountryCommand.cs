using Records.Shared.Application.Messaging;

namespace Records.Persons.Application.Countries.Commands.DeleteCountry;

/// <inheritdoc cref="ICommand"/>
public sealed class DeleteCountryCommand : ICommand
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCountryCommand"/> class.
    /// </summary>
    /// <param name="appKey">The key to identify from what application the request is coming from.</param>
    /// <param name="iataCode">Country IATA code.</param>
    public DeleteCountryCommand(string appKey, string iataCode)
    {
        AppKey = appKey;
        IataCode = iataCode;
    }

    #endregion

    #region Properties

    /// <summary>Country IATA code.</summary>
    public string IataCode { get; }

    /// <summary>The key to identify from what application the request is coming from.</summary>
    public string AppKey { get; }

    #endregion
}
