namespace ExperimentController.Model
{
    public class ExecutionPayload
    {
        public string Id { get; set; }

        public string Filename { get; set; }

        public byte[] BinaryExecutionData { get; set; }
    }
}