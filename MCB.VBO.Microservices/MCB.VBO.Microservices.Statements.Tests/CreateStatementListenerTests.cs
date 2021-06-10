using MCB.VBO.Microservices.RabbitMQ;
using MCB.VBO.Microservices.Statements.Actions;
using MCB.VBO.Microservices.Statements.Listeners;
using MCB.VBO.Microservices.Statements.Shared.Interfaces;
using MCB.VBO.Microservices.Statements.Shared.Models;
using Moq;
using NUnit.Framework;
using RabbitMQ.Client;
using RabbitMQ.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MCB.VBO.Microservices.Statements.Tests
{
    public class StatementCreateListenerTests
    {
        RabbitServer _rabbitServer;

        [SetUp]
        public void Setup()
        {
            //_rabbitServer = new RabbitServer();
            //ConfigureQueueBinding(_rabbitServer, "statement_exchange", "statements_response");
        }

        //[Test]
        public void Test()
        {
            //TODO: Посмотреть другие варианты. Класс ConnectionFactory sealed. Обертка больше не работает похоже.
            _rabbitServer = new RabbitServer();

            Guid requestId = Guid.NewGuid();

            DateTime fromDate = new DateTime(2020, 01, 01);
            DateTime tillDate = new DateTime(2021, 01, 01);

            var statementRequest = new StatementRequest()
            {
                FromDate = fromDate,
                TillDate = tillDate,
                AccountName = "Ok",
                AccountNumber = "1234 5678 9012 3456"
            };

            var statementData = new StatementData()
            {
                Id = requestId,
                StatementTransactions = new List<StatementTransaction>(),
                Status = StatusEnum.New,
                FromDate = statementRequest.FromDate,
                TillDate = statementRequest.TillDate,
                AccountName = statementRequest.AccountName,
                AccountNumber = statementRequest.AccountNumber
            };

            var repository = new Mock<IStatementRepository>();

            repository.Setup(x => x.Create(statementRequest)).Returns(statementData);

            repository.Setup(x => x.Retrive(requestId)).Returns(statementData);

            repository.Setup(x => x.Update(It.IsAny<StatementData>())).Callback<StatementData>(sd =>
            {
                repository.Setup(x => x.Retrive(requestId)).Returns(sd);
            });

            ConnectionFactory connectionFactory = new FakeConnectionFactory(_rabbitServer);
            ConnectionProvider connectionProvider = new ConnectionProvider(connectionFactory);

            Subscriber subscriber = new Subscriber(connectionProvider,
                "statements_exchange",
                "statements_response",
                "statements.created",
                ExchangeType.Topic);

            StatementCreateAction action = new StatementCreateAction(repository.Object);
            StatementCreateListener listener = new StatementCreateListener(subscriber, action);

            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            listener.StartAsync(token);
            Thread.Sleep(2000);
            listener.StopAsync(token);

            var result = repository.Object.Retrive(requestId);
        }

        private void ConfigureQueueBinding(RabbitServer rabbitServer, string exchangeName, string queueName)
        {
            var connectionFactory = new FakeConnectionFactory(rabbitServer);
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);

                channel.QueueBind(queueName, exchangeName, null);
            }
        }

        private static void SendMessage(RabbitServer rabbitServer, string exchange, string message, IBasicProperties basicProperties = null)
        {
            var connectionFactory = new FakeConnectionFactory(rabbitServer);

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var messageBody = Encoding.ASCII.GetBytes(message);
                channel.BasicPublish(exchange: exchange, routingKey: null, mandatory: false, basicProperties: basicProperties, body: messageBody);
            }
        }
    }
}
