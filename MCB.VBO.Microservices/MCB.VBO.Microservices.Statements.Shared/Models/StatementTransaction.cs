using System;

namespace MCB.VBO.Microservices.Statements.Shared.Models
{
    public class StatementTransaction
    {
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public string Recipient { get; set; }

        public string Sender { get; set; }
    }
}