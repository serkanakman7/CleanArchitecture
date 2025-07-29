using App.API.ExceptionHandler;
using App.API.Extensions;
using App.API.Filters;
using App.Application.Contracts.Caching;
using App.Application.Extensions;
using App.Bus;
using App.Caching;
using App.Domain.Options;
using App.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithFiltersExt().AddSwaggerGenExt().AddExceptionHandlerExt();

builder.Services.AddPersistences(builder.Configuration).AddApplications()
    .AddBusExt(builder.Configuration);

var keycloakOption = builder.Configuration.GetSection(nameof(KeycloakOption)).Get<KeycloakOption>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = keycloakOption.Authority;
        options.Audience = keycloakOption.Audience;
        options.RequireHttpsMetadata = keycloakOption.RequireHttpsMetadata;
        // Eðer HTTPS yoksa ve self-signed sertifika varsa bunu dev ortamda açabilirsin:
        options.TokenValidationParameters.ValidateIssuer = true;
    });

builder.Services.AddCachingExt(builder.Configuration);

var app = builder.Build();

app.UseConfigurePipelineExt();

app.MapControllers();

app.Run();
