using Microsoft.EntityFrameworkCore;
using Project3.Application;
using Project3.Application.Common.Interfaces;
using Project3.Infrastructure.Persistence;
using Project3.Infrastructure.Persistence.Repositories;
using Project3.Infrastructure.Persistence.Repositories.Appointments;

var builder = WebApplication.CreateBuilder(args);

//  Swagger / OpenAPI
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Application services (MediatR, Validators...)
builder.Services.AddApplicationServices();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSnakeCaseNamingConvention());

// Repositories
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IServiceProviderRepository, ServiceProviderRepository>();
builder.Services.AddScoped<IWorkingHourRepository, WorkingHourRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Swagger 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();