namespace AspNetCoreTodo.Models;

/// <summary>
/// <c>CommentModel</c> represents a single comment attached to a To-Do item.
/// </summary>
public class CommentModel
{
    /// <summary>
    /// the ID of this comment. This ID identifies the comment within the server.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// the ID of the To-Do item where this comment is attached
    /// </summary>
    private Guid _todoId { get; }

    /// <summary>
    /// when this comment was created
    /// </summary>
    public DateTimeOffset CreationTime { get; }

    /// <summary>
    /// the main content of the comment
    /// </summary>
    public string Body { get; }

    /// <summary>
    /// Attach a new comment to the given To-Do item.
    /// </summary>
    /// <param name="todoModel">the To-do item where the new comment will be attached</param>
    /// <param name="body">the main content of the comment</param>
    public CommentModel(TodoModel todoModel, string body)
    {
        Id = Guid.NewGuid();
        _todoId = todoModel.Id;
        CreationTime = DateTimeOffset.Now;
        Body = body;
    }
}