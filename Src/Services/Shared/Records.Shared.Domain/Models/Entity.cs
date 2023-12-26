namespace Records.Shared.Domain.Models;

/// <summary>
/// Represents the Entity base class.
/// </summary>
/// <typeparam name="TId">The type of the identifier.</typeparam>
public abstract class Entity<TId> : IEquatable<Entity<TId>>
{
    #region Declarations

    private int? _requestedHashCode;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Entity{TId}"/> class.
    /// </summary>
    /// <param name="id">The Entity identification.</param>
    protected Entity(TId id)
    {
        Id = id;
    }

    #endregion

    #region Properties

    /// <summary>The <see cref="Entity{TId}"/> Id.</summary>
    public TId Id { get; init; }

    #endregion

    #region Operators overload

    /// <summary>
    /// Implicit operator.
    /// </summary>
    /// <param name="left">The left object.</param>
    /// <param name="right">The right object.</param>
    /// <returns>True, if they are equals.</returns>
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    /// <summary>
    /// Implicit operator.
    /// </summary>
    /// <param name="left">The left object.</param>
    /// <param name="right">The right object.</param>
    /// <returns>True, if they are not equals.</returns>
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
    {
        return !(left == right);
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public bool Equals(Entity<TId>? other)
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals(this, other) || Equals(other.Id, Id);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        if (obj is not Entity<TId> other)
        {
            return false;
        }

        return Equals(other.Id, Id);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        //// ReSharper disable NonReadonlyMemberInGetHashCode
        if (!_requestedHashCode.HasValue)
        {
            _requestedHashCode = Id!.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)
        }

        return _requestedHashCode.Value;
        //// ReSharper restore NonReadonlyMemberInGetHashCode
    }

    #endregion
}
