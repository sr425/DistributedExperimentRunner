using System.Linq;
using System.Threading.Tasks;
using ExperimentController.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExperimentController.Services
{
    public class ResultHandlerService
    {
        private int _retries;
        private ApplicationDbContext _context;
        private ExperimentEvaluation _evaluation;

        public ResultHandlerService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _retries = configuration["RetriesOnTaskFail"] == null ? 3 : int.Parse(configuration["RetriesOnTaskFail"]);
            _evaluation = new ExperimentEvaluation(_context, _retries);
        }

        public async Task<bool> HandleResultAsync(ClientResult result)
        {
            if (!(await _context.Tasks.AnyAsync(t => t.Id == result.TaskId)))
            {
                return false;
            }
            await _context.Results.AddAsync(result);
            await _context.SaveChangesAsync();

            var setId = await _context.Tasks.Where(t => t.Id == result.TaskId).Select(t => t.SetId).FirstAsync();
            var set = await _context.TaskSets.Include(s => s.ExperimentPart).FirstAsync(s => s.Id == setId);

            if (await _evaluation.HasTasksetFinished(set.Id))
            {
                _evaluation.TasksetFinished(set.Id);

                set.Running = false;
                set.Finished = true;

                if (await _evaluation.HasTasksetFailed(set.Id))
                {
                    set.Failed = true;
                }

                if (set.ExperimentPart.DynamicParameters == null || set.ExperimentPart.DynamicParameters.Count == 0)
                {
                    set.ExperimentPart.Running = false;
                    set.ExperimentPart.Finished = true;
                    set.ExperimentPart.Failed = set.Failed || set.ExperimentPart.Failed;
                    set.ExperimentPart.AggregatedValues = set.AggregatedValues;
                }
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}