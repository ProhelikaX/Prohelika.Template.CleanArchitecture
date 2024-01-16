using AutoMapper;
using MediatR;
using Prohelika.Template.CleanArchitecture.Application.Common.Common;
using Prohelika.Template.CleanArchitecture.Application.Common.Exceptions;
using Prohelika.Template.CleanArchitecture.Domain.UnitOfWorks;

namespace Prohelika.Template.CleanArchitecture.Application.Features.Todos;

public record DeleteTodo(Guid Id) : IRequest;

public class DeleteTodoHandler(IUnitOfWork unitOfWork, IMapper mapper) : BaseRequestHandler<DeleteTodo>(unitOfWork, mapper)
{
    public override async Task Handle(DeleteTodo request, CancellationToken cancellationToken)
    {
        var entity =
            await unitOfWork.Todos.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException();
        }

        unitOfWork.Todos.Delete(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}