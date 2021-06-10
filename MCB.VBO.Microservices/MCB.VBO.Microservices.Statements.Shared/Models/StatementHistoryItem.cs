using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCB.VBO.Microservices.Statements.Shared.Models
{
    public class StatementHistoryItem
    {
        public Guid Id { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime TillDate { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public DateTime RequestDate { get; set; }

        public int Status { get; set; }
    }
}
