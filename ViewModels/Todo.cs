using AspNetCoreTodo.Models;

namespace AspNetCoreTodo.ViewModels;

/// <summary>
/// A &#x54;odo represents a single To-Do item.
/// </summary>
/// <param name="Id">the ID of the To-Do item</param>
/// <param name="CreationTime">when this item was created</param>
/// <param name="UpdateTime">the last time when this item was mutated</param>
/// <param name="Title">the title of the item</param>
/// <param name="Body">the body of the item</param>
public record Todo(
    Guid Id,
    DateTimeOffset CreationTime,
    DateTimeOffset UpdateTime,
    string Title,
    string Body
)
{
    /// <summary>
    /// the comments attached to this item. <c>null</c> if <c>includeComments</c> is false.
    /// </summary>
    public IEnumerable<Comment>? Comments { get; init; }
}

public static class TodoExtensions
{
    public static Todo ToViewModel(this TodoModel todoModel, bool includeComments = false)
    {
        Todo rtn = new(
            todoModel.Id,
            todoModel.CreationTime,
            todoModel.UpdateTime,
            todoModel.Title,
            todoModel.Body
        );
        if (includeComments)
        {
            rtn = rtn with
            {
                Comments = todoModel.Comments.Select(CommentExtensions.ToViewModel)
            };
        }

        return rtn;
    }
}