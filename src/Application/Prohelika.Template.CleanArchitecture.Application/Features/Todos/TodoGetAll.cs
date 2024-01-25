using AutoMapper;
using MediatR;
using Prohelika.Template.CleanArchitecture.Application.Common.Common;
using Prohelika.Template.CleanArchitecture.Domain.UnitOfWorks;

namespace Prohelika.Template.CleanArchitecture.Application.Features.Todos;

public record TodoGetAll : IRequest<IEnumerable<TodoDto>>;

public class TodoGetAllHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : BaseRequestHandler<TodoGetAll, IEnumerable<TodoDto>>(unitOfWork,
        mapper)
{
    public override async Task<IEnumerable<TodoDto>> Handle(TodoGetAll request,
        CancellationToken cancellationToken)
    {
        var entities = await UnitOfWork.Todos.GetAllAsync(cancellationToken: cancellationToken);

        return Mapper.Map<IEnumerable<TodoDto>>(entities);
    }
}