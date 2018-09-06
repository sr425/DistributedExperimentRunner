namespace ExperimentClient
{
    public class QueueConfig
    {
        public string TaskQueueConnectionString { get; set; }
        public string TaskQueueName { get; set; }
        public string ResultQueueConnectionString { get; set; }
        public string ResultQueueName { get; set; }

        public override string ToString()
        {
            return $"TaskQueue: {TaskQueueConnectionString}, Name: {TaskQueueName}\n ResultQueue: {ResultQueueConnectionString}, Name: {ResultQueueName}";
        }
    }
}