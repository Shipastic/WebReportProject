using DAPManSWebReports.API.Services.DI.Interfaces;
using DAPManSWebReports.API.Services.DI.Registration;
using DAPManSWebReports.Domain.ErrorReportService;

using LoggingLibrary.Service;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.Text;
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

// Добавление аутентификации с использованием OpenID Connect (OIDC)
// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
