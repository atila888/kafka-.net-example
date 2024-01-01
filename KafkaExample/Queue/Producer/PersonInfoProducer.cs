using Confluent.Kafka;
using KafkaExample3.Queue.Core;
using KafkaExample3.Queue.Messages;
using Microsoft.Extensions.Options;

namespace KafkaExample3.Queue.Producer
{
    public class PersonInfoProducer : KafkaProducerBase<PersonInfoMessage>
    {
        public PersonInfoProducer(IOptions<KafkaSettings> options):base(options,options.Value.PersonInfoTopic)
        {

        }
    }
}
