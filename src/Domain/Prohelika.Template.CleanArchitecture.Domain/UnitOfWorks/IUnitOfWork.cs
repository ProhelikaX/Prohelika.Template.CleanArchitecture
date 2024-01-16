using Prohelika.Template.CleanArchitecture.Domain.Repositories;

namespace Prohelika.Template.CleanArchitecture.Domain.UnitOfWorks;

public interface IUnitOfWork: IDisposable, IAsyncDisposable
{
    ITodoRepository Todos { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}