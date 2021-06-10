using MCB.VBO.Microservices.RabbitMQ;
using MCB.VBO.Microservices.Statements.Shared.Interfaces;
using MCB.VBO.Microservices.RabbitMQ.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System;
using MCB.VBO.Microservices.Statements.Shared.Repositories;

namespace MCB.VBO.Microservices.Statements
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();

            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(2);
                options.Predicate = (check) => check.Tags.Contains("ready");
            });

            //
            services.AddSingleton<IStatementRepository, StatementRepository>();

            //
            ConfigureRabbit(services);

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MCB.VBO.Microservices.Statements", Version = "v1" });
            });
        }


        private void ConfigureRabbit(IServiceCollection services)
        {
            // RabbitMQ
            var rabbitConfig = Configuration.GetRabbitConfig();

            services.AddSingleton<IConnectionProvider>(new ConnectionProvider($"amqp://{rabbitConfig.User}:{rabbitConfig.Password}@{rabbitConfig.Server}:{rabbitConfig.Port}"));

            services.AddSingleton<IPublisher>(x => new Publisher(x.GetService<IConnectionProvider>(),
                    "statements_exchange",
                    ExchangeType.Topic));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MCB.VBO.Microservices.Statements v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions());

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions());
            });
        }
    }
}
