using HospitalBusinessLayer.Core;
using HospitalDataLayer.Infrastructure;
using HospitalDataLayer.Infrastructure.Interfaces;
using Serilog;


var builder = WebApplication.CreateBuilder(args);


// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Read from appsettings.json
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); // Use Serilog instead of default logging


// Register the connection string in the DI container
builder.Services.AddSingleton(sp => builder.Configuration.GetConnectionString("DefaultConnection"));



// Add services to the container.

// This is required to use the controllers created.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IVisitorData, clsVisitorData>();
builder.Services.AddScoped<clsVisitor>();
builder.Services.AddScoped<IPatientData, clsPatientData>();
builder.Services.AddScoped<clsPatient>();
builder.Services.AddScoped<IDoctorData, clsDoctorData>();
builder.Services.AddScoped<clsDoctor>();
builder.Services.AddScoped<IStaffData, clsStaffData>();
builder.Services.AddScoped<clsStaff>();
builder.Services.AddScoped<IShiftData, clsShiftData>();
builder.Services.AddScoped<clsShift>();
builder.Services.AddScoped<IInsuranceClaimData, clsInsuranceClaimData>();
builder.Services.AddScoped<clsInsuranceClaim>();
builder.Services.AddScoped<IBillData, clsBillData>();
builder.Services.AddScoped<clsBill>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add Serilog request logging
app.UseSerilogRequestLogging();


app.UseHttpsRedirection();

// This line ensures controllers will be used.
app.MapControllers();

app.Run();