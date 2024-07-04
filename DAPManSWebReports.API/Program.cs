using DAPManSWebReports.API.Services.DI.Interfaces;
using DAPManSWebReports.API.Services.DI.Registration;
using DAPManSWebReports.Domain.ErrorReportService;
using DAPManSWebReports.Domain.IdentityService.TokenServise;
using LoggingLibrary.Service;

using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.AddTransient<IEmailService, EmailService>(provider =>
{
    var smtpSettings = provider.GetService<IOptions<SmtpSettings>>();
    var receiverEmail = configuration.GetValue<string>("ReceiverEmail");
    return new EmailService(smtpSettings, receiverEmail);
});

builder.Logging
           .ClearProviders()
           .AddConsole()
           .AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));

// Add services to the container.

var serviceRegistration = new List<IServiceRegistration>
{
    new ControllerServiceRegistration(),
    new EntityRepoServiceRegistration(),
    new DomainRepoServiceRegistration(),
    new InfrastructureRepoServiceRegistration()
    //new LoggerLibraryServiceRegistration()
};
foreach (var registration in serviceRegistration)
{
    registration.RegisterServices(builder.Services);
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>   builder.WithOrigins("https://localhost:5173") 
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
});
builder.Services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));

builder.Services.AddMemoryCache();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "CustomScheme";
    options.DefaultChallengeScheme    = "CustomScheme";
});
// Добавляем настройки аутентификации для собственного JWT
builder.Services.AddCustomJwtAuthentication(builder.Configuration);

// Добавляем настройки аутентификации для Keycloak
builder.Services.AddKeycloakAuthentication(builder.Configuration);

// Добавляем политику аутентификации для  собственной схемы
builder.Services.AddAuthorization(options =>
{
    // Политика для использования собственного JWT
    options.AddPolicy("CustomPolicy", policy =>
                                                policy.RequireAuthenticatedUser()
                                                      .AddAuthenticationSchemes("CustomScheme")
                                                      .RequireAuthenticatedUser());

    // Политика для использования Keycloak
    options.AddPolicy("KeycloakPolicy", policy =>
                                                policy.RequireAuthenticatedUser()
                                                      .AddAuthenticationSchemes("KeycloakScheme")
                                                      .RequireAuthenticatedUser());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web Report API", Version = "v1" });

    // Определите схему безопасности
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Пожалуйста, введите ваш JWT токен, используя это поле (пример: 'Bearer 12345abcdef')",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
           {
               {
                   new OpenApiSecurityScheme
                   {
                       Reference = new OpenApiReference
                       {
                           Type = ReferenceType.SecurityScheme,
                           Id = "Bearer"
                       }
                   },
                   new string[] { }
               }
           });
});
//builder.Services.AddResponseCaching();
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});
app.MapControllers();
//app.UseResponseCaching();
app.Run();
