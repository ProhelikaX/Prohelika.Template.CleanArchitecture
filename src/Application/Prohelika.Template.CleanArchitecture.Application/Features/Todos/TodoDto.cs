namespace Prohelika.Template.CleanArchitecture.Application.Features.Todos;

public record TodoCreateDto
{
    public string Title { get; set; }
}

public record TodoDto : TodoCreateDto
{
    public Guid Id { get; set; }
    public bool Completed { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}