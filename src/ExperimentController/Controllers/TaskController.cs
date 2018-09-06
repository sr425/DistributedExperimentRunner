using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperimentController.Model;
using ExperimentController.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExperimentController.Controllers
{

    [Route("api/tasksets/{tasksetId}/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ITaskManager _manager;

        public TaskController(ApplicationDbContext context, ITaskManager manager)
        {
            _context = context;
            _manager = manager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InstanceTask>>> Get([FromRoute] long tasksetId)
        {
            var taskset = await _context.TaskSets.Include(t => t.Tasks).FirstOrDefaultAsync(t => t.Id == tasksetId);
            return taskset?.Tasks.ToList();
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<InstanceTask>> Get([FromRoute] long tasksetId, [FromRoute] long taskId)
        {
            var taskset = await _context.TaskSets.Include(t => t.Tasks).FirstOrDefaultAsync(t => t.Id == tasksetId);
            return taskset?.Tasks.FirstOrDefault(t => t.Id == taskId);
        }

        [HttpPost]
        public async Task<ActionResult<InstanceTask>> Post([FromRoute] long tasksetId, [FromBody] InstanceTask value)
        {
            var taskset = await _context.TaskSets.Include(t => t.Tasks).FirstOrDefaultAsync(t => t.Id == tasksetId);
            if (taskset == null)
                return NotFound();

            taskset.Tasks.Add(value);
            _context.SaveChanges();
            return value;
        }

        [HttpPut("{taskId}")]
        public async Task<ActionResult<InstanceTask>> Put([FromRoute] long tasksetId, [FromRoute] long taskId, [FromBody] InstanceTask value)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task.Set.Id != tasksetId)
                return NotFound();

            _context.Entry(task).CurrentValues.SetValues(value);
            _context.SaveChanges();
            return value;
        }

        [HttpDelete("{taskId}")]
        public async Task<ActionResult> Delete([FromRoute] long tasksetId, [FromRoute] long taskId)
        {
            var taskset = await _context.TaskSets.Include(t => t.Tasks).FirstOrDefaultAsync(t => t.Id == tasksetId);
            var task = await _context.Tasks.FindAsync(taskId);
            if (taskset == null || task == null)
                return NotFound();

            taskset.Tasks.Remove(task);
            _context.Tasks.Remove(task);

            _context.SaveChanges();
            return NotFound();
        }
    }
}