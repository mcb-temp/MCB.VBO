using NUnit.Framework;
using MCB.VBO.Microservices.Statements.Repositories;
using System;
using System.Threading.Tasks;
using System.Threading;
using Moq;
using Microsoft.Extensions.Logging;
using MCB.VBO.Microservices.Statements.Controllers;
using MCB.VBO.Microservices.Statements.Shared.Interfaces;
using MCB.VBO.Microservices.Statements.Shared.Models;

namespace MCB.VBO.Microservices.Statements.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void StatementController_1()
        {
            var logger = new Mock<ILogger<StatementController>>();
            var repository = new Mock<IStatementRepository>();

            Guid newId = Guid.NewGuid();
            DateTime fromDate = new DateTime(2020, 01, 01);
            DateTime tillDate = new DateTime(2021, 01, 01);

            var willBeReturn = new StatementData()
            {
                Id = newId,
                Name = $"{newId}:{It.IsAny<DateTime>()}:{It.IsAny<DateTime>()}",
                Status = StatusEnum.New,
                fromDate = It.IsAny<DateTime>(),
                tillDate = It.IsAny<DateTime>()
            };
            /*repository.Setup(x => x.Create(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(willBeReturn);

            StatementController statementController = new StatementController(logger.Object, repository.Object);
            var result = statementController.Create(fromDate, tillDate);

            Assert.IsTrue(result == newId);*/
        }

        [Test]
        public void StatementRepository_WhenCreate_ReturnsGuidTest()
        {
            StatementRepository repository = new StatementRepository();

            DateTime fromDate = new DateTime(2020, 01, 01);
            DateTime tillDate = new DateTime(2021, 01, 01);

            var statement = repository.Create(fromDate, tillDate);

            Assert.NotNull(statement);
        }

        private StatementData Test(StatementData statement)
        {
            Assert.NotNull(statement);
            return statement;
        }

        [Test]
        [Timeout(5000)]
        public void StatementRepository_2()
        {
            StatementRepository repository = new StatementRepository();

            DateTime fromDate = new DateTime(2020, 01, 01);
            DateTime tillDate = new DateTime(2021, 01, 01);

            (Guid statement, Task<StatementData> task) = repository.Create(fromDate, tillDate);

            /*, (sd) =>
        {
            Assert.NotNull(sd);
            Assert.IsTrue(sd.fromDate == fromDate);
            Assert.IsTrue(sd.tillDate == tillDate);
            Assert.IsTrue(sd.Status == StatusEnum.Complete);
            Assert.IsTrue(sd.StatementTransactions.Count == 0);
            return sd;
        });*/

            task.Wait();
            StatementData sd = task.Result;
            Assert.NotNull(sd);
            Assert.IsTrue(sd.fromDate == fromDate);
            Assert.IsTrue(sd.tillDate == tillDate);
            Assert.IsTrue(sd.Status == StatusEnum.New);
            Assert.IsTrue(sd.StatementTransactions.Count > 0);

            //Assert.IsTrue(statement != StatusEnum.Complete);
            //Assert.IsTrue(statement.StatementTransactions.Count == 0);

            /*StatementRepository repository = new StatementRepository();

            DateTime fromDate = new DateTime(2020, 01, 01);
            DateTime tillDate = new DateTime(2021, 01, 01);

            var statement = repository.Create(fromDate, tillDate);


            Assert.NotNull(statement);
            Assert.IsTrue(statement.Id is Guid);

            return statement.Status;*/
        }

        public async Task<int> Return30()
        {
            return 30;
        }
    }
}