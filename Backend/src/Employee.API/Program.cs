using EmployeeManagerInfrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Infra (InMemory for now; replace with relational provider when needed)
builder.Services.AddInfrastructureInMemory("EmployeeDb");

// Application UseCases registration
builder.Services.AddScoped<EmployeeManagement.Application.Employees.UseCases.Create.CreateEmployeeUseCase>();
builder.Services.AddScoped<EmployeeManagement.Application.Employees.UseCases.Update.UpdateEmployeeUseCase>();
builder.Services.AddScoped<EmployeeManagement.Application.Employees.UseCases.Delete.DeleteEmployeeUseCase>();
builder.Services.AddScoped<EmployeeManagement.Application.Employees.UseCases.GetById.GetEmployeeByIdUseCase>();
builder.Services.AddScoped<EmployeeManagement.Application.Employees.UseCases.GetAll.GetAllEmployeesUseCase>();

// Authentication (JWT Bearer)
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var jwtSecret = builder.Configuration["Jwt:Secret"];
if (!string.IsNullOrWhiteSpace(jwtSecret))
{
    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = !string.IsNullOrWhiteSpace(jwtIssuer),
            ValidateAudience = !string.IsNullOrWhiteSpace(jwtAudience),
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });
}

// CORS (wide-open only for Development)
const string DevCorsPolicy = "DevCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: DevCorsPolicy, policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(DevCorsPolicy);
}

// Global error handling
app.UseMiddleware<EmployeeManagement.API.Middleware.GlobalExceptionMiddleware>();

// Custom JWT authentication middleware
app.UseMiddleware<EmployeeManagement.API.Middleware.JwtAuthMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


