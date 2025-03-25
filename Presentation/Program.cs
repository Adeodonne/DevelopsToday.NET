using Application;
using Domain.Interfaces;
using DotNetEnv;
using Infrastructure;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var dbInitializer = app.Services.GetRequiredService<IDatabaseInitializer>();
dbInitializer.EnsureDatabaseExists();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();