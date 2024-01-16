using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prohelika.Template.CleanArchitecture.Application.Features.Todos;
using Prohelika.Template.CleanArchitecture.Presentation.Common.Extensions;
using Prohelika.Template.CleanArchitecture.Presentation.WebAPI.Common;

namespace Prohelika.Template.CleanArchitecture.Presentation.WebAPI.Controllers;

/// <summary>
/// Todo controller
/// </summary>
/// <param name="mediator"></param>
public class TodoController(IMediator mediator) : BaseController(mediator)
{
    /// <summary>
    /// Get all todos
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoDto>>> Get()
    {
        var response = await Mediator.Send(new GetAllTodo());

        return Ok(response);
    }

    /// <summary>
    /// Get todo by id
    /// </summary>
    /// <param name="id">Id of todo</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoDto>> Get(Guid id)
    {
        var response = await Mediator.Send(new GetTodoById(id));

        return Ok(response);
    }

    /// <summary>
    /// Create new todo
    /// </summary>
    /// <param name="todoCreateDto">Todo create dto</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<TodoDto>> Create(TodoCreateDto todoCreateDto)
    {
        var response = await Mediator.Send(new AddTodo(User.GetId(), todoCreateDto));

        return CreatedAtAction(nameof(Get), new { response.Id }, response);
    }

    /// <summary>
    /// Update todo
    /// </summary>
    /// <param name="id">Id of todo</param>
    /// <param name="todoDto">Todo update dto</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<TodoDto>> Update(Guid id, TodoDto todoDto)
    {
        var response = await Mediator.Send(new UpdateTodo(id, todoDto));

        return Ok(response);
    }

    /// <summary>
    /// Delete todo
    /// </summary>
    /// <param name="id">Id of todo</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeleteTodo(id));

        return NoContent();
    }
}