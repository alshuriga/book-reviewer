using System.ComponentModel.DataAnnotations;
using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Exceptions;

namespace BookReviewer.Shared.MassTransit;
public static class Extensions
{
    public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddMassTransit(opts =>
        {

            opts.AddConsumers(Assembly.GetEntryAssembly());

            opts.UsingRabbitMq((context, cfg) =>
            {
                var settings = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
                
                cfg.UseMessageRetry(conf =>
                {
                    conf.Handle<BrokerUnreachableException>();
                    conf.Interval(5, TimeSpan.FromSeconds(3));
                });

                cfg.Host(settings.Host);
                cfg.ConfigureEndpoints(context);
            });


        });
        return services;
    }
}