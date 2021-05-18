using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCB.VBO.Microservices.Statements.Shared.Models
{
    public class StatementRequest
    {
        /// <summary>
        /// Начиная с даты
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Заканчивая датой
        /// </summary>
        public DateTime TillDate { get; set; }

        /// <summary>
        /// Название организации (Фирма)
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Номер счета
        /// </summary>
        public string AccountNumber { get; set; }
    }
}
