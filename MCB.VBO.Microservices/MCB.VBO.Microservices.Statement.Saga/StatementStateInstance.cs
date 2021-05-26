using Automatonymous;
using System;

namespace MCB.VBO.Microservices.Statement.Saga
{
    public class StatementStateInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public string CurrentState { get; set; }
    }
}
