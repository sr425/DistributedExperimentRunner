using System.Collections.Generic;
using System.Threading.Tasks;
using ExperimentController.Model;

namespace ExperimentController.Services
{
    public interface ITaskManager
    {
        Task<bool> QueueTask(InstanceTask task);

        Task<bool> QueueTaskRange(IEnumerable<InstanceTask> tasks);

        Task<bool> CancelTask(InstanceTask task);

        Task<bool> CancelTasks(IEnumerable<InstanceTask> tasks);
    }

    public class WorkPackage
    {
        public long InstanceTaskId { get; set; }

        public string PayloadHash { get; set; }

        public string FilesPrefix { get; set; }

        public Dictionary<string, string> Files { get; set; }

        public List<FixedParameter> Parameters;
    }
}