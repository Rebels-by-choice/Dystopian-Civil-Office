using Dystopian_Civil_Office.Services.Read.Impls;
using Dystopian_Civil_Office.Services.Read.Interfaces;
using Dystopian_Civil_Office.Services.Validation;
using Dystopian_Civil_Office.Services.Write.Impls;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using NodaTime;
using VMelnalksnis.PaperlessDotNet.DependencyInjection;

namespace Dystopian_Civil_Office.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection InfrastructureAddQueryServices(this IServiceCollection services)
    {
        services.AddScoped<OfficeQueryService>()
                .AddScoped<IQueryValidationService, QueryValidationService>();

        return services;
    }
    public static IServiceCollection InfrastructureAddReadServices(this IServiceCollection services)
    {
        services.AddScoped<IMarriageReadService, MarriageReadService>()
                .AddScoped<IPersonReadService, PersonReadService>()
                .AddScoped<IDocumentReadService, DocumentReadService>()
                .AddScoped<IDeathRecordReadService, DeathRecordReadService>()
                .AddScoped<IBirthRecordService, BirthRecordReadService>()
                .AddScoped<IPersonAddressReadService, PersonAddressReadService>()
                .AddSingleton<IApiStatsService, ApiStatsService>();

        return services;
    }
    public static IServiceCollection InfrastructureAddWriteServices(this IServiceCollection services)
    {
        services
            .AddScoped<IBirthRecordWriteService, BirthRecordWriteService>()
            .AddScoped<IDeathRecordWriteService, DeathRecordWriteService>()
            .AddScoped<IDocumentWriteService, DocumentWriteService>()
            .AddScoped<IMarriageWriteService, MarriageWriteService>()
            .AddScoped<IPersonAddressWriteService, PersonAddressWriteService>()
            .AddScoped<IPersonWriteService, PersonWriteService>();

        return services;
    }

    public static IServiceCollection InfrastructureAddPaperless(this IServiceCollection services)
    {
        services
            .AddSingleton<IClock>(SystemClock.Instance)
            .AddSingleton(DateTimeZoneProviders.Tzdb)
            .AddPaperlessDotNet();

        return services;
    }
}