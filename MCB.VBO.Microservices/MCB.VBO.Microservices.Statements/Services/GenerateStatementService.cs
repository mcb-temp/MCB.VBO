using MCB.VBO.Microservices.Statements.Services.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MCB.VBO.Microservices.Statements.Services
{
    public class GenerateStatementService
    {
        // Create a scheduler that uses five threads.
        private readonly LimitedConcurrencyLevelTaskScheduler _taskScheduler;
        public List<Task> Tasks { get; } = new List<Task>();

        // Create a TaskFactory and pass it our custom scheduler.
        private readonly TaskFactory _factory;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public GenerateStatementService()
        {
            _taskScheduler = new LimitedConcurrencyLevelTaskScheduler(5);
            _factory = new TaskFactory(_taskScheduler);
        }

        public Task Process(Action action)
        {
            Task task = _factory.StartNew(action, _cts.Token, TaskCreationOptions.LongRunning, _taskScheduler);
            Tasks.Add(task);
            return task;
        }
    }
}
