using MCB.VBO.Microservices.Statements.Shared.Interfaces;
using MCB.VBO.Microservices.Statements.Shared.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MCB.VBO.Microservices.Statements.WorkerHost.Worker
{
    public class StatementCreateWorker : IWorker
    {
        private readonly IStatementRepository _repository;

        public StatementCreateWorker(IStatementRepository repository)
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
                    int maxInDay = r.Next(0, 4);
                    for (int j = 0; j <= maxInDay; j++)
                    {
                        StatementTransaction st = new StatementTransaction();
                        if (r.Next(0, 3) >= 2)
                        {
                            st.Credit = r.Next(0, 1000000);
                        }
                        else
                        {
                            st.Debit = r.Next(0, 1000000);
                        }

                        st.Date = sd.FromDate.AddDays(i);
                        st.Inn = r.Next(1000000, 3333333).ToString();
                        st.Description = "Описание платежа";
                        st.Client = "Название получателя";
                        st.DocumentNumber = r.Next(1234567, 87654321);

                        sd.StatementTransactions.Add(st);
                    }
                }

                sd.BalanceIncome = r.Next(1000000, 2000000);

                sd.TurnoverCredit = sd.StatementTransactions.Where(s => s.Credit > 0).Sum(s => s.Credit);
                sd.TurnoverDebit = sd.StatementTransactions.Where(s => s.Debit > 0).Sum(s => s.Debit);

                sd.BalanceOutcome = sd.BalanceIncome + sd.TurnoverCredit - sd.TurnoverDebit;

                if (sd.BalanceOutcome < 0)
                {
                    sd.BalanceIncome = sd.BalanceIncome - sd.BalanceOutcome;
                    sd.BalanceOutcome = sd.BalanceIncome + sd.TurnoverCredit - sd.TurnoverDebit;
                }

                sd.LasActionDate = sd.StatementTransactions.Max(s => s.Date);

                //await Task.Delay(new Random(DateTime.Now.Millisecond).Next(10, 20) * 1000);

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
