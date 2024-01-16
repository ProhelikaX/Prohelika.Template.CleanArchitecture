using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prohelika.Template.CleanArchitecture.Presentation.Common.Utils;

namespace Prohelika.Template.CleanArchitecture.Presentation.WebAPI.Common;

/// <summary>
/// Base API controller with Mediator
/// </summary>
[Route("[controller]")]
[ApiController]
[Authorize(AppAuthPolicy.IsAdmin)]
public abstract class BaseController(IMediator mediator) : ControllerBase
{
    protected readonly IMediator Mediator = mediator;
}