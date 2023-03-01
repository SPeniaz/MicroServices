using Microsoft.EntityFrameworkCore;
using PaltformService.Data;
using PaltformService.Data.DataPreparation;
using PaltformService.Data.Interfaces;
using PaltformService.Data.Repositories;
using PaltformService.SyncDataServices.Http;
using PaltformService.SyncDataServices.Http.Interfaces;
using PlatformService.AsyncDataServices;
using PlatformService.AsyncDataServices.Interfaces;
using PlatformService.SyncDataServices.Grpc;

var builder = WebApplication.CreateBuilder(args);

//DB connection
if (builder.Environment.IsDevelopment())
{
    System.Console.WriteLine("--> Using InMem DB");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("PlatformSrvcInMemory"));
}
else
{
    System.Console.WriteLine("--> Using SqlServer DB");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformConnection")));
}

//DI
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddGrpc();

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
app.MapGrpcService<GrpcPlatformService>();
app.MapGet("/protos/platforms.proto", async context =>
{
    await context.Response.WriteAsync(System.IO.File.ReadAllText("Protos/platforms.proto"));
});

DataPreparation.Populate(app, app.Environment.IsProduction());
System.Console.WriteLine($"--> CommandService endpoint: {builder.Configuration["CommandServiceURL"]}");

app.Run();
