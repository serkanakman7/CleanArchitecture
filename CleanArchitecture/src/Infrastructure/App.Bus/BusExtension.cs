using App.Application.Contracts.ServiceBus;
using App.Bus.Consumers;
using App.Domain.Const;
using App.Domain.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Bus
{
    public static class BusExtension
    {
        public static IServiceCollection AddBusExt(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceBusOption = configuration.GetSection(nameof(ServiceBusOption)).Get<ServiceBusOption>();

            services.AddScoped<IServiceBus, ServiceBus>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<ProductAddedEventConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(serviceBusOption!.Url), host =>
                    {

                    });

                    cfg.ReceiveEndpoint(ServiceBusConst.ProductAddedEventQueueName, cfg =>
                    {
                        cfg.ConfigureConsumer<ProductAddedEventConsumer>(context);
                    });
                });


            });

            return services;
        }
    }
}
