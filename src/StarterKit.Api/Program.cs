using Asp.Versioning;
using Serilog;
using StarterKit.Api.Extensions;
using StarterKit.Api.Middleware;
using StarterKit.Api.Middleware.Observability;
using StarterKit.Api.Swagger;
using StarterKit.Infrastructure.Data.DependencyInjection;
using StarterKit.Infrastructure.Data.Persistence.Seed;
using StarterKit.Infrastructure.External.DependencyInjection;
using StarterKit.Infrastructure.External.Logging.Serilog;
using StarterKit.UseCase.DependencyInjection;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) => cfg.ConfigureSerilog(ctx.Configuration));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddUseCases();
builder.Services.AddInfrastructureData(builder.Configuration);
builder.Services.AddInfrastructureExternal(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services.AddOutputCache();
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("global", limiter =>
    {
        limiter.Window = TimeSpan.FromMinutes(1);
        limiter.PermitLimit = 100;
        limiter.QueueLimit = 10;
        limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});
builder.Services.AddStarterKitObservability(builder.Configuration);

var app = builder.Build();

if (builder.Configuration.GetValue("SeedData:Enabled", true))
{
    await ApplicationDbSeeder.SeedAsync(app.Services);
}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseSerilogRequestLogging();
app.UseSwaggerDocumentation();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.UseOutputCache();
app.MapStarterKitObservability();
app.MapApiEndpoints();

app.Run();

public partial class Program { }
