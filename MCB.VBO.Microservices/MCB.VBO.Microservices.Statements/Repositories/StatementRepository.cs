using MCB.VBO.Microservices.Statements.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MCB.VBO.Microservices.Statements.Repositories
{
    public class StatementRepository
    {
        private string _dbPath { get; }

        //public Timer Timer { get; set; }

        public StatementRepository()
        {
            _dbPath = Path.Combine(AppContext.BaseDirectory, "db");

            if (!Directory.Exists(_dbPath))     // Create the log directory if it doesn't exist
                Directory.CreateDirectory(_dbPath);

            //var autoEvent = new AutoResetEvent(false);
            //Timer = new Timer(ProcessStatement, autoEvent, 20000, (int)(60000 * 0.25));
        }

        public void ProcessStatement(Object stateInfo)
        {
            var files = Directory.GetFiles(_dbPath);
            foreach (var file in files)
            {
                string statementJson = File.ReadAllText(file);
                StatementData sd = JsonConvert.DeserializeObject<StatementData>(statementJson);

                switch (sd.Status)
                {
                    case StatusEnum.New:
                        sd.Status = StatusEnum.InProgress;
                        break;

                    case StatusEnum.InProgress:
                        sd.Status = StatusEnum.Complete;

                        TimeSpan ts = sd.tillDate - sd.fromDate;
                        double days = ts.TotalDays >= 1 ? ts.TotalDays : 1;

                        Random r = new Random(DateTime.Now.Millisecond);
                        for (int i = 0; i <= days; i++)
                        {
                            StatementTransaction st = new StatementTransaction();
                            st.Amount = r.Next(0, 1000000);
                            st.Date = sd.fromDate.AddDays(i);
                            st.Recipient = $"{r.Next(1000000),6}{r.Next(1000000),6}";
                            st.Sender = $"{r.Next(1000000),6}{r.Next(1000000),6}";

                            sd.StatementTransactions.Add(st);
                        }
                        break;
                }

                Update(sd);
            }
        }

        public StatementData Create(DateTime fromDate, DateTime tillDate)
        {
            StatementData statement = new StatementData();

            statement.Id = Guid.NewGuid();
            statement.Name = $"{statement.Id}:{fromDate}:{tillDate}";
            statement.Status = StatusEnum.New;
            statement.fromDate = fromDate;
            statement.tillDate = tillDate;

            Save(statement);
            Task.Factory.StartNew(() =>
            {
                StatementProcessActionAsync(statement);
            });
            return statement;
        }

        private void StatementProcessActionAsync(StatementData sd)
        {
            Task.Delay(10000);
            switch (sd.Status)
            {
                case StatusEnum.New:
                    sd.Status = StatusEnum.InProgress;
                    break;

                case StatusEnum.InProgress:
                    sd.Status = StatusEnum.Complete;

                    TimeSpan ts = sd.tillDate - sd.fromDate;
                    double days = ts.TotalDays >= 1 ? ts.TotalDays : 1;

                    Random r = new Random(DateTime.Now.Millisecond);
                    for (int i = 0; i <= days; i++)
                    {
                        StatementTransaction st = new StatementTransaction();
                        st.Amount = r.Next(0, 1000000);
                        st.Date = sd.fromDate.AddDays(i);
                        st.Recipient = $"{r.Next(1000000),6}{r.Next(1000000),6}";
                        st.Sender = $"{r.Next(1000000),6}{r.Next(1000000),6}";

                        sd.StatementTransactions.Add(st);
                    }
                    break;
            }
            Update(sd);
            if (sd.Status != StatusEnum.Complete)
            {
                Task.Factory.StartNew(() =>
                {
                    StatementProcessActionAsync(sd);
                });
            }
        }

        public StatementData Retrive(Guid id)
        {
            return Load(id);
        }

        public void Update(StatementData statement)
        {
            Save(statement);
        }

        public void Delete(Guid id)
        {
            string filePath = Path.Combine(_dbPath, $"{id}.json");

            File.Delete(filePath);
        }

        public List<StatementData> GetHistory()
        {
            List<StatementData> data = new List<StatementData>();

            var files = Directory.GetFiles(_dbPath);
            foreach (var file in files)
            {
                string statementJson = File.ReadAllText(file);

                StatementData sd = JsonConvert.DeserializeObject<StatementData>(statementJson);

                data.Add(sd);
            }

            return data;
        }

        private void Save(StatementData statement)
        {
            string filePath = Path.Combine(_dbPath, $"{statement.Id}.json");

            string stetementJson = JsonConvert.SerializeObject(statement);

            File.WriteAllText(filePath, stetementJson);
        }

        private StatementData Load(Guid id)
        {
            string filePath = Path.Combine(_dbPath, $"{id}.json");

            string statementJson = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<StatementData>(statementJson);
        }
    }
}
