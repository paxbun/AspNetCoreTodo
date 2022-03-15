using AspNetCoreTodo.Services;
using AspNetCoreTodo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreTodo.Controllers;

/// <summary>
/// A &#x54;odo represents a single To-Do item.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly ITodoService _service;

    public TodoController(ITodoService service)
    {
        _service = service;
    }

    /// <summary>
    /// Returns all To-Do items present in the DB.
    /// </summary>
    /// <param name="sortedBy">the name of the column by which the items are to be sorted</param>
    /// <param name="includeComments">whether to include comments in each item</param>
    /// <returns>the collection of To-Do items</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IEnumerable<Todo>> GetAllTodosAsync(
        [FromQuery] ITodoService.TodoSortedBy? sortedBy,
        [FromQuery] bool? includeComments
    ) => _service.GetAllTodosAsync(sortedBy, includeComments);

    /// <summary>
    /// Returns the To-Do item with the given ID.
    /// </summary>
    /// <param name="todoId">the ID of the item</param>
    /// <returns>the item if present; <c>null</c> otherwise</returns>
    [HttpGet("{todoId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Todo>> GetTodoByIdAsync(
        [FromRoute] Guid todoId
    )
    {
        Todo? todo = await _service.GetTodoByIdAsync(todoId);
        if (todo is null)
        {
            return NotFound();
        }

        return Ok(todo);
    }

    /// <summary>
    /// Creates a new To-Do item based on the given command.
    /// </summary>
    /// <param name="command"></param>
    /// <returns>the new To-Do item created</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Todo>> CreateNewTodoAsync(
        [FromBody] ITodoService.TodoPostModel command
    )
    {
        try
        {
            Todo todo = await _service.CreateNewTodoAsync(command);
            return CreatedAtAction("GetTodoById", new {todoId = todo.Id}, todo);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex);
        }
    }

    /// <summary>
    /// Updates the To-Do item with the given ID with the information contained in the given command.
    /// </summary>
    /// <param name="todoId">the ID of the To-Do item</param>
    /// <param name="command"></param>
    /// <returns>the To-Do item with the given ID</returns>
    [HttpPatch("{todoId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Todo>> PatchTodoAsync(
        [FromRoute] Guid todoId,
        [FromBody] ITodoService.TodoPatchModel command
    )
    {
        try
        {
            Todo? todo = await _service.PatchTodoAsync(todoId, command);
            if (todo is null)
            {
                return NotFound();
            }

            return Ok(todo);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex);
        }
    }

    /// <summary>
    /// Deletes the To-Do item with the given ID.
    /// </summary>
    /// <param name="todoId">the ID of the To-Do item</param>
    [HttpDelete("{todoId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteTodoAsync([FromRoute] Guid todoId)
    {
        return await _service.DeleteTodoAsync(todoId) ? Ok() : NotFound();
    }

    /// <summary>
    /// Returns the collection of comments of the To-Do item with the given ID.
    /// </summary>
    /// <param name="todoId">the ID of the To-Do item</param>
    /// <returns>the collection of comments if the To-Do item is present in the DB</returns>
    [HttpGet("{todoId:guid}/comment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsOfTodoAsync([FromRoute] Guid todoId)
    {
        IEnumerable<Comment>? comments = await _service.GetCommentsOfTodoAsync(todoId);
        if (comments is null)
        {
            return NotFound();
        }

        return Ok(comments);
    }

    /// <summary>
    /// Adds a comment to the To-Do item with the given ID.
    /// </summary>
    /// <param name="todoId">the ID of the To-Do item</param>
    /// <param name="command"></param>
    /// <returns>the new comment item added to the To-Do item</returns>
    [HttpPost("{todoId:guid}/comment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Comment>> AddCommentAsync(
        [FromRoute] Guid todoId,
        [FromBody] ITodoService.CommentPostModel command
    )
    {
        try
        {
            Comment? comment = await _service.AddCommentAsync(todoId, command);
            if (comment is null)
            {
                return NotFound();
            }

            return Ok(comment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex);
        }
    }
}