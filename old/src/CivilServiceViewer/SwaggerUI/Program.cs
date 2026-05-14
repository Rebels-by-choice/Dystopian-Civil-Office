using CivilServiceViewer.Models;
using CivilServiceViewer.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// shared services
builder.Services.AddScoped<IDataOperationsService, DataFetchService>();
builder.Services.AddScoped<IVMarriageService, VMarriageService>();
builder.Services.AddScoped<IVPersonFamilyService, VPersonFamilyService>();
builder.Services.AddScoped<IVPersonAddressStatusService, VPersonAddressStatusService>();
builder.Services.AddScoped<IVPersonBirthService, VPersonBirthService>();
builder.Services.AddScoped<IVPersonDeathService, VPersonDeathService>();

// controllers from CivilServiceViewer
builder.Services.AddControllers()
    .AddApplicationPart(typeof(CivilServiceViewer.Controllers.VMarriageController).Assembly);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
