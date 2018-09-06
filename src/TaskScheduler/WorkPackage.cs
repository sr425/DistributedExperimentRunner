using System;

namespace TaskScheduler
{
    public class QueueEntry
    {
        public long Id { get; set; }

        public long InstanceTaskId { get; set; }

        public string JsonContent { get; set; }

        public int HandoutCnt { get; set; } = 0;

        public DateTime? FirstHandout { get; set; }
        public DateTime? LastHandout { get; set; }
    }

    public class SimpleQueueBufferEntry
    {
        public long Id { get; set; }

        public long InstanceTaskId { get; set; }

        public string JsonContent { get; set; }

        public long Timestamp { get; set; }
    }
}