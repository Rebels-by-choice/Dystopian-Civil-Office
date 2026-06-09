using Dystopian_Civil_Office.Services.Read.Impls;
using Dystopian_Civil_Office.Services.Read.Interfaces;
using Dystopian_Civil_Office.Services.Validation;
using Dystopian_Civil_Office.Services.Write.Impls;
using Dystopian_Civil_Office.Services.Write.Interfaces;

namespace Dystopian_Civil_Office.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection InfrastructureAddQueryServices(this IServiceCollection services)
    {
        services.AddScoped<OfficeQueryService>();
        services.AddScoped<IQueryValidationService, QueryValidationService>();

        return services;
    }
    public static IServiceCollection InfrastructureAddReadServices(this IServiceCollection services)
    {
        services.AddScoped<IMarriageReadService, MarriageReadService>();
        services.AddScoped<IPersonReadService, PersonReadService>();
        services.AddScoped<IDocumentReadService, DocumentReadService>();
        services.AddScoped<IDeathRecordReadService, DeathRecordReadService>();
        services.AddScoped<IBirthRecordService, BirthRecordReadService>();
        services.AddScoped<IPersonAddressReadService, PersonAddressReadService>();
        services.AddSingleton<IApiStatsService, ApiStatsService>();

        return services;
    }
    public static IServiceCollection InfrastructureAddWriteServices(this IServiceCollection services)
    {
        services.AddScoped<IBirthRecordWriteService, BirthRecordWriteService>();
        services.AddScoped<IDeathRecordWriteService, DeathRecordWriteService>();
        services.AddScoped<IDocumentWriteService, DocumentWriteService>();
        services.AddScoped<IMarriageWriteService, MarriageWriteService>();
        services.AddScoped<IPersonAddressWriteService, PersonAddressWriteService>();
        services.AddScoped<IPersonWriteService, PersonWriteService>();

        return services;
    }
}