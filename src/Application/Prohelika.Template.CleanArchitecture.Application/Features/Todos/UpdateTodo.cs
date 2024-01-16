using AutoMapper;
using MediatR;
using Prohelika.Template.CleanArchitecture.Application.Common.Common;
using Prohelika.Template.CleanArchitecture.Application.Common.Exceptions;
using Prohelika.Template.CleanArchitecture.Domain.UnitOfWorks;

namespace Prohelika.Template.CleanArchitecture.Application.Features.Todos;

public record UpdateTodo(Guid Id, TodoDto Todo) : IRequest<TodoDto>;

public class UpdateTodoHandler
    (IUnitOfWork unitOfWork, IMapper mapper) : BaseRequestHandler<UpdateTodo, TodoDto>(unitOfWork, mapper)
{
    public override async Task<TodoDto> Handle(UpdateTodo request, CancellationToken cancellationToken)
    {
        if (request.Todo.Id != request.Id)
        {
            throw new ForbiddenException();
        }

        var entity =
            await unitOfWork.Todos.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException();
        }

        mapper.Map(request.Todo, entity);
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        
       var  result = unitOfWork.Todos.Update(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<TodoDto>(result);
    }
}