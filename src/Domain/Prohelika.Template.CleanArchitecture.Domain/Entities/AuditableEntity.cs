namespace Prohelika.Template.CleanArchitecture.Domain.Entities;

public abstract class AuditableEntity<TId> : BaseEntity<TId>
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
}

public abstract class AuditableEntity<TId, TUser> : AuditableEntity<TId>
{
    public TUser CreatedBy { get; set; }
    public TUser? UpdatedBy { get; set; }
    public TUser? DeletedBy { get; set; }
}