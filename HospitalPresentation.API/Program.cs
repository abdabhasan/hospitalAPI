using System.Text;
using HospitalBusinessLayer.Core;
using HospitalDataLayer.Infrastructure;
using HospitalDataLayer.Infrastructure.Helpers;
using HospitalDataLayer.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;


var builder = WebApplication.CreateBuilder(args);


// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Read from appsettings.json
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); // Use Serilog instead of default logging


// Register the connection string in the DI container
builder.Services.AddSingleton(sp => builder.Configuration.GetConnectionString("DefaultConnection")!);



// Add JWT authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true
    };
});



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
builder.Services.AddScoped<IUserData, clsUserData>();
builder.Services.AddScoped<clsUser>();
builder.Services.AddScoped<PasswordHelper>();





var app = builder.Build();

// Use authentication middleware
app.UseAuthentication();

app.UseAuthorization();

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