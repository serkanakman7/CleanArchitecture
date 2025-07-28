using App.API.ExceptionHandler;
using App.API.Extensions;
using App.API.Filters;
using App.Application.Contracts.Caching;
using App.Application.Extensions;
using App.Bus;
using App.Caching;
using App.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithFiltersExt().AddSwaggerGenExt().AddExceptionHandlerExt();

builder.Services.AddPersistences(builder.Configuration).AddApplications()
    .AddBusExt(builder.Configuration);

builder.Services.AddCachingExt(builder.Configuration);

var app = builder.Build();

app.UseConfigurePipelineExt();

app.MapControllers();

app.Run();
