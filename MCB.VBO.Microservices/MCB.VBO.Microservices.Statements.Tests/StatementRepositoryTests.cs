using NUnit.Framework;
using MCB.VBO.Microservices.Statements.Repositories;
using System;
using Moq;
using Microsoft.Extensions.Logging;
using MCB.VBO.Microservices.Statements.Controllers;
using MCB.VBO.Microservices.Statements.Shared.Interfaces;
using MCB.VBO.Microservices.Statements.Shared.Models;

namespace MCB.VBO.Microservices.Statements.Tests
{
    public class StatementRepositoryTests
    {
        //TODO: Переписать на тест сервисов, а не контроллера
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void StatementController_1()
        {
            var logger = new Mock<ILogger<StatementsController>>();
            var repository = new Mock<IStatementRepository>();

            Guid newId = Guid.NewGuid();
            DateTime fromDate = new DateTime(2020, 01, 01);
            DateTime tillDate = new DateTime(2021, 01, 01);

            var statementRequest = new StatementRequest()
            {
                FromDate = fromDate,
                TillDate = tillDate,
                AccountName = "Test Acc",
                AccountNumber = "1234 5678 9012 3456"
            };

            repository.Setup(x => x.Create(statementRequest)).Returns(new StatementData()
            {
                Id = newId,
                StatementTransactions = new System.Collections.Generic.List<StatementTransaction>(),
                Name = statementRequest.AccountName,
                Status = StatusEnum.New,
                FromDate = statementRequest.FromDate,
                TillDate = statementRequest.TillDate
            });

            /* generateStatementService = new GenerateStatementService(repository.Object);
            StatementRepository statementRepository = new StatementRepository();

            StatementController statementController = new StatementController(logger.Object, generateStatementService, statementRepository);
            var result = statementController.Create(statementRequest);

            int status = statementController.GetStatus(result);
            StatementData statement = statementController.GetStatement(result);

            Assert.AreEqual(status, 30);

            Assert.NotNull(statement, "StatementData is null");
            Assert.AreEqual(statement.FromDate, statementRequest.FromDate);
            Assert.AreEqual(statement.TillDate, statementRequest.TillDate);
            Assert.IsTrue(statement.Status == StatusEnum.Complete);
            Assert.IsTrue(statement.StatementTransactions.Count > 0);*/
        }

        [Test]
        public void StatementRepository_WhenCreate_ReturnsGuidTest()
        {
            StatementRepository repository = new StatementRepository();

            StatementRequest request = new StatementRequest()
            {
                FromDate = new DateTime(2020, 01, 01),
                TillDate = new DateTime(2021, 01, 01),
                AccountName = "Ололо",
                AccountNumber = "1234 5678 9012 1234"
            };

            var statementId = repository.Create(request);

            Assert.NotNull(statementId);
        }

        [Test]
        //[Timeout(5000)]
        public void StatementRepository_CreateRequest_ReturnStatementData()
        {
            StatementRepository repository = new StatementRepository();

            StatementRequest request = new StatementRequest()
            {
                FromDate = new DateTime(2020, 01, 01),
                TillDate = new DateTime(2021, 01, 01),
                AccountName = "ОЛОЛО ТЕСТ",
                AccountNumber = "1234 5678 9012 1234"
            };

            StatementData sd = repository.Create(request);

            Assert.AreNotEqual(sd.Id, Guid.Empty, "Ïóñòîé Guid");
            Assert.NotNull(sd, "StatementData is null");
            Assert.AreEqual(sd.FromDate, request.FromDate, "Íå âåðíàÿ äàòà íà÷àëà ïåðèîäà");
            Assert.AreEqual(sd.TillDate, request.TillDate, "Íå âåðíàÿ äàòà çàâåðøåíèÿ ïåðèîäà");
            Assert.IsTrue(sd.Status == StatusEnum.New, "Íå ôèíàëüíûé ñòàòóñ âûïèñêè");
            Assert.IsTrue(sd.StatementTransactions.Count == 0, "Îòêóäà òóò òðàíçàêöèè");
        }
    }
}