using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExperimentController.Model;
using ExperimentController.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExperimentController.Controllers
{
    [Route ("api/datasets")]
    [ApiController]
    public class DatasetController : ControllerBase
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;

        public DatasetController (ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DatasetViewModel>>> Get ()
        {
            return await (_context.Datasets.Select (d => _mapper.Map<DatasetViewModel> (d))).ToListAsync ();
        }

        [HttpGet ("{id}")]
        public async Task<ActionResult<Dataset>> Get ([FromRoute] long id)
        {
            var dataset = await _context.Datasets.FindAsync (id);

            if (dataset == null)
                return NotFound ();
            return dataset;
        }

        [HttpPost]
        public async Task<ActionResult<Dataset>> Create ([FromBody] Dataset dataset)
        {
            await _context.Datasets.AddAsync (dataset);
            await _context.SaveChangesAsync ();
            return dataset;
        }

        [HttpPut ("{id}")]
        public async Task<ActionResult<Dataset>> Update ([FromRoute] long id, [FromBody] Dataset dataset)
        {
            var dbDataset = await _context.Datasets.FindAsync (id);
            if (dbDataset == null)
                return NotFound ();

            _context.Entry (dbDataset).CurrentValues.SetValues (dataset);

            await _context.SaveChangesAsync ();
            return dataset;
        }

        [HttpDelete ("{id}")]
        public async Task<ActionResult> Delete ([FromRoute] long id)
        {
            var dataset = await _context.Datasets.FindAsync (id);
            if (dataset == null)
                return NotFound ();
            _context.Datasets.Remove (dataset);
            await _context.SaveChangesAsync ();
            return NotFound ();
        }
    }
}