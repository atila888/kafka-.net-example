using KafkaExample3.Queue.Core;
using KafkaExample3.Queue.Messages;
using KafkaExample3.Queue.Producer;
using Microsoft.AspNetCore.Mvc;

namespace KafkaExample3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly PersonInfoProducer _personInfoProducer;
        public HomeController(PersonInfoProducer personInfoProducer)
        {
            _personInfoProducer = personInfoProducer;
        }
        [HttpPost("api/index")]
        public async Task<string> Index(PersonInfoMessage personInfoMessage)
        {
            await _personInfoProducer.SendKafkaAsync(personInfoMessage);
            //_personInfoProducer.SendKafka(personInfoMessage);
            return "";
        }
    }
}
