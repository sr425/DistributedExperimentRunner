using System;
using System.Linq;
using System.Threading.Tasks;
using ExperimentController.Model;
using ExperimentController.Services.TaskCreation;
using Microsoft.EntityFrameworkCore;

namespace ExperimentController.Services
{
    public class ExperimentRunManager
    {
        private ApplicationDbContext _context;
        private TaskGenerator _generator;
        private ITaskManager _taskManager;

        public ExperimentRunManager(ApplicationDbContext context, TaskGenerator generator, ITaskManager taskManager)
        {
            _context = context;
            _taskManager = taskManager;
        }

        public async Task StartExperimentPart(long id)
        {
            var experimentPart = await _context.ExperimentParts
                .Include(exp => exp.Experiment)
                .Include(exp => exp.TaskSets)
                .ThenInclude(ts => ts.Tasks)
                .FirstOrDefaultAsync(exp => exp.Id == id);
            if (experimentPart == null)
                throw new ArgumentException("No experiment for the given id found");
            if (experimentPart.TaskSets == null)
            {
                experimentPart.TaskSets = _generator.GenerateTaskSets(experimentPart);
            }

            experimentPart.TaskSets.Where(t => t.Tasks == null || t.Tasks.Count == 0).ToList().ForEach(taskset =>
         {
             taskset.Tasks = _generator.GenerateTasks(taskset);
         });
            await _context.SaveChangesAsync();

            var tasks = experimentPart.TaskSets.SelectMany(ts => ts.Tasks).ToList();
            await _taskManager.QueueTaskRange(tasks);
        }

        public async Task StartTaskSet(long id)
        {
            var taskSet = await _context.TaskSets.Include(ts => ts.Tasks).FirstOrDefaultAsync(ts => ts.Id == id);
            if (taskSet == null)
                throw new ArgumentException("No taskset for the given id found");

            if (taskSet.Tasks == null)
                taskSet.Tasks = _generator.GenerateTasks(taskSet);

            await _context.SaveChangesAsync();

            await _taskManager.QueueTaskRange(taskSet.Tasks);
        }

        public async Task StopExperimentPart(long id)
        {
            var experimentPart = await _context.ExperimentParts.Include(exp => exp.TaskSets).FirstOrDefaultAsync(exp => exp.Id == id);
            if (experimentPart == null)
                throw new ArgumentException("No experiment for the given id found");
            if (experimentPart.TaskSets == null)
                return;

            var tasks = experimentPart.TaskSets.Select(ts => StopTaskset(ts.Id));
            foreach (var t in tasks)
                await t;
        }

        public async Task StopTaskset(long id)
        {
            var taskSet = await _context.TaskSets.Include(ts => ts.Tasks).FirstOrDefaultAsync(ts => ts.Id == id);
            if (taskSet == null)
                throw new ArgumentException("No taskset for the given id found");

            if (taskSet.Tasks == null) return;

            await _taskManager.CancelTasks(taskSet.Tasks);
        }
    }
}