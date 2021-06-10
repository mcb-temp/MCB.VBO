using System;
using System.Collections.Generic;

namespace MCB.VBO.Microservices.RabbitMQ
{
    public interface IPublisher : IDisposable
    {
        void Publish(string message, string routingKey, IDictionary<string, object> messageAttributes, string timeToLive = null);
    }
}
