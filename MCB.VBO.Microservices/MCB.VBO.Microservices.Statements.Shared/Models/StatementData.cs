using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCB.VBO.Microservices.Statements.Shared.Models
{
    public class StatementData
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<StatementTransaction> StatementTransactions { get; set; }

        public StatusEnum Status { get; set; }

        public DateTime fromDate { get; set; }

        public DateTime tillDate { get; set; }

        public StatementData()
        {
            StatementTransactions = new List<StatementTransaction>();
        }
    }
}
