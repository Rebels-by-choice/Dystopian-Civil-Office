using Dystopian_Civil_Office.Data;
using Dystopian_Civil_Office.Services;
using Dystopian_Civil_Office.Services.Read.Interfaces;
using Dystopian_Civil_Office.Services.Read.Impls;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Main service for data fetch and mapping to DTO
builder.Services.AddScoped<OfficeQueryService>();

// Services for viewing data
builder.Services.AddScoped<IMarriageService, MarriageService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IDeathRecordService, DeathRecordService>();
builder.Services.AddScoped<IBirthRecordService, BirthRecordService>();
builder.Services.AddScoped<IPersonAddressService, PersonAddressService>();


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

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

app.Run();
