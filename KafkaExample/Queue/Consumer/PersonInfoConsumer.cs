using KafkaExample3.Queue.Core;
using KafkaExample3.Queue.Messages;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace KafkaExample3.Queue.Consumer
{
    public class PersonInfoConsumer : KafkaConsumerBase<PersonInfoMessage>
    {
        public PersonInfoConsumer(IOptions<KafkaSettings> options) : base(options, options.Value.PersonInfoTopic)
        {
        }

        public override async Task HandleMessage(PersonInfoMessage message)
        {
            Debug.WriteLine(message);
            Debug.WriteLine("Handle Yakaladı");
        }
    }
}
