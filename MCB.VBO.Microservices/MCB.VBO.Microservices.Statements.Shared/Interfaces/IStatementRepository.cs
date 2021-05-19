using MCB.VBO.Microservices.Statements.Shared.Models;
using System;
using System.Collections.Generic;

namespace MCB.VBO.Microservices.Statements.Shared.Interfaces
{
    public interface IStatementRepository
    {
        public StatementData Create(StatementRequest request);

        public StatementData Retrive(Guid id);

        public void Update(StatementData statement);

        public void Delete(Guid id);

        public List<StatementData> ListAll();
    }
}
