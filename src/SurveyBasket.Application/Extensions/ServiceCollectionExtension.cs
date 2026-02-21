using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using SurveyBasket.Application.Behaviors;
using System.Reflection;

namespace SurveyBasket.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assamply = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assamply);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assamply);

        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(assamply);
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));

        return services;
    }
}