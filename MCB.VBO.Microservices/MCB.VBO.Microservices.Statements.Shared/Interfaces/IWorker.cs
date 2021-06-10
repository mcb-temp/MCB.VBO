using System;
using System.Threading.Tasks;

namespace MCB.VBO.Microservices.Statements.Shared.Interfaces
{
    public interface IWorker
    {
        public Task Execute(Guid statementId);
    }
}
