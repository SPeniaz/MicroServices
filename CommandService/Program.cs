using CommandService.Data;
using CommandService.Data.Interfaces;
using CommandService.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//DB connection
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseInMemoryDatabase("CommandSrvcInMemory"));

//DI
builder.Services.AddScoped<ICommandRepository, CommandRepository>();

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
