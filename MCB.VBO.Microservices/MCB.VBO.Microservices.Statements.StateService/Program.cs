using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;

namespace MCB.VBO.Microservices.Statements.StateService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    services.AddMassTransit(x =>
                    {
                        x.AddSagaStateMachine<InvetsmentBuyerStateMachine, InvetsmentBuyerState>();

                        x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                        {
                            var host = cfg.Host(_configuration.GetSection("RabbitMQ").Get<RabbitMqConfig>());

                            cfg.ReceiveEndpoint(host, "broker", e =>
                            {
                                e.Durable = false;
                                e.ConfigureSaga<InvetsmentBuyerState>(provider);
                            });
                        }));
                    });
                });
    }
}
