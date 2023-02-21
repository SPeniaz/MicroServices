using Microsoft.EntityFrameworkCore;
using PaltformService.Data;
using PaltformService.Data.DataPreparation;
using PaltformService.Data.Interfaces;
using PaltformService.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

//DB connection
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseInMemoryDatabase("PlatformSrvcInMemory"));

//DI
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();

builder.Services.AddControllers();
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

DataPreparation.Populate(app);

app.Run();
