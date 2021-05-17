using MCB.VBO.Microservices.Statements.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCB.VBO.Microservices.Statements.Shared.Interfaces
{
    public interface IStatementRepository
    {
        public (Guid, Task<StatementData>) Create(DateTime fromDate, DateTime tillDate);

        public StatementData Retrive(Guid id);

        public void Update(StatementData statement);

        public void Delete(Guid id);

        public List<StatementData> ListAll();
    }
}
