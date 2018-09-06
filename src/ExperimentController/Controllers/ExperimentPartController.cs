using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExperimentController.Model;
using ExperimentController.Services;
using ExperimentController.Services.TaskCreation;
using ExperimentController.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExperimentController.Controllers
{
    [Route ("api/experiments/{experimentId}/experimentparts")]
    [Authorize]
    [ApiController]
    public class ExperimentPartController : ControllerBase
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        private TaskGenerator _generator;
        private ITaskManager _manager;

        public ExperimentPartController (ApplicationDbContext context, IMapper mapper, TaskGenerator generator, ITaskManager manager)
        {
            _mapper = mapper;
            _context = context;
            _generator = generator;
            _manager = manager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ExperimentPartViewModel>>> Get ([FromRoute] long experimentId)
        {
            var exp = await _context.Experiments
                .Include (e => e.Parts)
                .ThenInclude (p => p.InputDatasetRelations)
                .ThenInclude (r => r.Dataset)
                .FirstOrDefaultAsync (e => e.Id == experimentId);
            if (exp == null)
            {
                return NotFound ();
            }
            if (exp.Parts == null)
            {
                exp.Parts = new List<ExperimentPart> ();
                await _context.SaveChangesAsync ();
            }
            return exp.Parts?.Select (p => _mapper.Map<ExperimentPartViewModel> (p)).ToList ();
        }

        [HttpGet ("{partId}")]
        public async Task<ActionResult<ExperimentPartViewModel>> Get ([FromRoute] long experimentId, [FromRoute] long partId)
        {
            var part = await _context.ExperimentParts
                .Where (p => p.Id == partId && p.ExperimentId == experimentId)
                .Include (p => p.InputDatasetRelations)
                .ThenInclude (r => r.Dataset)
                .FirstOrDefaultAsync ();
            if (part == null)
            {
                return NotFound ();
            }

            return _mapper.Map<ExperimentPartViewModel> (part);
        }

        [HttpPost]
        public async Task<ActionResult> Create ([FromRoute] long experimentId, [FromBody] ExperimentPartViewModel partViewModel)
        {
            partViewModel.ExperimentId = experimentId;
            var exp = await _context.Experiments.Include (e => e.Parts).FirstOrDefaultAsync (e => e.Id == experimentId);
            if (exp == null)
            {
                return NotFound ();
            }
            if (exp.Parts == null) { exp.Parts = new List<ExperimentPart> (); }
            var part = _mapper.Map<ExperimentPart> (partViewModel);
            part.InputDatasetRelations = partViewModel.InputDatasets?
                .Select (d => new ExperimentPart_Dataset_Relation () { ExperimentPart = part, DatasetId = d.Id })
                .ToList ();

            exp.Parts.Add (part);
            await _context.SaveChangesAsync ();
            return Ok ();
        }

        [HttpPut ("{partId}")]
        public async Task<ActionResult> Update ([FromRoute] long experimentId, [FromRoute] long partId, [FromBody] ExperimentPartViewModel partViewModel)
        {
            if (partViewModel.ExperimentId != experimentId || partViewModel.Id != partId) { return BadRequest (); }
            var dbPart = await _context.ExperimentParts
                .Include (p => p.InputDatasetRelations)
                .ThenInclude (r => r.Dataset)
                .Where (p => p.Id == partId && p.ExperimentId == experimentId).FirstOrDefaultAsync ();
            if (dbPart == null)
            {
                return NotFound ();
            }
            var part = _mapper.Map<ExperimentPart> (partViewModel);

            var newDatasetIds = partViewModel?.InputDatasets.Select (d => d.Id).ToList ();
            if (newDatasetIds != null)
            {
                dbPart.InputDatasets = await _context.Datasets.Where (d => newDatasetIds.Contains (d.Id)).ToListAsync ();
            }
            else
            {
                dbPart.InputDatasets = null;
            }

            _context.Entry (dbPart).CurrentValues.SetValues (part);
            await _context.SaveChangesAsync ();
            return Ok ();
        }

        [HttpDelete ("{partId}")]
        public async Task<ActionResult> Delete ([FromRoute] long experimentId, [FromRoute] long partId)
        {
            var exp = await _context.Experiments.Include (e => e.Parts).FirstOrDefaultAsync (e => e.Id == experimentId);
            if (exp == null) { return NotFound (); }

            var partToDelete = exp.Parts.FirstOrDefault (p => p.Id == partId);

            if (partToDelete == null) { return NotFound (); }

            exp.Parts.Remove (partToDelete);
            await _context.SaveChangesAsync ();
            return Ok ();
        }

        [HttpPost ("{partId}/start")]
        public async Task<ActionResult> StartExperimentPart ([FromRoute] long experimentId, [FromRoute] long partId)
        {
            var part = await _context.ExperimentParts
                .Include (p => p.Experiment)
                .Include (p => p.InputDatasetRelations)
                .ThenInclude (r => r.Dataset)
                .Include (p => p.TaskSets)
                .ThenInclude (t => t.Tasks)
                .Where (p => p.Id == partId && p.ExperimentId == experimentId).FirstOrDefaultAsync ();
            if (part == null)
            {
                return NotFound ();
            }

            part.TaskSets = _generator.GenerateTaskSets (part);
            part.TaskSets.ForEach (ts => ts.Tasks = _generator.GenerateTasks (ts));
            await _context.SaveChangesAsync ();

            var tasks = part.TaskSets.SelectMany (p => p.Tasks).ToList ();
            Console.WriteLine ("Starting " + tasks.Count + " tasks.....");
            var success = await _manager.QueueTaskRange (tasks);
            if (!success)
            {
                var cancelList = part.TaskSets.Where (ts => ts.Tasks != null).SelectMany (p => p.Tasks).ToList ();
                await _manager.CancelTasks (cancelList);
                return StatusCode (500);
            }

            part.TaskSets.ForEach (ts => ts.Running = true);
            part.Running = true;

            await _context.SaveChangesAsync ();
            return Ok ();
        }

        [HttpPost ("{partId}/stop")]
        public async Task<ActionResult> StopExperimentPart ([FromRoute] long experimentId, [FromRoute] long partId)
        {
            var part = await _context.ExperimentParts
                .Include (p => p.TaskSets)
                .ThenInclude (t => t.Tasks)
                .Where (p => p.Id == partId && p.ExperimentId == experimentId).FirstOrDefaultAsync ();
            if (part == null)
            {
                return NotFound ();
            }

            if (part.TaskSets == null)
            {
                return Ok ();
            }

            var cancelList = part.TaskSets.Where (ts => ts.Tasks != null).SelectMany (p => p.Tasks).ToList ();
            var success = await _manager.CancelTasks (cancelList);
            if (!success)
            {
                return StatusCode (500);
            }

            part.TaskSets.ForEach (ts => ts.Running = false);
            part.Running = false;

            await _context.SaveChangesAsync ();
            return Ok ();
        }
    }
}