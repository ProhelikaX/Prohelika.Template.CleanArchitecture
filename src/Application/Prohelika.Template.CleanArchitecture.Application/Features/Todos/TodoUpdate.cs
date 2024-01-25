using AutoMapper;
using MediatR;
using Prohelika.Template.CleanArchitecture.Application.Common.Common;
using Prohelika.Template.CleanArchitecture.Application.Common.Exceptions;
using Prohelika.Template.CleanArchitecture.Domain.UnitOfWorks;

namespace Prohelika.Template.CleanArchitecture.Application.Features.Todos;

public record TodoUpdate(Guid Id, TodoDto Todo) : IRequest<TodoDto>;

public class TodoUpdateHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : BaseRequestHandler<TodoUpdate, TodoDto>(unitOfWork, mapper)
{
   

    public override async Task<TodoDto> Handle(TodoUpdate request, CancellationToken cancellationToken)
    {
        if (request.Todo.Id != request.Id)
        {
            throw new ForbiddenException();
        }

        var entity =
            await UnitOfWork.Todos.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException();
        }

        Mapper.Map(request.Todo, entity);
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        
       var  result = UnitOfWork.Todos.Update(entity);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Mapper.Map<TodoDto>(result);
    }
}