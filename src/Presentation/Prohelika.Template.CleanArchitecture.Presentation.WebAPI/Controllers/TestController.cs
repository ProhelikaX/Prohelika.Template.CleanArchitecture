using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prohelika.Template.CleanArchitecture.Presentation.WebAPI.Common;

namespace Prohelika.Template.CleanArchitecture.Presentation.WebAPI.Controllers;

/// <summary>
/// Test controller
/// </summary>
/// <param name="mediator"></param>
public class TestController(IMediator mediator) : BaseController(mediator)
{
    
    /// <summary>
    /// Test endpoint
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<ActionResult> Get()
    {
        var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
        return Task.FromResult<ActionResult>(Ok(userRoles));
    }
    
}