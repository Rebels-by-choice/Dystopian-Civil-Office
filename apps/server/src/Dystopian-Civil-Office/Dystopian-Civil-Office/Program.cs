using Dystopian_Civil_Office.DataSource;
using Dystopian_Civil_Office.Exceptions;
using Dystopian_Civil_Office.Middleware;
using Dystopian_Civil_Office.Services;
using Dystopian_Civil_Office.Services.Read.Interfaces;
using Dystopian_Civil_Office.Services.Read.Impls;
using Dystopian_Civil_Office.Services.Validation;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Dystopian_Civil_Office.Services.Write.Impls;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddExceptionHandler<ViewDataExceptionHandler>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<OfficeQueryService>();
builder.Services.AddScoped<IQueryValidationService, QueryValidationService>();

builder.Services.AddScoped<IMarriageReadService, MarriageReadService>();
builder.Services.AddScoped<IPersonReadService, PersonReadService>();
builder.Services.AddScoped<IDocumentReadService, DocumentReadService>();
builder.Services.AddScoped<IDeathRecordReadService, DeathRecordReadService>();
builder.Services.AddScoped<IBirthRecordService, BirthRecordReadService>();
builder.Services.AddScoped<IPersonAddressReadService, PersonAddressReadService>();
builder.Services.AddSingleton<IApiStatsService, ApiStatsService>();

builder.Services.AddScoped<IBirthRecordWriteService, BirthRecordWriteService>();
builder.Services.AddScoped<IDeathRecordWriteService, DeathRecordWriteService>();
builder.Services.AddScoped<IDocumentWriteService, DocumentWriteService>();
builder.Services.AddScoped<IMarriageWriteService, MarriageWriteService>();
builder.Services.AddScoped<IPersonAddressWriteService, PersonAddressWriteService>();
builder.Services.AddScoped<IPersonWriteService, PersonWriteService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<DatabaseExceptionHandlingMiddleware>();
app.UseMiddleware<DmlTrackerMiddleware>();

app.UseCors("FrontendPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();