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
            string v = value.Trim();
            if (v.Length == 0)
                throw new ArgumentException("title is empty");

            _updateTime = DateTimeOffset.Now;
            _title = v;
        }
    }

    private string _title = null!;

    /// <summary>
    /// the main content of the To-Do item
    /// </summary>
    public string Body
    {
        get => _body;
        set
        {
            string v = value.Trim();
            if (v.Length == 0)
                throw new ArgumentException("body is empty");

            _updateTime = DateTimeOffset.Now;
            _body = v;
        }
    }

    private string _body = null!;

    private readonly IList<CommentModel> _comments = null!;

    /// <summary>
    /// the <see cref="CommentModel"/>s attached to this To-Do item
    /// </summary>
    public IEnumerable<CommentModel> Comments => _comments;

    /// <summary>
    /// Creates a new To-Do item.
    /// </summary>
    /// <param name="title">the title of the item</param>
    /// <param name="body">the main content of the item</param>
    public TodoModel(string title, string body)
    {
        Id = Guid.NewGuid();
        CreationTime = _updateTime = DateTimeOffset.Now;
        Title = title;
        Body = body;
        _comments = new List<CommentModel>();
    }

    /// <summary>
    /// Adds a new comment to this To-Do item.
    /// </summary>
    /// <param name="body">the main content of the comment</param>
    /// <returns>the new comment created</returns>
    public CommentModel AddNewComment(string body)
    {
        CommentModel newComment = new(this, body);
        _comments.Add(newComment);
        return newComment;
    }

    /// <summary>
    /// Removes the comment with the given ID from this To-Do item.
    /// </summary>
    /// <param name="id">the ID of the comment</param>
    public bool RemoveComment(Guid id)
    {
        int idx = -1;
        for (int i = 0; i < _comments.Count; ++i)
        {
            if (_comments[i].Id == id)
            {
                idx = i;
                break;
            }
        }

        if (idx != -1)
        {
            _comments.RemoveAt(idx);
            return true;
        }

        return false;
    }

    /// <remarks>
    /// Entity Framework Core requires a parameterless constructor defined in the model. Do not explicitly use this
    /// constructor.
    /// </remarks>
    public TodoModel()
    {
    }
}