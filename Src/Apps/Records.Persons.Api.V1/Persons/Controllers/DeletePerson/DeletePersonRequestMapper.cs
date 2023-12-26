#region Usings

using Mapster;
using Records.Persons.Application.Persons.Commands.DeletePerson;
using Records.Persons.Contracts.Persons;
using Records.Shared.Contracts.Abstractions.Mappers;

#endregion

namespace Records.Persons.Api.V1.Persons.Controllers.DeletePerson;

/// <summary>
/// Mapper to map the request to a command.
/// </summary>
public class DeletePersonRequestMapper : IRequestToCommandMapper<DeletePersonRequest, DeletePersonCommand>
{
    #region Public methods

    /// <summary>
    /// Maps the specified <paramref name="request"/> to a command.
    /// </summary>
    /// <param name="request">A <see cref="DeletePersonRequest"/> request.</param>
    /// <returns>A <see cref="DeletePersonCommand"/> command.</returns>
    public DeletePersonCommand FromRequestToCommand(DeletePersonRequest request)
    {
        DeletePersonCommand command = request.Adapt<DeletePersonCommand>();

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
