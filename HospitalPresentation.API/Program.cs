using HospitalBusinessLayer.Core;
using HospitalDataLayer.Infrastructure;
using HospitalDataLayer.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);



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





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// This line ensures controllers will be used.
app.MapControllers();

app.Run();