using System;

namespace MCB.VBO.Microservices.Statements.Shared.Models
{
    public class StatementTransaction
    {
        public DateTime Date { get; set; }

        public int DocumentNumber { get; set; }

        public string Inn { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }

        public string Client { get; set; }

        public string Description { get; set; }
    }
}