using AutoMapper;
using MediatR;
using Prohelika.Template.CleanArchitecture.Domain.UnitOfWorks;

namespace Prohelika.Template.CleanArchitecture.Application.Common.Common;

/// <summary>
/// Base handler for IRequest<TResponse>
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class BaseRequestHandler<TRequest, TResponse>
    (IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;
    public IMapper Mapper { get; } = mapper;
    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Base handler for IRequest
/// </summary>
/// <typeparam name="TRequest"></typeparam>
public abstract class BaseRequestHandler<TRequest>
    (IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<TRequest>
    where TRequest : IRequest
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;
    public IMapper Mapper { get; } = mapper;
    public abstract Task Handle(TRequest request, CancellationToken cancellationToken);
}