using MCB.VBO.Microservices.Statements.Shared.Interfaces;
using MCB.VBO.Microservices.Statements.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace MCB.VBO.Microservices.Statements.Shared.Repositories
{
    public class StatementRepository : IStatementRepository
    {
        private string _dbPath { get; }

        public StatementRepository()
        {
            _dbPath = Path.Combine(AppContext.BaseDirectory, "db");

            if (!Directory.Exists(_dbPath))     // Create the log directory if it doesn't exist
                Directory.CreateDirectory(_dbPath);
        }

        public StatementData Create(StatementRequest request)
        {
            StatementData statement = new StatementData();

            statement.Id = Guid.NewGuid();
            statement.Status = StatusEnum.New;
            statement.FromDate = request.FromDate;
            statement.TillDate = request.TillDate;
            statement.AccountName = request.AccountName;
            statement.AccountNumber = request.AccountNumber;
            statement.RequestDate = DateTime.Now;

            Save(statement);

            return statement;
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

        public List<StatementData> ListAll()
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
