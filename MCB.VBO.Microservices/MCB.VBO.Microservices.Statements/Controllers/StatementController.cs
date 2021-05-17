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

namespace MCB.VBO.Microservices.Statements.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatementController : ControllerBase
    {
        private readonly ILogger<StatementController> _logger;

        private readonly IStatementRepository _repository;

        public StatementController(ILogger<StatementController> logger, IStatementRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpPost("create")]
        public Guid Create(DateTime fromDate, DateTime tillDate)
        {
            return _repository.Create(fromDate, tillDate).Item1;
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
