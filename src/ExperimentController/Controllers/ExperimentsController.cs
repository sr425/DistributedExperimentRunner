using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExperimentController.Model;
using ExperimentController.Services;
using ExperimentController.Services.TaskCreation;
using ExperimentController.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExperimentController.Controllers
{
    [Route("api/experiments")]
    [Authorize]
    [ApiController]
    public class ExperimentsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private TaskGenerator _generator;
        private IMapper _mapper;
        private ExperimentRunManager _experimentRunManager;

        public ExperimentsController(ApplicationDbContext context, TaskGenerator generator, ExperimentRunManager runManager, IMapper mapper)
        {
            _context = context;
            _generator = generator;
            _mapper = mapper;
            _experimentRunManager = runManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExperimentFolderViewModel>>> Get()
        {

            return await _context.Experiments
                .Select(e => new ExperimentFolderViewModel() { Id = e.Id, Name = e.Name })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExperimentViewModel>> Get([FromRoute] long id)
        {
            var experiment = await _context.Experiments
                .FirstOrDefaultAsync(exp => exp.Id == id);

            if (experiment == null)
                return NotFound();

            return _mapper.Map<ExperimentViewModel>(experiment);
        }

        [HttpPost]
        public async Task<ActionResult<Experiment>> Create([FromBody] ExperimentViewModel experimentVM)
        {
            var experiment = _mapper.Map<Experiment>(experimentVM);

            await _context.Experiments.AddAsync(experiment);
            await _context.SaveChangesAsync();
            return experiment;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Experiment>> Update([FromRoute] long id, [FromBody] ExperimentViewModel experimentVM)
        {
            var experiment = _mapper.Map<Experiment>(experimentVM);
            var dbExperiment = await _context.Experiments.FindAsync(id);
            if (dbExperiment == null)
                return NotFound();

            _context.Entry(dbExperiment).CurrentValues.SetValues(experiment);
            await _context.SaveChangesAsync();
            return experiment;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] long id)
        {
            var experiment = await _context.Experiments.FindAsync(id);
            if (experiment == null)
                return Ok();
            _context.Experiments.Remove(experiment);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}