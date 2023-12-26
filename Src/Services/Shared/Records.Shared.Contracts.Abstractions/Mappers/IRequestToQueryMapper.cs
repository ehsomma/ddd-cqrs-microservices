namespace Records.Shared.Contracts.Abstractions.Mappers;

/// <summary>
/// Represents the mapper interface to map a request to a query.
/// </summary>
/// <typeparam name="TRequest">The request contract type to map from.</typeparam>
/// <typeparam name="TQuery">The type of the query to map to.</typeparam>
public interface IRequestToQueryMapper<in TRequest, out TQuery>
{
    #region Public methods

    /// <summary>
    /// Maps the specified <paramref name="request"/> to a query.
    /// </summary>
    /// <param name="request">The request to map from.</param>
    /// <returns>A TQuery.</returns>
    public TQuery FromRequestToQuery(TRequest request);

    /// <summary>
    /// Maps the specified array to a query.
    /// </summary>
    /// <param name="myParams">The array to map from.</param>
    /// <returns>A TQuery.</returns>
    public TQuery FromQueryStringToQuery(params object[] myParams);

    #endregion
}
