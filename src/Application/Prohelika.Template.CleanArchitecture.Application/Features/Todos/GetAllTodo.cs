using AutoMapper;
using MediatR;
using Prohelika.Template.CleanArchitecture.Application.Common.Common;
using Prohelika.Template.CleanArchitecture.Domain.UnitOfWorks;

namespace Prohelika.Template.CleanArchitecture.Application.Features.Todos;

public record GetAllTodo : IRequest<IEnumerable<TodoDto>>;

public class GetAllTodoHandler
    (IUnitOfWork unitOfWork, IMapper mapper) : BaseRequestHandler<GetAllTodo, IEnumerable<TodoDto>>(unitOfWork,
        mapper)
{
    public override async Task<IEnumerable<TodoDto>> Handle(GetAllTodo request,
        CancellationToken cancellationToken)
    {
        var entities = await unitOfWork.Todos.GetAllAsync(cancellationToken: cancellationToken);

        return mapper.Map<IEnumerable<TodoDto>>(entities);
    }
}