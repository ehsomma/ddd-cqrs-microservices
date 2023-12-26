#region Usings

using Mapster;
using Records.Countries.Application.Countries.Commands.DeleteCountry;
using Records.Countries.Contracts.Countries;
using Records.Shared.Contracts.Abstractions.Mappers;

#endregion

namespace Records.Countries.Api.V1.Countries.Controllers.DeleteCountry;

/// <summary>
/// Mapper to map the request to a command.
/// </summary>
public class DeleteCountryRequestMapper : IRequestToCommandMapper<DeleteCountryRequest, DeleteCountryCommand>
{
    #region Public methods

    /// <summary>
    /// Maps the specified <paramref name="request"/> to a command.
    /// </summary>
    /// <param name="request">A <see cref="DeleteCountryRequest"/> request.</param>
    /// <returns>A <see cref="DeleteCountryCommand"/> command.</returns>
    public DeleteCountryCommand FromRequestToCommand(DeleteCountryRequest request)
    {
        DeleteCountryCommand command = request.Adapt<DeleteCountryCommand>();

        return command;
    }

    #endregion
}
