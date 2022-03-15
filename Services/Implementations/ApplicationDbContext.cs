using AspNetCoreTodo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AspNetCoreTodo.Services.Implementations;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly string _dbPath;

    public ApplicationDbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        _dbPath = Path.Join(path, "todo.db");
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        => Database.BeginTransactionAsync(cancellationToken);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoModel>(model =>
        {
            model.HasKey(m => m.Id);
            model.Property(m => m.Id)
                .IsRequired();
            model.Property(m => m.CreationTime)
                .IsRequired();
            model.HasIndex(m => m.CreationTime);
            model.Ignore(m => m.UpdateTime);
            model.Property<DateTimeOffset>("_updateTime")
                .IsRequired()
                .HasColumnName("UpdateTime");
            model.HasIndex("_updateTime");
            model.Ignore(m => m.Title);
            model.Property<string>("_title")
                .IsRequired()
                .HasColumnName("Title");
            model.HasIndex("_title");
            model.Ignore(m => m.Body);
            model.Property<string>("_body")
                .IsRequired()
                .HasColumnName("Body");
            model.HasMany(m => m.Comments)
                .WithOne()
                .HasForeignKey("_todoId")
                .OnDelete(DeleteBehavior.ClientCascade);
            model.Navigation(m => m.Comments)
                .AutoInclude();
        });
        modelBuilder.Entity<TodoModel>().ToTable("Todos");

        modelBuilder.Entity<CommentModel>(model =>
        {
            model.HasKey(m => m.Id);
            model.Property(m => m.Id)
                .IsRequired();
            model.Property<Guid>("_todoId")
                .IsRequired()
                .HasColumnName("TodoId");
            model.Property(m => m.CreationTime)
                .IsRequired();
            model.Property(m => m.Body)
                .IsRequired();
        });
        modelBuilder.Entity<CommentModel>().ToTable("Comments");
    }
}