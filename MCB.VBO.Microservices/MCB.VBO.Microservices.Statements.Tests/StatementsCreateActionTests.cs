using NUnit.Framework;
using System;
using Moq;
using MCB.VBO.Microservices.Statements.Shared.Interfaces;
using MCB.VBO.Microservices.Statements.Shared.Models;
using MCB.VBO.Microservices.Statements.Actions;

namespace MCB.VBO.Microservices.Statements.Tests
{
    public class StatementsCreateActionTests
    {
        //TODO: Переписать на тест сервисов, а не контроллера
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void StatementsCreateAction_ThenCreate_Success()
        {
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
                StatementTransactions = new System.Collections.Generic.List<StatementTransaction>(),
                Name = statementRequest.AccountName,
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

            StatementCreateAction action = new StatementCreateAction(repository.Object);

            action.Execute(requestId).GetAwaiter().GetResult();

            var result = repository.Object.Retrive(requestId);

            Assert.AreEqual(result.Status, StatusEnum.Complete);
            Assert.IsTrue(result.StatementTransactions.Count > 0);
        }

        [Test]
        public void StatementsCreateAction_ThenCreate_Failed()
        {
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
                StatementTransactions = new System.Collections.Generic.List<StatementTransaction>(),
                Name = statementRequest.AccountName,
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


            StatementCreateAction action = new StatementCreateAction(repository.Object);

            Assert.Throws<NullReferenceException>(() => action.Execute(Guid.NewGuid()).GetAwaiter().GetResult());

            var result = repository.Object.Retrive(requestId);

            Assert.AreEqual(result.Status, StatusEnum.New);
            Assert.IsTrue(result.StatementTransactions.Count == 0);
        }
    }
}