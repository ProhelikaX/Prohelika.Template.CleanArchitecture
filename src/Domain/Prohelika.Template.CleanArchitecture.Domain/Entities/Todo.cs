namespace Prohelika.Template.CleanArchitecture.Domain.Entities;

public class Todo: AuditableEntity<Guid, string>
{
    public string Title { get; set; }
    public bool Completed { get; set; }
    
    void MarkDone() => Completed = true;
}