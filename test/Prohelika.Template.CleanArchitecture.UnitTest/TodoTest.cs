using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prohelika.Template.CleanArchitecture.Application.Common.Exceptions;
using Prohelika.Template.CleanArchitecture.Application.Features.Todos;
using Prohelika.Template.CleanArchitecture.Domain.Entities;
using Prohelika.Template.CleanArchitecture.Domain.UnitOfWorks;
using Prohelika.Template.CleanArchitecture.Infrastructure.Data;
using Prohelika.Template.CleanArchitecture.Infrastructure.UnitOfWorks;

namespace Prohelika.Template.CleanArchitecture.UnitTest;

public class TodoTest : IDisposable, IAsyncDisposable
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public TodoTest()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseInMemoryDatabase("Test");
        _context = new ApplicationDbContext(optionsBuilder.Options);
        _unitOfWork = new UnitOfWork(_context);

        _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new TodosMappingProfile())).CreateMapper();

        _context.Database.EnsureDeleted();

        _context.Todos.Add(new Todo
        {
            Title = "Test", CreatedBy = "testuser"
        });
        _context.Todos.Add(new Todo
        {
            Title = "Test2", CreatedBy = "testuser2"
        });

        _context.Todos.Add(new Todo
        {
            Title = "Test3", CreatedBy = "testuser3"
        });

        _context.SaveChanges();
    }

    [Fact]
    public async Task GetTodos_Returns_All_Todos()
    {
        var todos = await _unitOfWork.Todos.GetAllAsync();

        Assert.NotEmpty(todos);
    }

    [Fact]
    public async Task GetAllTodoHandler_Returns_All_Todos()
    {
        var handler = new TodoGetAllHandler(_unitOfWork, _mapper);

        var result = await handler.Handle(new TodoGetAll(), CancellationToken.None);

        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetTodoById_Throws_NotFoundException_With_NewGuid()
    {
        var handler = new TodoGetByIdHandler(_unitOfWork, _mapper);

        var result = handler.Handle(new TodoGetById(Guid.NewGuid()), CancellationToken.None);

        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }

    [Fact]
    public async Task Todo_Created_Successfully()
    {
        var handler = new TodoAddHandler(_unitOfWork, _mapper);

        var todo = new TodoCreateDto { Title = "Test" };
        var result = await handler.Handle(new TodoAdd("testuser", todo), CancellationToken.None);

        Assert.NotNull(result);

        Assert.Equal("Test", result.Title);

        Assert.Equal(4, await _unitOfWork.Todos.CountAsync());
    }

    [Fact]
    public async Task Todo_Updated_Successfully()
    {
        var handler = new TodoUpdateHandler(_unitOfWork, _mapper);

        var todo = (await _unitOfWork.Todos.GetAllAsync()).First();
        todo.Title = "Test";
        todo.Completed = true;

        var result = await handler.Handle(new TodoUpdate(todo.Id, _mapper.Map<TodoDto>(todo)), CancellationToken.None);

        Assert.NotNull(result);

        Assert.True(result.Completed);
    }

    [Fact]
    public async Task Todo_Deleted_Successfully()
    {
        var handler = new TodoDeleteHandler(_unitOfWork, _mapper);

        var todo = (await _unitOfWork.Todos.GetAllAsync())[0];

        await handler.Handle(new TodoDelete(todo.Id), CancellationToken.None);

        Assert.Equal(2, await _unitOfWork.Todos.CountAsync());
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _unitOfWork.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
        await _unitOfWork.DisposeAsync();
    }
}