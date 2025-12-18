using Microsoft.EntityFrameworkCore;
using Project3.Application;
using Project3.Application.Common.Interfaces;
using Project3.Infrastructure.Persistence;
using Project3.Infrastructure.Persistence.Repositories;
using Project3.Infrastructure.Persistence.Repositories.Appointments;
using Project3.Infrastructure.Services.Email;

var builder = WebApplication.CreateBuilder(args);

// Swagger / OpenAPI
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Application services (MediatR, Validators...)
builder.Services.AddApplicationServices();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        // .UseSnakeCaseNamingConvention());

builder.Services.AddDbContext<LoggingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Email Settings
var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
if (emailSettings != null)
{
    builder.Services.AddSingleton(emailSettings);
}

// Repositories
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IServiceProviderRepository, ServiceProviderRepository>();
builder.Services.AddScoped<IWorkingHourRepository, WorkingHourRepository>();
builder.Services.AddScoped<IBlockedTimesRepository, BlockedTimesRepository>();
builder.Services.AddScoped<IEmailQueueRepository, EmailQueueRepository>(); // ADD THIS
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<INotificationLogsRepository, NotificationLogsRepository>();

// Services
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

// Background Service
builder.Services.AddHostedService<EmailSenderBackgroundService>();

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