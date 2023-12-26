namespace Records.Shared.Contracts.Abstractions.Mappers;

/// <summary>
/// Represents the mapper interface to map a request to a command.
/// </summary>
/// <typeparam name="TRequest">The request contract type to map from.</typeparam>
/// <typeparam name="TCommand">The type of the command to map to.</typeparam>
public interface IRequestToCommandMapper<in TRequest, out TCommand>
{
    #region Public methods

    /// <summary>
    /// Maps the specified <paramref name="request"/> to a command.
    /// </summary>
    /// <param name="request">The request to map from.</param>
    /// <returns>A TCommand.</returns>
    public TCommand FromRequestToCommand(TRequest request);

    #endregion
}
