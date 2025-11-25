using Microsoft.EntityFrameworkCore;
using Project3.Application.Common.Interfaces;
using Project3.Infrastructure.Persistence;
using Project3.Infrastructure.Persistence.Repositories;
using Project3.Infrastructure.Persistence.Repositories.Appointments;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IServiceProviderRepository, ServiceProviderRepository>();
builder.Services.AddScoped<IWorkingHourRepository, WorkingHourRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// builder.Services.AddSwaggerGen();

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }


app.UseHttpsRedirection();

app.Run();
