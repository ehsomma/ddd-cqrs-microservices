using Records.Shared.Application.Messaging;

namespace Records.Countries.Application.Countries.Commands.DeleteCountry;

/// <inheritdoc cref="ICommand"/>
public sealed class DeleteCountryCommand : ICommand
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCountryCommand"/> class.
    /// </summary>
    /// <param name="appKey">The appKey (just an example).</param>
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

    /// <summary>The appKey (just an example).</summary>
    public string AppKey { get; }

    #endregion
}
