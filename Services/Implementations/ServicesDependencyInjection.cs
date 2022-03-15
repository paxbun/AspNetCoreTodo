namespace AspNetCoreTodo.Services.Implementations;

public static class ServicesDependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>();
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>(
            serviceProvider => serviceProvider.GetService<ApplicationDbContext>()!);
        services.AddScoped<ITodoService, TodoService>();
        services.AddScoped<ICommentService, CommentService>();
        return services;
    }
}