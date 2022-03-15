namespace AspNetCoreTodo.Services;

public interface ICommentService
{
    /// <summary>
    /// Deletes the comment with the given ID.
    /// </summary>
    /// <param name="id">the ID of the comment item</param>
    /// <param name="cancellationToken"></param>
    /// <returns><c>true</c> if the comment was present in the DB; <c>false</c> otherwise</returns>
    public Task<bool> DeleteCommentAsync(Guid id, CancellationToken cancellationToken = default);
}