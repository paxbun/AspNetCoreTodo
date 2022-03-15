namespace AspNetCoreTodo.Services;

public interface ICommentService
{
    /// <summary>
    /// Deletes the comment with the given ID.
    /// </summary>
    /// <param name="id">the ID of the comment item</param>
    /// <returns><c>true</c> if the comment was present in the DB; <c>false</c> otherwise</returns>
    public bool DeleteComment(Guid id);
}