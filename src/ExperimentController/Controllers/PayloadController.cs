using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ExperimentController.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExperimentController.Controllers
{
    [Route("api/payload")]
    [ApiController]
    public class PayloadController : ControllerBase
    {
        private ApplicationDbContext _context;

        public PayloadController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get([FromRoute] string id)
        {
            var data = await _context.Payload.FindAsync(id);
            var memStream = new MemoryStream(data.BinaryExecutionData);
            return File(data.BinaryExecutionData, "application/zip", data.Id + ".zip");
            //return await _context.Payload.FindAsync(id);
        }

        [HttpGet("{id}/exists")]
        public async Task<bool> Exists([FromRoute] string id)
        {
            return await _context.Payload.AnyAsync(p => p.Id == id);
        }



        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult<string>> Create(IFormFile file) //[FromBody] ExecutionPayload value)
        {
            var memStream = new MemoryStream();
            await file.CopyToAsync(memStream);
            var binaryContent = memStream.GetBuffer();

            string id = null;
            using (var shaM = new SHA512Managed())
            {
                id = shaM.ComputeHash(binaryContent).ToHexString();
            }

            var payload = await _context.Payload.FindAsync(id);
            if (payload != null)
                return id;//CreatedAtRoute("get_payload", new { payload.Id });

            await _context.Payload.AddAsync(new ExecutionPayload() { Id = id, BinaryExecutionData = binaryContent, Filename = file.FileName });
            await _context.SaveChangesAsync();
            return id;
            //return CreatedAtRoute("get_payload", new { id });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ExecutionPayload>> Delete([FromRoute] long id)
        {
            var payload = await _context.Payload.FindAsync(id);
            if (payload == null)
                return NotFound();
            _context.Payload.Remove(payload);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}