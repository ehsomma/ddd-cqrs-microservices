#region Usings

using Records.Shared.Contracts.Abstractions;
using Records.Shared.Infra.Swagger.Schemas;

#endregion

namespace Records.Shared.Contracts;

/// <summary>
/// Represents a base request.
/// </summary>
public abstract class Request : IRequest
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Request"/> class.
    /// </summary>
    /// <param name="appKey">The key to identify from what application the request is coming from.</param>
    protected Request(string appKey)
    {
        AppKey = appKey;
    }

    #endregion

    #region Properties

    /// <inheritdoc />
    [SwaggerSchemaExample("<your-api-key-or-null>")]
    public string AppKey { get; init; }

    #endregion
}
