#region Usings

using FluentValidation.Results;
using My.Core.Exceptions;

#endregion

#pragma warning disable IDE0130 // Namespace does not match folder structure
//// ReSharper disable once CheckNamespace
namespace FluentValidation;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Extension methods for FluentValidations.
/// </summary>
public static class DefaultValidatorExtensions
{
    #region Public methods

    /// <summary>
    /// Performs validation and then throws an EntityValidationException if validation fails.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to validate.</typeparam>
    /// <param name="validator">The validator class.</param>
    /// <param name="entity">The entity to validate.</param>
    public static void ValidateAndThrowExeption<TEntity>(this IValidator<TEntity> validator, TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(validator);

        // Realiza la validación.
        ValidationResult validationResult = validator.Validate(entity);

        // Si la validación falla, lanza una excepción con la lista de errores.
        if (!validationResult.IsValid)
        {
            ////List<ValidationError> validationErrors = new List<ValidationError>();

            ////foreach (ValidationFailure validationFailure in validationResult.Errors)
            ////{
            ////    ValidationError validationError = new ValidationError(
            ////        validationFailure.PropertyName,
            ////        validationFailure.ErrorMessage,
            ////        validationFailure.AttemptedValue);

            ////    validationErrors.Add(validationError);
            ////}

            IReadOnlyCollection<ValidationError> errors = validationResult.Errors
                .Select(failure => new ValidationError(
                    failure.PropertyName,
                    failure.ErrorMessage,
                    failure.AttemptedValue))
                .ToList().AsReadOnly();

            throw new My.Core.Exceptions.ValidationException(errors);
        }
    }

    #endregion
}
