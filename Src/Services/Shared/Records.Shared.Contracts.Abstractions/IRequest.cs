namespace Records.Shared.Contracts.Abstractions;

/// <summary>
/// Defines a base request.
/// </summary>
public interface IRequest
{
    /// <summary>The key to identify from what application the request is coming from.</summary>
    public string AppKey { get; init; }
}
