#region Usings

using Mapster;
using Records.Persons.Application.Persons.Commands.CreatePerson;
using Records.Persons.Contracts.Persons;
using Records.Shared.Contracts.Abstractions.Mappers;

#endregion

namespace Records.Persons.Api.V1.Persons.Controllers.CreatePerson;

/// <summary>
/// Mapper to map the request to a command.
/// </summary>
public class CreatePersonRequestMapper : IRequestToCommandMapper<CreatePersonRequest, CreatePersonCommand>
{
    #region Public methods

    /// <summary>
    /// Maps the specified <paramref name="request"/> to a command.
    /// </summary>
    /// <param name="request">A <see cref="CreatePersonRequest"/> request.</param>
    /// <returns>A <see cref="CreatePersonCommand"/> command.</returns>
    public CreatePersonCommand FromRequestToCommand(CreatePersonRequest request)
    {
        CreatePersonCommand command = request.Adapt<CreatePersonCommand>();

        return command;
    }

    #endregion
}
