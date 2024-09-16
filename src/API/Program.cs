using Framework.Core.Extensions;
using Framework.Core.Middleware;

var builder = WebApplication.CreateBuilder(args);

await builder.Services.AddApplicationCoreServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
