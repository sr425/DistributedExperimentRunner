using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperimentController.Model;
using ExperimentController.Services;
using ExperimentController.Services.TaskCreation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExperimentController.Controllers
{
    [Route("api/experiments/{experimentId}/tasksets")]
    [ApiController]
    public class TaskSetController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ITaskManager _manager;
        private TaskGenerator _generator;

        public TaskSetController(ApplicationDbContext context, ITaskManager manager, TaskGenerator generator)
        {
            _context = context;
            _manager = manager;
            _generator = generator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskSet>>> Get([FromRoute] long experimentPartId)
        {
            var experiment = await _context.ExperimentParts.Include(exp => exp.TaskSets).FirstOrDefaultAsync(exp => exp.Id == experimentPartId);
            if (experiment == null)
                return NotFound();
            if (experiment.TaskSets == null)
                return new List<TaskSet>();
            return experiment.TaskSets.ToList();
        }

        [HttpGet("{tasksetId}")]
        public async Task<ActionResult<TaskSet>> Get([FromRoute] long experimentPartId, [FromRoute] long tasksetId)
        {
            var experiment = await _context.ExperimentParts.Include(exp => exp.TaskSets).FirstOrDefaultAsync(exp => exp.Id == experimentPartId);
            var set = experiment?.TaskSets?.FirstOrDefault(t => t.Id == tasksetId);
            if (set == null)
                return NotFound();
            return set;
        }

        [HttpPost]
        public async Task<ActionResult<TaskSet>> Post([FromRoute] long experimentId, [FromBody] TaskSet value)
        {
            var experiment = await _context.ExperimentParts.Include(exp => exp.TaskSets).FirstOrDefaultAsync(exp => exp.Id == experimentId);
            if (experiment == null)
                return NotFound();

            experiment.TaskSets.Add(value);
            _context.SaveChanges();
            return value;
        }

        [HttpPut("{tasksetId}")]
        public async Task<ActionResult<TaskSet>> Put([FromRoute] long experimentId, [FromRoute] long tasksetId, [FromBody] TaskSet value)
        {
            var task = await _context.TaskSets.FindAsync(tasksetId);
            _context.Entry(task).CurrentValues.SetValues(value);
            _context.SaveChanges();
            return value;
        }

        [HttpDelete("{tasksetId}")]
        public async Task<ActionResult> Delete([FromRoute] long experimentId, [FromRoute] long tasksetId)
        {
            var experiment = await _context.ExperimentParts.Include(exp => exp.TaskSets).FirstOrDefaultAsync(exp => exp.Id == experimentId);
            var task = await _context.TaskSets.FindAsync(tasksetId);
            if (experiment == null || task == null)
                return Ok();

            experiment.TaskSets.Remove(task);
            _context.TaskSets.Remove(task);

            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("{tasksetId}/generatetasks")]
        public async Task<ActionResult<List<InstanceTask>>> GenerateTasks([FromRoute] long experimentId, [FromRoute] long tasksetId)
        {
            //TODO: only for testing
            var task = await _context.TaskSets
                .Include(t => t.Tasks)
                .Include(t => t.InputDataset)
                .FirstOrDefaultAsync(t => t.Id == tasksetId);

            if (task == null)
                return NotFound();

            task.Tasks = _generator.GenerateTasks(task);

            _context.SaveChanges();
            return task.Tasks;
        }
    }
}