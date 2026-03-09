using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using SurveyBasket.Application.Common.Behaviors;
using System.Reflection;

namespace SurveyBasket.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(CacheInvalidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);

        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(assembly);
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));

        return services;
    }
}