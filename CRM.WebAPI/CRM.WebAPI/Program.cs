using System.Configuration;
using System.Text;
using CRM.Infrastructure.Interfaces;
using CRM.Infrastructure.Repositories;
using CRM.WebApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CRM.WebAPI;
using CRM.WebAPI.Services;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var configuration = builder.Configuration;
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "CRM WebApi", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["AuthOptions:Issuer"],
            ValidAudience = configuration["AuthOptions:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthOptions:Key"]))
        };
    });

builder.Services.AddSingleton<IClientRepository, ClientRepository>();
builder.Services.AddSingleton<IVisitRepository, VisitRepository>();
builder.Services.AddSingleton<IContactRepository, ContactRepository>();
builder.Services.AddSingleton<IFileRepository, FileRepository>();
builder.Services.AddSingleton<IFormRepository, FormRepository>();
builder.Services.AddSingleton<ILoginRepository, LoginRepository>();
builder.Services.AddSingleton<IPaymentRepository, PaymentRepository>();
builder.Services.AddSingleton<IPsychologistRepository, PsychologistRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IServiceRepository, ServiceRepository>();
builder.Services.AddSingleton<IScheduleRepository, ScheduleRepository>();



builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<ITokenRepository, TokenRepository>();

builder.Services.AddSingleton<ExceptionMiddleware>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
// app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.Run();