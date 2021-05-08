using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace MCB.VBO.Microservices.Templates.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TemplatesController : ControllerBase
    {
        private readonly ILogger<TemplatesController> _logger;

        public TemplatesController(ILogger<TemplatesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            // Путь к файлу
            string file_path = Path.Combine(Environment.CurrentDirectory, "temp.pdf");
            // Тип файла - content-type
            string file_type = "application/pdf";
            // Имя файла - необязательно
            string file_name = "book.pdf";

            return PhysicalFile(file_path, file_type, file_name);
        }
    }
}
