namespace KafkaExample3.Queue.Core
{
    public class KafkaSettings
    {
        public string Brokers { get; set; }
        public string GroupId { get; set; }
        public string Topic { get; set; }
        public string Key { get; set; }
        public string ConsumerGroup { get; set; }
        public string PersonInfoTopic { get; set; }

    }
}
