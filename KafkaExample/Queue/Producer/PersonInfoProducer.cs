using Confluent.Kafka;
using KafkaExample.Queue.Core;
using KafkaExample.Queue.Messages;
using Microsoft.Extensions.Options;

namespace KafkaExample.Queue.Producer
{
    public class PersonInfoProducer : KafkaProducerBase<PersonInfoMessage>
    {
        public PersonInfoProducer(IOptions<KafkaSettings> options):base(options,options.Value.PersonInfoTopic)
        {

        }
    }
}
