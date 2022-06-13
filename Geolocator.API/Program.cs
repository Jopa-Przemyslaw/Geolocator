using Geolocator.Client;
using Geolocator.Client.Abstractions;
using Geolocator.Data;
using Geolocator.Data.Abstractions;
using Geolocator.Data.Helpers;
using Geolocator.Data.Repositories;
using Geolocator.Services;
using Geolocator.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Console.WriteLine($"--> GeolocatorAPI is in the {builder.Environment.EnvironmentName} environment.");
if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using SqlServer Db... ");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlDatabaseConnection")));
}
else
{
    Console.WriteLine("--> Using InMem Db... ");
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// my dependencies registration
builder.Services.AddScoped<IIpInfoRepository, IpInfoRepository>();
builder.Services.AddScoped<IGeolocationService, GeolocationService>();
builder.Services.AddHttpClient<IIpInfoClient, IpInfoClient>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

PrepDb.PrepPopulation(app, builder.Environment.IsProduction());

app.Run();