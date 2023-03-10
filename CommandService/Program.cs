using CommandService.AsyncDataServices;
using CommandService.Data;
using CommandService.Data.Interfaces;
using CommandService.Data.Repositories;
using CommandService.EventProcessing;
using CommandService.EventProcessing.Interfaces;
using CommandService.SyncDataServices.Grpc;
using CommandService.SyncDataServices.Grpc.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//DB connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("CommandSrvcInMemory"));

//DI
builder.Services.AddScoped<ICommandRepository, CommandRepository>();

builder.Services.AddControllers();

builder.Services.AddHostedService<MessageBusSubscriber>();

builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IPlatformDataClient, PlatformDataClient>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

PrepDb.PrepPopulation(app);

app.Run();
