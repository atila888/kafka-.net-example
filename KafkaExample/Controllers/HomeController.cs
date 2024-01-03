using KafkaExample.Queue.Messages;
using KafkaExample.Queue.Producer;
using Microsoft.AspNetCore.Mvc;

namespace KafkaExample.Controllers
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
