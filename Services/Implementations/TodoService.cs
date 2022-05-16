using AspNetCoreTodo.Models;
using AspNetCoreTodo.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AspNetCoreTodo.Services.Implementations;

public class TodoService : ITodoService
{
    private readonly IApplicationDbContext _dbContext;

    public TodoService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Todo>> GetAllTodosAsync(ITodoService.TodoSortedBy? sortedBy, bool? includeComments,
        CancellationToken cancellationToken = default)
    {
        TodoModel[] models = await _dbContext.Set<TodoModel>().ToArrayAsync(cancellationToken);
        IEnumerable<Todo> query = models.Select(todo => todo.ToViewModel(includeComments == true));

        // SQLite does not support ordering with DateTimeOffset
        if (sortedBy is { } s)
        {
            switch (s)
            {
                case ITodoService.TodoSortedBy.CreationTime:
                    query = query.OrderBy(todo => todo.CreationTime);
                    break;
                case ITodoService.TodoSortedBy.UpdateTime:
                    query = query.OrderBy(todo => todo.UpdateTime);
                    break;
                case ITodoService.TodoSortedBy.Title:
                    query = query.OrderBy(todo => todo.Title);
                    break;
            }
        }

        return query.ToArray();
    }

    public async Task<Todo?> GetTodoByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        TodoModel? todo = await _dbContext.Set<TodoModel>().FindAsync(new object[] {id}, cancellationToken);
        return todo?.ToViewModel(true);
    }

    public async Task<Todo> CreateNewTodoAsync(ITodoService.TodoPostModel command,
        CancellationToken cancellationToken = default)
    {
        await using IDbContextTransaction transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

        TodoModel todo = new(command.Title, command.Body);
        _dbContext.Set<TodoModel>().Add(todo);

        await _dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return todo.ToViewModel(true);
    }

    public async Task<Todo?> PatchTodoAsync(Guid id, ITodoService.TodoPatchModel command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using IDbContextTransaction transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

            TodoModel? todo = await _dbContext.Set<TodoModel>().FindAsync(new object[] {id}, cancellationToken);
            if (todo is null)
            {
                return null;
            }

            if (command.Title is not null)
                todo.Title = command.Title;

            if (command.Body is not null)
                todo.Body = command.Body;

            _dbContext.Set<TodoModel>().Update(todo);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return todo.ToViewModel(true);
        }
        catch (DbUpdateException)
        {
            return null;
        }
    }

    public async Task<bool> DeleteTodoAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using IDbContextTransaction transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

            TodoModel? todo = await _dbContext.Set<TodoModel>().FindAsync(new object[] {id}, cancellationToken);
            if (todo is null)
            {
                return false;
            }

            _dbContext.Set<TodoModel>().Remove(todo);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    public async Task<IEnumerable<Comment>?> GetCommentsOfTodoAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        TodoModel? todo = await _dbContext.Set<TodoModel>().FindAsync(new object[] {id}, cancellationToken);
        return todo?.Comments.Select(CommentExtensions.ToViewModel);
    }

    public async Task<Comment?> AddCommentAsync(
        Guid id,
        ITodoService.CommentPostModel command,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            await using IDbContextTransaction transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

            TodoModel? todo = await _dbContext.Set<TodoModel>().FindAsync(new object[] {id}, cancellationToken);
            if (todo is null)
            {
                return null;
            }

            CommentModel comment = todo.AddNewComment(command.Body);
            _dbContext.Set<CommentModel>().Add(comment);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return comment.ToViewModel();
        }
        catch (DbUpdateException)
        {
            return null;
        }
    }

    public async Task<bool> RemoveCommentAsync(Guid id, Guid commentId, CancellationToken cancellationToken)
    {
        try
        {
            await using IDbContextTransaction transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

            TodoModel? todo = await _dbContext.Set<TodoModel>().FindAsync(new object[] {id}, cancellationToken);
            if (todo is null)
            {
                return false;
            }

            bool result = todo.RemoveComment(commentId);

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }
}