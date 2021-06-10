using System;
using System.Collections.Generic;

namespace MCB.VBO.Microservices.Statements.Shared.Models
{
    public class StatementData
    {
        public Guid Id { get; set; }

        public List<StatementTransaction> StatementTransactions { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public StatusEnum Status { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime TillDate { get; set; }

        public DateTime RequestDate { get; set; }

        public decimal BalanceIncome { get; set; }

        public decimal BalanceOutcome { get; set; }

        public decimal TurnoverDebit { get; set; }

        public decimal TurnoverCredit { get; set; }

        public DateTime LasActionDate { get; set; }

        public StatementData()
        {
            StatementTransactions = new List<StatementTransaction>();
        }
    }
}
