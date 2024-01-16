using Microsoft.EntityFrameworkCore;
using Prohelika.Template.CleanArchitecture.Domain.Entities;

namespace Prohelika.Template.CleanArchitecture.Infrastructure.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
        
    }
    
    public DbSet<Todo> Todos => Set<Todo>();
}