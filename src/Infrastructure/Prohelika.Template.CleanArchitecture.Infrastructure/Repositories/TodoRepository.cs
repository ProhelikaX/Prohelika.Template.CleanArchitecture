using Microsoft.EntityFrameworkCore;
using Prohelika.Template.CleanArchitecture.Domain.Entities;
using Prohelika.Template.CleanArchitecture.Domain.Repositories;
using Prohelika.Template.CleanArchitecture.Infrastructure.Data;

namespace Prohelika.Template.CleanArchitecture.Infrastructure.Repositories;

public class TodoRepository(ApplicationDbContext context) : GenericRepository<Todo, Guid>(context), ITodoRepository
{
    public async Task<IReadOnlyList<Todo>> GetAllDoneAsync()
    {
        return await DbSet.Where(x => x.Completed).ToListAsync();
    }
}