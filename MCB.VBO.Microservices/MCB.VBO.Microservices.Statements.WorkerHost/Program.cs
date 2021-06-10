using MCB.VBO.Microservices.RabbitMQ;
using MCB.VBO.Microservices.RabbitMQ.Configuration;
using MCB.VBO.Microservices.Statements.Shared.Interfaces;
using MCB.VBO.Microservices.Statements.Shared.Repositories;
using MCB.VBO.Microservices.Statements.WorkerHost.Listeners;
using MCB.VBO.Microservices.Statements.WorkerHost.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

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
                    IConfiguration configuration = hostContext.Configuration;

                    services.AddSingleton<IStatementRepository, StatementRepository>();

                    //
                    var rabbitConfig = configuration.GetRabbitConfig();

                    services.AddSingleton<IConnectionProvider>(new ConnectionProvider($"amqp://{rabbitConfig.User}:{rabbitConfig.Password}@{rabbitConfig.Server}:{rabbitConfig.Port}"));

                    services.AddSingleton<ISubscriber>(x => new Subscriber(x.GetService<IConnectionProvider>(),
                       "statements_exchange",
                       "statements_response",
                       "statements.created",
                       ExchangeType.Topic));

                    services.AddSingleton<StatementCreateWorker>();

                    // Listeners
                    services.AddHostedService<StatementCreateListener>();
                });


    }
}
