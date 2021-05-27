using MCB.VBO.Microservices.RabbitMQ;
using MCB.VBO.Microservices.Statements.Actions;
using MCB.VBO.Microservices.Statements.Shared.Interfaces;
using MCB.VBO.Microservices.Statements.Shared.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MCB.VBO.Microservices.Statements.Listeners
{
    public class StatementCreateListener : IHostedService
    {
        private readonly ISubscriber _subscriber;
        private readonly IProcessAction _action;

        public StatementCreateListener(ISubscriber subscriber, StatementCreateAction action)
        {
            _subscriber = subscriber;
            _action = action;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriber.Subscribe(Subscribe);
            return Task.CompletedTask;
        }

        private bool Subscribe(string message, IDictionary<string, object> header)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<StatementRequest>(message);
                _action.Execute(request.Id).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
