using System;
using Microsoft.Extensions.Configuration;

namespace MCB.VBO.Microservices.RabbitMQ.Configuration
{
    public static class RabbitMQConfigExtension
    {
        public static RabbitMQConfig GetRabbitConfig(this IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var serviceConfig = new RabbitMQConfig
            {
                Server = configuration.GetValue<string>("RabbitMQ:Server"),
                Port = configuration.GetValue<int>("RabbitMQ:Port"),
                User = configuration.GetValue<string>("RabbitMQ:User"),
                Password = configuration.GetValue<string>("RabbitMQ:Password")
            };

            return serviceConfig;
        }
    }
}
