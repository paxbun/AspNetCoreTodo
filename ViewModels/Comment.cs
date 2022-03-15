using AspNetCoreTodo.Models;

namespace AspNetCoreTodo.ViewModels;

/// <summary>
/// A Comment represents a single comment attached to a single To-Do item. 
/// </summary>
/// <param name="Id">the ID of the comment</param>
/// <param name="CreationTime">when this comment was created</param>
/// <param name="Body">the main content of the comment</param>
public record Comment(
    Guid Id,
    DateTimeOffset CreationTime,
    string Body
);

public static class CommentExtensions
{
    public static Comment ToViewModel(this CommentModel commentModel)
        => new Comment(commentModel.Id, commentModel.CreationTime, commentModel.Body);
}