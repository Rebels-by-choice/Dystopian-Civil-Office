using Dystopian_Civil_Office.DataSource;
using Dystopian_Civil_Office.Services;
using Dystopian_Civil_Office.Services.Read.Interfaces;
using Dystopian_Civil_Office.Services.Read.Impls;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Dystopian_Civil_Office.Services.Write.Impls;
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
builder.Services.AddScoped<IMarriageReadService, MarriageReadService>();
builder.Services.AddScoped<IPersonReadService, PersonReadService>();
builder.Services.AddScoped<IDocumentReadService, DocumentReadService>();
builder.Services.AddScoped<IDeathRecordReadService, DeathRecordReadService>();
builder.Services.AddScoped<IBirthRecordService, BirthRecordReadService>();
builder.Services.AddScoped<IPersonAddressReadService, PersonAddressReadService>();

// Services for posting data
builder.Services.AddScoped<IDocumentWriteService, DocumentWriteService>();

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
