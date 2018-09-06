using System.Collections.Generic;
using System.Threading.Tasks;
using ExperimentController.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExperimentController.Controllers
{
    [Route ("api/folders")]
    [Authorize]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private ApplicationDbContext _context;

        public FolderController (ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<ExperimentFolderHierarchy> GetOrCreateHierarchy ()
        {
            var hierarchy = await _context.ExperimentFolderHierarchies.FirstOrDefaultAsync ();
            if (hierarchy != null)
                return hierarchy;
            hierarchy = new ExperimentFolderHierarchy ();
            hierarchy.SubFolders = new List<ExperimentFolder> ();
            await _context.AddAsync (hierarchy);
            await _context.SaveChangesAsync ();
            return hierarchy;
        }

        [HttpGet]
        public async Task<ActionResult<List<ExperimentFolder>>> Get ()
        {
            var hierarchy = await GetOrCreateHierarchy ();
            return hierarchy.SubFolders;
        }

        [HttpPut]
        public async Task<ActionResult> Update ([FromBody] List<ExperimentFolder> folders)
        {
            var hierarchy = await GetOrCreateHierarchy ();
            hierarchy.SubFolders = folders;
            await _context.SaveChangesAsync ();
            return Ok ();
        }

        [HttpDelete]
        public async Task<ActionResult> Remove ()
        {
            var hierarchy = await _context.ExperimentFolderHierarchies.FirstOrDefaultAsync ();
            if (hierarchy == null)
                return NotFound ();
            _context.Remove (hierarchy);
            await _context.SaveChangesAsync ();
            return Ok ();
        }
    }
}