using Prohelika.Template.CleanArchitecture.Domain.Repositories;
using Prohelika.Template.CleanArchitecture.Domain.UnitOfWorks;
using Prohelika.Template.CleanArchitecture.Infrastructure.Data;
using Prohelika.Template.CleanArchitecture.Infrastructure.Repositories;

namespace Prohelika.Template.CleanArchitecture.Infrastructure.UnitOfWorks;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public ITodoRepository Todos { get; } = new TodoRepository(context);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
    }
}