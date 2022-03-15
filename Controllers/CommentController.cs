using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreTodo.Controllers;

/// <summary>
/// A Comments represents a single comment attached to a single To-Do item.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentService _service;

    public CommentController(ICommentService service)
    {
        _service = service;
    }

    /// <summary>
    /// Deletes the comment with the given ID.
    /// </summary>
    /// <param name="commentId">the ID of the comment item</param>
    [HttpDelete("{commentId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteCommentAsync([FromRoute] Guid commentId)
    {
        return await _service.DeleteCommentAsync(commentId) ? Ok() : NotFound();
    }
}