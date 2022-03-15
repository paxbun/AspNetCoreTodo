using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AspNetCoreTodo.Services;

/// <summary>
/// IApplicationDbContext represents a connection to the DB.
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Retrieves a table
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public DbSet<TEntity> Set<TEntity>() where TEntity : class;

    /// <summary>
    /// Creates a new transaction.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>the new transaction instance</returns>
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Save all changes made in this context to the database. 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>the number of entries written</returns>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Save all changes made in this context to the database. 
    /// </summary>
    /// <returns>the number of entries written</returns>
    public int SaveChanges();
}