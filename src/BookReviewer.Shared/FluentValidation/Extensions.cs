using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace BookReviewer.Shared.FluentValidation;

public static class Extensions
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetEntryAssembly());
        services.AddFluentValidationAutoValidation(config => {
            config.DisableBuiltInModelValidation = true;
            config.ValidationStrategy = SharpGrip.FluentValidation.AutoValidation.Mvc.Enums.ValidationStrategy.All;
        });
        return services;
    }
}