using Automatonymous;
using MCB.VBO.Microservices.Statement.Saga.Contracts;
using Microsoft.Extensions.Logging;

namespace MCB.VBO.Microservices.Statement.Saga
{
    public class StatementStateMachine : MassTransitStateMachine<StatementStateInstance>
    {
        private readonly ILogger<StatementStateMachine> _logger;

        public StatementStateMachine(ILogger<StatementStateMachine> logger)
        {
            _logger = logger;

            InstanceState(x => x.CurrentState);

            Event(() => StatementRequestReceived, x => x.CorrelateById(context => context.Message.Id));

            Event(() => StatementRequestProcessing, x => x.CorrelateById(context => context.Message.Id));

            Event(() => StatementRequestCompleted, x => x.CorrelateById(context => context.Message.Id));

            Initially(
                When(StatementRequestReceived)
                    //.Then(Processing)
                    //.ThenAsync(InitiateProcessing)
                    .TransitionTo(Received));
            
            During(Received,
                When(StatementRequestProcessing)
                //.Then()
                .TransitionTo(Processing)
                );

            During(Processing,
                When(StatementRequestProcessing)
                .TransitionTo(Processed));
        }

        public State Received { get; private set; }
        public State Processing { get; private set; }
        public State Processed { get; private set; }

        public Event<IStatementRequest> StatementRequestReceived { get; private set; }

        public Event<IStatementRequest> StatementRequestProcessing { get; private set; }

        public Event<IStatementRequest> StatementRequestCompleted { get; private set; }
    }
}
