using Dystopian_Civil_Office.DataSource;
using Dystopian_Civil_Office.Exceptions;
using Dystopian_Civil_Office.Middleware;
using Dystopian_Civil_Office.Services;
using Dystopian_Civil_Office.Services.Write.Impls;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var corsPolicy = "FrontendPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
    {
        policy
            .WithOrigins(
                "http://127.0.0.1:4200",
                "http://localhost:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddExceptionHandler<ViewDataExceptionHandler>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSnakeCaseNamingConvention());

builder.Services.InfrastructureAddQueryServices();
builder.Services.InfrastructureAddReadServices();
builder.Services.InfrastructureAddWriteServices();
builder.Services.InfrastructureAddPaperless();
builder.Services.AddScoped<ICaseService, CaseService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("FrontendPolicy", policy =>
//     {
//         policy.WithOrigins("http://localhost:4200")
//               .AllowAnyHeader()
//               .AllowAnyMethod();
//     });
// });

var app = builder.Build();

// Comment this if you want to update migrations manually
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

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