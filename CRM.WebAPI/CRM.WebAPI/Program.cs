using CRM.Domain.Models;
using CRM.Infrastructure.Interfaces;
using CRM.Infrastructure.Repositories;
using CRM.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IClientRepository, ClientRepository>();
builder.Services.AddSingleton<IVisitRepository, VisitRepository>();
builder.Services.AddSingleton<IContactRepository, ContactRepository>();
builder.Services.AddSingleton<IFileRepository, FileRepository>();
builder.Services.AddSingleton<IFormRepository, FormRepository>();


builder.Services.AddSingleton<ExceptionMiddleware>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.Run();