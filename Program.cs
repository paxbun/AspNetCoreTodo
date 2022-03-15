using System.Reflection;
using AspNetCoreTodo;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var environment = builder.Environment;

services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);
    options.DescribeAllParametersInCamelCase();
    options.SupportNonNullableReferenceTypes();
    options.UseAllOfToExtendReferenceSchemas();
    options.SchemaFilter<RequiredPropertiesSchemaFilter>();
});
services.AddRouting(options => { options.LowercaseUrls = true; });
services.AddControllers();

var app = builder.Build();

if (environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.MapControllers();
app.Run();