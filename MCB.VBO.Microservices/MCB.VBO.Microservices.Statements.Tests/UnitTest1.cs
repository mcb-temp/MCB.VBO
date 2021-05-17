using NUnit.Framework;
using MCB.VBO.Microservices.Statements.Repositories;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace MCB.VBO.Microservices.Statements.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void StatementRepository_WhenCreate_ReturnsGuidTest()
        {
            StatementRepository repository = new StatementRepository();

            DateTime fromDate = new DateTime(2020, 01, 01);
            DateTime tillDate = new DateTime(2021, 01, 01);

            var statement = repository.Create(fromDate, tillDate);

            Assert.NotNull(statement);
            Assert.IsTrue(statement.Id is Guid);
        }

        [Test(ExpectedResult = 30)]
        public async Task<int> StatementRepository_2()
        {
            StatementRepository repository = new StatementRepository();

            DateTime fromDate = new DateTime(2020, 01, 01);
            DateTime tillDate = new DateTime(2021, 01, 01);

            var statement = repository.Create(fromDate, tillDate);

            Assert.NotNull(statement);
            Assert.IsTrue(statement.Id is Guid);

            Thread.Sleep(1500);

            statement = repository.Retrive(statement.Id);
            return (int)statement.Status;

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