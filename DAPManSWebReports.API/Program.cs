using DAPManSWebReports.API.Services.DI.Interfaces;
using DAPManSWebReports.API.Services.DI.Registration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var serviceRegistration = new List<IServiceRegistration>
{
    new ControllerServiceRegistration(),
    new EntityRepoServiceRegistration(),
    new DomainRepoServiceRegistration(),
    new InfrastructureRepoServiceRegistration()
};
foreach (var registration in serviceRegistration)
{
    registration.RegisterServices(builder.Services);
}
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("https://localhost:5173") 
                            .AllowAnyMethod()
                            .AllowAnyHeader());
});
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
//app.MapControllers();

app.Run();
