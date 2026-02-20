using Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SurveyBasket.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        //services.AddAutoMapper(applicationAssembly);

        //services.AddValidatorsFromAssembly(applicationAssembly)
        //    .AddFluentValidationAutoValidation();

        //services.AddScoped<IUserContext, UserContext>();

        //services.AddHttpContextAccessor();
        return services;
    }
}