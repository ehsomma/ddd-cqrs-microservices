#region Usings

using Mapster;
using Records.Countries.Application.Countries.Commands.CreateCountry;
using Records.Countries.Contracts.Countries;
using Records.Shared.Contracts.Abstractions.Mappers;

#endregion

namespace Records.Countries.Api.V1.Countries.Controllers.CreateCountry;

/// <summary>
/// Mapper to map the request to a command.
/// </summary>
public class CreateCountryRequestMapper : IRequestToCommandMapper<CreateCountryRequest, CreateCountryCommand>
{
    #region Public methods

    /// <summary>
    /// Maps the specified <paramref name="request"/> to a command.
    /// </summary>
    /// <param name="request">A <see cref="CreateCountryRequest"/> request.</param>
    /// <returns>A <see cref="CreateCountryCommand"/> command.</returns>
    public CreateCountryCommand FromRequestToCommand(CreateCountryRequest request)
    {
        CreateCountryCommand command = request.Adapt<CreateCountryCommand>();

        return command;
    }

    #endregion
}
