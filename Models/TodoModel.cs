namespace AspNetCoreTodo.Models;

/// <summary>
/// <c>TodoModel</c> represents a single To-Do item. 
/// </summary>
public class TodoModel
{
    /// <summary>
    /// the ID of the To-Do item
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// when this To-Do item was created
    /// </summary>
    public DateTimeOffset CreationTime { get; }


    /// <summary>
    /// the last time when this To-Do item was modified
    /// </summary>
    public DateTimeOffset UpdateTime => _updateTime;

    private DateTimeOffset _updateTime;

    /// <summary>
    /// the title of the To-Do item
    /// </summary>
    public string Title
    {
        get => _title;
        set
        {
            _updateTime = DateTimeOffset.Now;
            _title = value;
        }
    }

    private string _title;

    /// <summary>
    /// the main content of the To-Do item
    /// </summary>
    public string Body
    {
        get => _body;
        set
        {
            _updateTime = DateTimeOffset.Now;
            _body = value;
        }
    }

    private string _body;

    /// <summary>
    /// the <see cref="CommentModel"/>s attached to this To-Do item
    /// </summary>
    public IList<CommentModel> Comments { get; set; } = new List<CommentModel>();

    /// <summary>
    /// Creates a new To-Do item.
    /// </summary>
    /// <param name="title">the title of the item</param>
    /// <param name="body">the main content of the item</param>
    public TodoModel(string title, string body)
    {
        Id = Guid.NewGuid();
        CreationTime = _updateTime = DateTimeOffset.Now;
        _title = title;
        _body = body;
    }

    /// <summary>
    /// Adds a new comment to this To-Do item.
    /// </summary>
    /// <param name="body">the main content of the comment</param>
    /// <returns>the new comment created</returns>
    public CommentModel AddNewComment(string body)
    {
        if (body.Trim().Length == 0)
            throw new ArgumentException("body is empty", nameof(body));

        CommentModel newComment = new(this, body);
        Comments.Add(newComment);

        return newComment;
    }
}