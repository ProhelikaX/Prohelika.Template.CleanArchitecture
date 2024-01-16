using Prohelika.Template.CleanArchitecture.Domain.Entities;

namespace Prohelika.Template.CleanArchitecture.Domain.Repositories;

public interface ITodoRepository: IGenericRepository<Todo, Guid>
{
    Task<IReadOnlyList<Todo>> GetAllDoneAsync();
}