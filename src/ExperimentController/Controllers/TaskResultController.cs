/*using System.Linq;
using System.Threading.Tasks;
using ExperimentController.Model;
using ExperimentController.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExperimentController.Controllers
{
    [Route ("api/tasks/{taskId}/results")]
    [ApiController]
    public class TaskResultController : ControllerBase
    {
        private ApplicationDbContext _context;
        private int _retries;
        private ExperimentEvaluation _evaluation;

        public TaskResultController (ApplicationDbContext context, IConfiguration config, ExperimentEvaluation evaluation)
        {
            _context = context;
            _retries = config["RetriesOnTaskFail"] == null?3 : int.Parse (config["RetriesOnTaskFail"]);
            _evaluation = evaluation;
        }

        [HttpPost]
        public async Task<ActionResult> Add (long taskId, [FromBody] ClientResult value)
        {
            var task = await _context.Tasks.Include (t => t.Results).Include (t => t.Set).FirstOrDefaultAsync (t => t.Id == taskId);
            if (task == null)
                return NotFound ();

            task.Results.Add (value);
            if (!value.Failed)
            {
                _evaluation.TaskFinished (task.Id);
            }
            else if (task.Results.Count >= _retries)
            {
                if (!task.Results.Any (r => !r.Failed))
                {
                    task.Failed = true;
                }
            }
            _context.SaveChanges ();
            return Ok ();
        }
    }
}*/