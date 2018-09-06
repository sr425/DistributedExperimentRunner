using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ExperimentController.Services
{
    public class ExperimentEvaluation
    {
        private ApplicationDbContext _context;
        private int _retries;

        public ExperimentEvaluation(ApplicationDbContext context, int retries)
        {
            _context = context;
            _retries = retries;
        }

        public async void TasksetFinished(long setId)
        {
            var taskset = await _context.TaskSets
                .Include(ts => ts.Tasks)
                .ThenInclude(t => t.Results)
                .FirstOrDefaultAsync(ts => ts.Id == setId);


            if (taskset.Tasks == null || taskset.Tasks.Count == 0)
                return;

            var validResults = taskset.Tasks
                .Select(t => t.Results.FirstOrDefault(r => !r.Failed))
                .Where(r => r != null)
                .ToList();

            var aggregatedResults = new Dictionary<string, double>();
            var referenceResult = validResults[0];
            double cnt = (double)validResults.Count;
            foreach (var key in referenceResult.Values.Keys)
            {
                aggregatedResults[key] = validResults.Select(r => r.Values[key]).Sum() / cnt;
            }
            taskset.AggregatedValues = aggregatedResults;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasTasksetFinished(long setId)
        {
            var tasks = _context.Tasks.Include(t => t.Results).Where(t => t.SetId == setId);

            var nrTasks = tasks.CountAsync();
            var finished = tasks.CountAsync(t => t.Failed || t.Results.Any(r => !r.Failed) || t.Results.Count() >= _retries);
            return (await nrTasks) == (await finished);
        }

        public async Task<bool> HasTasksetFailed(long setId)
        {
            return await _context.Tasks.AnyAsync(t => t.SetId == setId
            && (t.Failed || (t.Results.Count() > _retries && !t.Results.Any(r => !r.Failed))));
        }
    }
}