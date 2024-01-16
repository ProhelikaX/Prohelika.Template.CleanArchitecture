using AutoMapper;
using MediatR;
using Prohelika.Template.CleanArchitecture.Application.Common.Common;
using Prohelika.Template.CleanArchitecture.Application.Common.Exceptions;
using Prohelika.Template.CleanArchitecture.Domain.Entities;
using Prohelika.Template.CleanArchitecture.Domain.UnitOfWorks;

namespace Prohelika.Template.CleanArchitecture.Application.Features.Todos;

public record GetTodoById(Guid Id) : IRequest<TodoDto>;

public class GetTodoByIdHandler
    (IUnitOfWork unitOfWork, IMapper mapper) : BaseRequestHandler<GetTodoById, TodoDto>(unitOfWork, mapper)
{
    public override async Task<TodoDto> Handle(GetTodoById request, CancellationToken cancellationToken)
    {
        var entity =
            await unitOfWork.Todos.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Todo), request.Id);
        }

        return mapper.Map<TodoDto>(entity);
    }
}