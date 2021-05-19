using MCB.VBO.Microservices.Statements.Services.Threading;
using MCB.VBO.Microservices.Statements.Shared.Interfaces;
using MCB.VBO.Microservices.Statements.Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MCB.VBO.Microservices.Statements.Services
{
    public class GenerateStatementService
    {
        // Create a scheduler that uses five threads.
        private readonly LimitedConcurrencyLevelTaskScheduler _taskScheduler;

        // Create a TaskFactory and pass it our custom scheduler.
        private readonly TaskFactory _factory;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private readonly IStatementRepository _repository;

        public GenerateStatementService(IStatementRepository repository)
        {
            _repository = repository;
            _taskScheduler = new LimitedConcurrencyLevelTaskScheduler(5);
            _factory = new TaskFactory(_taskScheduler);
        }

        public Task Process(StatementData sd)
        {
            return _factory.StartNew(() => { StatementProcessActionAsync(sd); }, _cts.Token, TaskCreationOptions.LongRunning, _taskScheduler);
        }

        private Task StatementProcessActionAsync(StatementData sd)
        {
            Task.Delay(new Random(DateTime.Now.Millisecond).Next(0, 3) * 1000);
            sd.Status = StatusEnum.InProgress;
            _repository.Update(sd);

            TimeSpan ts = sd.TillDate - sd.FromDate;
            double days = ts.TotalDays >= 1 ? ts.TotalDays : 1;

            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i <= days; i++)
            {
                StatementTransaction st = new StatementTransaction();
                st.Amount = r.Next(0, 1000000);
                st.Date = sd.FromDate.AddDays(i);
                st.Recipient = $"{r.Next(1000000),6}{r.Next(1000000),6}";
                st.Sender = $"{r.Next(1000000),6}{r.Next(1000000),6}";

                sd.StatementTransactions.Add(st);
            }

            Task.Delay(new Random(DateTime.Now.Millisecond).Next(0, 3) * 1000);
            sd.Status = StatusEnum.Complete;
            _repository.Update(sd);

            return Task.CompletedTask;
        }
    }
}
