using Microsoft.EntityFrameworkCore;
using PaltformService.Data;
using PaltformService.Data.DataPreparation;
using PaltformService.Data.Interfaces;
using PaltformService.Data.Repositories;
using PaltformService.SyncDataServices.Http;
using PaltformService.SyncDataServices.Http.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//DB connection
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseInMemoryDatabase("PlatformSrvcInMemory"));

//DI
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

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

DataPreparation.Populate(app);
System.Console.WriteLine($"--> CommandService endpoint: {builder.Configuration["CommandServiceURL"]}");

app.Run();
