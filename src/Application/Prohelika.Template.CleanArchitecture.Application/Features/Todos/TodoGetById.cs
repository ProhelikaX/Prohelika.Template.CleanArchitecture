using AutoMapper;
using MediatR;
using Prohelika.Template.CleanArchitecture.Application.Common.Common;
using Prohelika.Template.CleanArchitecture.Application.Common.Exceptions;
using Prohelika.Template.CleanArchitecture.Domain.Entities;
using Prohelika.Template.CleanArchitecture.Domain.UnitOfWorks;

namespace Prohelika.Template.CleanArchitecture.Application.Features.Todos;

public record TodoGetById(Guid Id) : IRequest<TodoDto>;

public class TodoGetByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : BaseRequestHandler<TodoGetById, TodoDto>(unitOfWork, mapper)
{
    public override async Task<TodoDto> Handle(TodoGetById request, CancellationToken cancellationToken)
    {
        var entity =
            await UnitOfWork.Todos.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Todo), request.Id);
        }

        return Mapper.Map<TodoDto>(entity);
    }
}