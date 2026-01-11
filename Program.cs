using LicenseService.Data;
using LicenseService.Exceptions;
using LicenseService.Model;
using LicenseService.Service;
using LicenseService.Service.Impl;
using LicenseService.Worker;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("PostgresConnection"),
        npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
        ));

builder.Services.Configure<AppConfigSetting>(
    builder.Configuration.GetSection("AppSettings")
    );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowSpecificOrigins",
//         builder =>
//         {
//             builder.WithOrigins("https://example.com", "https://www.example.com")
//                    .AllowAnyHeader()
//                    .AllowAnyMethod();
//         });
// });
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true; // optional
});

builder.Host.UseSerilog((ctx, services, cfg) =>
{
    cfg.ReadFrom.Configuration(ctx.Configuration)
       .ReadFrom.Services(services)
       .Enrich.FromLogContext()
       .WriteTo.Console();
});

builder.Services.AddHostedService<KeyRotationWorker>();
builder.Services.AddScoped<ILicenseService, LicService>();
builder.Services.AddScoped<IKeyRotateService, KeyRotateService>();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();


app.UseCors("AllowSpecificOrigins");
app.UseHttpsRedirection();
// This logs every HTTP request automatically
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

