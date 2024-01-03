using KafkaExample.Queue.Core;
using KafkaExample.Queue.Messages;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace KafkaExample.Queue.Consumer
{
    public class PersonInfoConsumer : KafkaConsumerBase<PersonInfoMessage>
    {
        public PersonInfoConsumer(IOptions<KafkaSettings> options) : base(options, options.Value.PersonInfoTopic)
        {
        }

        public override async Task HandleMessage(KafkaMessage<PersonInfoMessage> message)
        {
            Debug.WriteLine(message);
            Debug.WriteLine("New message received...");
        }
    }
}
