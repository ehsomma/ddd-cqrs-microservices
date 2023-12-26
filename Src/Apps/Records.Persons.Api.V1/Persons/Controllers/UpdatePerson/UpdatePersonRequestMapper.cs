#region Usings

using Mapster;
using Records.Persons.Application.Persons.Commands.UpdatePerson;
using Records.Persons.Contracts.Persons;
using Records.Shared.Contracts.Abstractions.Mappers;

#endregion

namespace Records.Persons.Api.V1.Persons.Controllers.UpdatePerson;

/// <summary>
/// Mapper to map the request to a command.
/// </summary>
public class UpdatePersonRequestMapper : IRequestToCommandMapper<UpdatePersonRequest, UpdatePersonCommand>
{
    #region Public methods

    /// <summary>
    /// Maps the specified <paramref name="request"/> to a command.
    /// </summary>
    /// <param name="request">A <see cref="UpdatePersonRequest"/> request.</param>
    /// <returns>A <see cref="UpdatePersonCommand"/> command.</returns>
    public UpdatePersonCommand FromRequestToCommand(UpdatePersonRequest request)
    {
        UpdatePersonCommand command = request.Adapt<UpdatePersonCommand>();

        /*
        UpdatePersonCommand command = new UpdatePersonCommand()
        {
            Id = UpdatePersonRequest.Id,
            Name = UpdatePersonRequest.Name,
            Position = UpdatePersonRequest.Position
        };
        */

        return command;
    }

    #endregion
}
