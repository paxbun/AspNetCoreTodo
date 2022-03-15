using AspNetCoreTodo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AspNetCoreTodo.Services.Implementations;

public class CommentService : ICommentService
{
    private readonly IApplicationDbContext _dbContext;

    public CommentService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> DeleteCommentAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            await using IDbContextTransaction transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

            CommentModel? comment =
                await _dbContext.Set<CommentModel>().FindAsync(new object[] {id}, cancellationToken);
            if (comment is null)
            {
                return false;
            }

            _dbContext.Set<CommentModel>().Remove(comment);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }
}