using AutoMapper;
using MediatR;
using Prohelika.Template.CleanArchitecture.Application.Common.Common;
using Prohelika.Template.CleanArchitecture.Application.Common.Exceptions;
using Prohelika.Template.CleanArchitecture.Domain.Entities;
using Prohelika.Template.CleanArchitecture.Domain.UnitOfWorks;

namespace Prohelika.Template.CleanArchitecture.Application.Features.Todos;

public record TodoAdd(string? UserId, TodoCreateDto Todo) : IRequest<TodoDto>;

public class TodoAddHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : BaseRequestHandler<TodoAdd, TodoDto>(unitOfWork, mapper)
{
    public override async Task<TodoDto> Handle(TodoAdd request, CancellationToken cancellationToken)
    {
        if (request.UserId == null)
        {
            throw new ForbiddenException();
        }

        var entity = Mapper.Map<Todo>(request.Todo);

        entity.CreatedBy = request.UserId;

        await UnitOfWork.Todos.AddAsync(entity, cancellationToken);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Mapper.Map<TodoDto>(entity);
    }
}