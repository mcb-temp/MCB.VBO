using RabbitMQ.Client;
using System;

namespace MCB.VBO.Microservices.RabbitMQ
{
    public interface IConnectionProvider : IDisposable
    {
        IConnection GetConnection();
    }
}
