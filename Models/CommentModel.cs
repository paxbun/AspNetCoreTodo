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
    public Guid TodoId { get; }

    /// <summary>
    /// when this comment was created
    /// </summary>
    public DateTimeOffset CreationTime { get; }

    /// <summary>
    /// the main content of the comment
    /// </summary>
    public string Body { get; } = null!;

    /// <summary>
    /// Attach a new comment to the given To-Do item.
    /// </summary>
    /// <param name="todoModel">the To-do item where the new comment will be attached</param>
    /// <param name="body">the main content of the comment</param>
    public CommentModel(TodoModel todoModel, string body)
    {
        if (body.Trim().Length == 0)
            throw new ArgumentException("body is empty", nameof(body));

        Id = Guid.NewGuid();
        TodoId = todoModel.Id;
        CreationTime = DateTimeOffset.Now;
        Body = body;
    }

    /// <remarks>
    /// Entity Framework Core requires a parameterless constructor defined in the model. Do not explicitly use this
    /// constructor.
    /// </remarks>
    public CommentModel()
    {
    }
}