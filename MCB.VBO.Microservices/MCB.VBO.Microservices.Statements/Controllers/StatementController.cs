﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using System.Linq;
using MCB.VBO.Microservices.Statements.Shared.Models;
using MCB.VBO.TemplatesLib.Builders;
using MCB.VBO.TemplatesLib;
using System.Threading.Tasks;
using MCB.VBO.Microservices.Statements.Shared.Interfaces;
using MCB.VBO.Microservices.Statements.Extensions;
using MCB.VBO.Microservices.RabbitMQ;
using Newtonsoft.Json;

namespace MCB.VBO.Microservices.Statements.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatementsController : ControllerBase
    {
        private readonly ILogger<StatementsController> _logger;
        private readonly IStatementRepository _repository;
        private readonly IPublisher _publisher;

        public StatementsController(ILogger<StatementsController> logger, IStatementRepository repository, IPublisher publisher)
        {
            _logger = logger;
            _repository = repository;
            _publisher = publisher;
        }

        [HttpPost("create")]
        public Guid Create(StatementRequest request)
        {
            _logger.LogInformation($"StatementRequest create {request.AccountNumber}, {request.FromDate},{request.TillDate}");
            StatementData statement = _repository.Create(request);
            _logger.LogInformation($"StatementRequest Id = {statement.Id}");

            _publisher.Publish(JsonConvert.SerializeObject(statement), "statements.created", null);

            return statement.Id;
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

            return statementsList.Select(s => new StatementHistoryItem
            {
                Id = s.Id,
                AccountName = s.AccountName,
                AccountNumber = s.AccountNumber,
                FromDate = s.FromDate,
                TillDate = s.TillDate,
                RequestDate = s.RequestDate,
                Status = (int)s.Status
            });
        }
    }
}
