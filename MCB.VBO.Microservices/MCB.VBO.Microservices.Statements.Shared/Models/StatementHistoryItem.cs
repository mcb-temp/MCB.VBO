using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCB.VBO.Microservices.Statements.Shared.Models
{
    public class StatementHistoryItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Status { get; set; }
    }
}
