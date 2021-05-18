using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using MCB.VBO.Microservices.Statements.Repositories;
using System;
using System.Linq;
using MCB.VBO.Microservices.Statements.Shared.Models;
using MCB.VBO.TemplatesLib.Builders;
using MCB.VBO.TemplatesLib;
using System.Threading.Tasks;
using MCB.VBO.Microservices.Statements.Shared.Interfaces;
using MCB.VBO.Microservices.Statements.Services;
using System.Threading;
using MCB.VBO.Microservices.Statements.Extensions;

namespace MCB.VBO.Microservices.Statements.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatementController : ControllerBase
    {
        private readonly ILogger<StatementController> _logger;

        private readonly IStatementRepository _repository;

        private readonly GenerateStatementService _generateStatementService;

        public StatementController(ILogger<StatementController> logger, GenerateStatementService generateStatementService, IStatementRepository repository)
        {
            _logger = logger;
            _repository = repository;
            _generateStatementService = generateStatementService;
        }

        [HttpPost("create")]
        public Guid Create(StatementRequest request)
        {
            StatementData statement = _repository.Create(request);

            //TODO
            _generateStatementService.Process(() => { StatementProcessActionAsync(statement); });

            return statement.Id;
        }

        private Task StatementProcessActionAsync(StatementData sd)
        {
            Task.Delay(new Random(DateTime.Now.Millisecond).Next(0, 3) * 1000);
            sd.Status = StatusEnum.InProgress;
            _repository.Update(sd);

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

            Task.Delay(new Random(DateTime.Now.Millisecond).Next(0, 3) * 1000);
            sd.Status = StatusEnum.Complete;
            _repository.Update(sd);

            return Task.CompletedTask;
        }

        [HttpGet]
        public StatementData GetStatement(Guid statementId)
        {
            return _repository.Retrive(statementId);
        }

        [HttpGet("transactions")]
        [ProducesResponseType(typeof(IList<StatementTransaction>), 200)]
        public IActionResult GetStatementTransactions(Guid statementId, int page, int limit)
        {
            var pagedTransactionsModel = _repository.Retrive(statementId)
                .StatementTransactions
                .Paginate(page, limit);

            return Ok(pagedTransactionsModel);
        }

        [HttpGet("status")]
        public int GetStatus(Guid id)
        {
            StatementData data = _repository.Retrive(id);

            Random r = new Random(DateTime.Now.Millisecond);
            if (data.Status == StatusEnum.New && r.Next(0, 9) > 5)
            {
                data.Status = StatusEnum.InProgress;
            }
            else if (data.Status == StatusEnum.InProgress && r.Next(0, 9) > 7)
            {
                data.Status = StatusEnum.Complete;
            }
            _repository.Update(data);

            return (int)data.Status;
        }

        [HttpGet("download")]
        public async Task<FileStreamResult> GetStatementFile(Guid id)
        {
            WordDocumentBuilder builder = new WordDocumentBuilder();
            DocumentDirector director = new DocumentDirector();

            StatementData data = _repository.Retrive(id);

            director.CreateWordDoc(builder, data);
            var mem = builder.GetResult();

            return new FileStreamResult(mem, "application/octet-stream")
            {
                FileDownloadName = $"{id}.docx"
            };
        }

        [HttpGet("history")]
        public IEnumerable<StatementHistoryItem> GetHistory()
        {
            var statementsList = _repository.ListAll();

            return statementsList.Select(s => new StatementHistoryItem { Id = s.Id, Name = s.Name, Status = (int)s.Status });
        }
    }
}
