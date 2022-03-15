namespace AspNetCoreTodo.Services.Implementations;

public static class ServicesDependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITodoService, TodoService>();
        services.AddScoped<ICommentService, CommentService>();
        return services;
    }
}