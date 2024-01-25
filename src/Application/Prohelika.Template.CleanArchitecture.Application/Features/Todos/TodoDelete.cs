using AutoMapper;
using MediatR;
using Prohelika.Template.CleanArchitecture.Application.Common.Common;
using Prohelika.Template.CleanArchitecture.Application.Common.Exceptions;
using Prohelika.Template.CleanArchitecture.Domain.UnitOfWorks;

namespace Prohelika.Template.CleanArchitecture.Application.Features.Todos;

public record TodoDelete(Guid Id) : IRequest;

public class TodoDeleteHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : BaseRequestHandler<TodoDelete>(unitOfWork, mapper)
{

    public override async Task Handle(TodoDelete request, CancellationToken cancellationToken)
    {
        var entity =
            await UnitOfWork.Todos.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException();
        }

        UnitOfWork.Todos.Delete(entity);

        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}