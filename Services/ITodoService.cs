using AspNetCoreTodo.Models;
using AspNetCoreTodo.ViewModels;

namespace AspNetCoreTodo.Services;

public interface ITodoService
{
    public enum TodoSortedBy
    {
        CreationTime,
        UpdateTime,
        Title,
    }

    /// <summary>
    /// Returns all To-Do items present in the DB.
    /// </summary>
    /// <param name="sortedBy">the name of the column by which the items are to be sorted</param>
    /// <param name="includeComments">whether to include comments in each item</param>
    /// <param name="cancellationToken"></param>
    /// <returns>the collection of To-Do items</returns>
    public Task<IEnumerable<Todo>> GetAllTodosAsync(
        TodoSortedBy? sortedBy,
        bool? includeComments,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns the To-Do item with the given ID.
    /// </summary>
    /// <param name="id">the ID of the item</param>
    /// <param name="cancellationToken"></param>
    /// <returns>the item if present; <c>null</c> otherwise</returns>
    public Task<Todo?> GetTodoByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public class TodoPostModel
    {
        /// <summary>
        /// the title of the item
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// the main content of the item
        /// </summary>
        public string Body { get; set; } = string.Empty;
    }

    /// <summary>
    /// Creates a new To-Do item based on the given command.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>the new To-Do item created</returns>
    public Task<Todo> CreateNewTodoAsync(TodoPostModel command, CancellationToken cancellationToken = default);

    public class TodoPatchModel
    {
        /// <summary>
        /// the title of the item
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// the main content of the item
        /// </summary>
        public string? Body { get; set; }
    }

    /// <summary>
    /// Updates the To-Do item with the given ID with the information contained in the given command.
    /// </summary>
    /// <param name="id">the ID of the To-Do item</param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>the item if present; <c>null</c> otherwise</returns>
    public Task<Todo?> PatchTodoAsync(Guid id, TodoPostModel command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the To-Do item with the given ID.
    /// </summary>
    /// <param name="id">the ID of the To-Do item</param>
    /// <param name="cancellationToken"></param>
    /// <returns><c>true</c> if the item was present in the DB; <c>false</c> otherwise</returns>
    public Task<bool> DeleteTodoAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Returns the collection of comments of the To-Do item with the given ID.
    /// </summary>
    /// <param name="id">the ID of the To-Do item</param>
    /// <param name="cancellationToken"></param>
    /// <returns>the collection of comments if the To-Do item is present in the DB; <c>null</c> otherwise</returns>
    public Task<IEnumerable<Comment>?> GetCommentsOfTodoAsync(Guid id, CancellationToken cancellationToken = default);

    public class CommentPostModel
    {
        /// <summary>
        /// the main content of the comment
        /// </summary>
        public string Body { get; set; } = string.Empty;
    }

    /// <summary>
    /// Adds a comment to the To-Do item with the given ID.
    /// </summary>
    /// <param name="id">the ID of the To-Do item</param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>the new comment item added to the To-Do item</returns>
    public Task<Comment> AddCommentAsync(
        Guid id,
        CommentPostModel command,
        CancellationToken cancellationToken = default
    );
}