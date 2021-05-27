using MCB.VBO.Microservices.Statements.Shared.Interfaces;
using MCB.VBO.Microservices.Statements.Shared.Models;
using System;
using System.Threading.Tasks;

namespace MCB.VBO.Microservices.Statements.Actions
{
    public class StatementCreateAction : IProcessAction
    {
        private readonly IStatementRepository _repository;

        public StatementCreateAction(IStatementRepository repository)
        {
            _repository = repository;
        }

        public async Task Execute(Guid statementId)
        {
            var sd = _repository.Retrive(statementId);
            sd.Status = StatusEnum.InProgress;
            _repository.Update(sd);

            try
            {
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

                await Task.Delay(new Random(DateTime.Now.Millisecond).Next(10,20) * 1000);

                if (sd.AccountName == "Error")
                {
                    throw new Exception("This is error account");
                }

                sd.Status = StatusEnum.Complete;
                _repository.Update(sd);
            }
            catch (Exception)
            {
                var sdFailed = _repository.Retrive(statementId);
                sdFailed.Status = StatusEnum.Failed;
                _repository.Update(sdFailed);
            }
        }
    }
}
