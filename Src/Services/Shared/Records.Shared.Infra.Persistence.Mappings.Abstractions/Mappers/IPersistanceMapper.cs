namespace Records.Shared.Infra.Persistence.Mappings.Abstractions.Mappers;

/// <summary>
/// Defines a mapper to map a TDomain to a TDataModel and vice versa.
/// </summary>
/// <typeparam name="TDomain">The type of the domain model object.</typeparam>
/// <typeparam name="TDataModel">The type of the data model object.</typeparam>
public interface IPersistanceMapper<TDomain, TDataModel>
{
    #region Public methods

    /// <summary>
    /// Maps the specified domain model to a data model.
    /// </summary>
    /// <param name="domainModel">The domain model to map from.</param>
    /// <returns>A data model.</returns>
    public TDataModel FromDomainToDataModel(TDomain domainModel);

    // Debe permitir recibir y devolver null, ya que del repo puede venir null y debe ser
    // atajado de un service de dominio para lanzar una excepción de dominio.

    /// <summary>
    /// Maps the specified data model to a domain model.
    /// </summary>
    /// <param name="dataModel">The data model to map from.</param>
    /// <remarks>
    /// It must allow to receive null, since it could come null from the repository/database and it
    /// must be catched in a domain service and there it should throw a domain exception.
    /// </remarks>
    /// <returns>A domain model.</returns>
    public TDomain? FromDataModelToDomain(TDataModel? dataModel);

    #endregion
}
