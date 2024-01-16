using AutoMapper;
using MediatR;
using Prohelika.Template.CleanArchitecture.Application.Common.Common;
using Prohelika.Template.CleanArchitecture.Application.Common.Exceptions;
using Prohelika.Template.CleanArchitecture.Domain.Entities;
using Prohelika.Template.CleanArchitecture.Domain.UnitOfWorks;

namespace Prohelika.Template.CleanArchitecture.Application.Features.Todos;

public record AddTodo(string? UserId, TodoCreateDto Todo) : IRequest<TodoDto>;

public class AddTodoHandler(IUnitOfWork unitOfWork, IMapper mapper) : BaseRequestHandler<AddTodo, TodoDto>(
    unitOfWork, mapper)
{
    public override async Task<TodoDto> Handle(AddTodo request, CancellationToken cancellationToken)
    {
        if (request.UserId == null)
        {
            throw new ForbiddenException();
        }

        var entity = mapper.Map<Todo>(request.Todo);

        entity.CreatedBy = request.UserId;

        await unitOfWork.Todos.AddAsync(entity, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<TodoDto>(entity);
    }
}