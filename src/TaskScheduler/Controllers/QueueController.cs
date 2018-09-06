using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using TaskScheduler.Services;

namespace TaskScheduler.Controllers
{
    [Route ("queue")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private IPersistenceProvider _persistenceProvider;
        private StaticAuthentication _authentication;

        public QueueController (IPersistenceProvider persistenceProvider, StaticAuthentication authentication)
        {
            _persistenceProvider = persistenceProvider;
            _authentication = authentication;
        }

        [HttpPost]
        public async Task<ActionResult<long>> QueueWork ([FromBody] dynamic json)
        {
            if (!Request.Headers.TryGetValue ("Authorization", out StringValues token) || !_authentication.IsValid (token.ToString ()))
            {
                return BadRequest ("No valid client secret");
            }

            var newEntry = new SimpleQueueBufferEntry ()
            {
                InstanceTaskId = json.instanceTaskId,
                JsonContent = JsonConvert.SerializeObject (json),
                Timestamp = DateTime.UtcNow.Ticks
            };
            return await _persistenceProvider.UpdateOrAddPackageAsync ((long) json.instanceTaskId, newEntry);
        }

        [HttpDelete ("{instanceTaskId}")]
        public async Task<ActionResult> DeleteWorkPackage ([FromRoute] long instanceTaskId)
        {
            if (!Request.Headers.TryGetValue ("Authorization", out StringValues token) || !_authentication.IsValid (token.ToString ()))
            {
                return BadRequest ("No valid client secret");
            }
            await _persistenceProvider.DeletePackageIfExistsAsync (instanceTaskId);
            return Ok ();
        }
    }
}

/*var newEntry = new QueueEntry ()
        {
            InstanceTaskId = json.instanceTaskId,
            JsonContent = JsonConvert.SerializeObject (json)
        };
        await _context.QueueTable.AddAsync (newEntry);
        await _context.SaveChangesAsync ();
        return newEntry.Id;*/

/*[HttpGet]
public async Task<ActionResult<string>> GetWorkPackage ()
{
    var errorCandidates = await _context.QueueTable.Where (e => e.HandoutCnt > 3).ToListAsync ();
    _context.QueueTable.RemoveRange (errorCandidates);
    await _context.SaveChangesAsync ();

    var current = DateTime.Now;
    var candidates = _context.QueueTable.FromSql ("SELECT * FROM QueueTable WHERE LastHandout IS NULL OR ((JulianDay({0}) - JulianDay(LastHandout))* 86400000) > (60 * 1000)", current); //about milliseconds
    candidates = candidates.OrderBy (c => c.Id);

    var candidate = await candidates.FirstOrDefaultAsync ();
    if (candidate == null)
    {
        return null;
    }
    candidate.LastHandout = current;
    candidate.HandoutCnt += 1;
    if (candidate.FirstHandout == null) { candidate.FirstHandout = current; }
    await _context.SaveChangesAsync ();

    return candidate.JsonContent;
}*/

/*[HttpPut ("{instanceTaskId}/completed")]
public async Task<ActionResult> GetWorkPackage ([FromRoute] long instanceTaskId)
{
    await TaskDeletePackageIfExists (instanceTaskId);
    return Ok ();
}*/