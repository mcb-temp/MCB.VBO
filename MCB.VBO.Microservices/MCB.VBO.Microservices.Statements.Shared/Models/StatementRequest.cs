using System;
using System.Text.Json.Serialization;

namespace MCB.VBO.Microservices.Statements.Shared.Models
{
    public class StatementRequest
    {
        [JsonIgnore]
        public Guid Id { get; set; }

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
