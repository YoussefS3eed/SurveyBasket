using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using SurveyBasket.Application.Behaviors;
using System.Reflection;

namespace SurveyBasket.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);

        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(assembly);
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));

        return services;
    }
}