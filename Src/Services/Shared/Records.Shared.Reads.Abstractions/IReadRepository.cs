namespace Records.Shared.Reads.Abstractions;

/// <summary>
/// Defines the operations to read from the Read (projection) database/repository.
/// </summary>
/// <typeparam name="TReadQuery">The type of the query.</typeparam>
/// <typeparam name="TReadResponse">The type of the response.</typeparam>
public interface IReadRepository<in TReadQuery, TReadResponse>
{
    #region Public methods

    /// <summary>
    /// Gets a TReadResponse from the Read (projection) database/repository.
    /// </summary>
    /// <param name="query">The data to use in the filter.</param>
    /// <param name="cancellationToken">The cancelation token.</param>
    /// <returns>A TReadResponse or null.</returns>
    public Task<TReadResponse> GetAsync(TReadQuery query, CancellationToken cancellationToken = default);

    #endregion
}

/// <summary>
/// Defines the operations to read from the Read (projection) database/repository (with out a query).
/// </summary>
/// <typeparam name="TReadResponse">The type of the response.</typeparam>
public interface IReadRepository<TReadResponse>
{
    #region Public methods

    /// <summary>
    /// Gets a TReadResponse from the Read (projection) database/repository.
    /// </summary>
    /// <param name="cancellationToken">The cancelation token.</param>
    /// <returns>A TReadResponse or null.</returns>
    public Task<TReadResponse?> GetAsync(CancellationToken cancellationToken = default);

    #endregion
}
