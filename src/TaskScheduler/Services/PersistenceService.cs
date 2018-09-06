using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TaskScheduler.Services
{
    public class SqlPersistenceProvider : IPersistenceProvider
    {
        private ApplicationDbContext _context;

        public SqlPersistenceProvider (ApplicationDbContext context)
        {
            _context = context;

            _context.Database.EnsureCreated ();
        }

        public async Task<long> AddPackageAsync (SimpleQueueBufferEntry entry)
        {
            await _context.QueueBuffer.AddAsync (entry);
            await _context.SaveChangesAsync ();
            return entry.Id;
        }

        public async Task<long> UpdateOrAddPackageAsync (long instanceTaskId, SimpleQueueBufferEntry entry)
        {
            var oldEntry = await _context.QueueBuffer.FirstOrDefaultAsync (e => e.InstanceTaskId == instanceTaskId);
            if (oldEntry == null)
            {
                return await AddPackageAsync (entry);
            }

            _context.Entry (oldEntry).CurrentValues.SetValues (entry);
            await _context.SaveChangesAsync ();
            return oldEntry.Id;
        }

        public async Task DeletePackageIfExistsAsync (long instanceTaskId)
        {
            var deletionObject = await _context.QueueBuffer.FirstOrDefaultAsync (t => t.InstanceTaskId == instanceTaskId);
            if (deletionObject == null) { return; }
            _context.QueueBuffer.Remove (deletionObject);
            await _context.SaveChangesAsync ();
        }

        public Task<int> GetPackageNumberAsync ()
        {
            return Task.Run (() => _context.QueueBuffer.Count ());
        }

        public async Task<IEnumerable<SimpleQueueBufferEntry>> GetPackages (int number)
        {
            return await _context.QueueBuffer.OrderBy (e => e.Timestamp).Take (number).ToListAsync ();
        }

        public async Task RemovePackagesAsync (IEnumerable<SimpleQueueBufferEntry> entries)
        {
            var ids = entries.Select (e => e.Id);
            var packages = _context.QueueBuffer.Where (i => ids.Contains (i.Id));
            _context.QueueBuffer.RemoveRange (packages);
            await _context.SaveChangesAsync ();
        }
    }

    /*
        public class CosmosDbPersistenceProvider : IPersistenceProvider
        {
            private DocumentClient _client;
            private const string DatabaseName = "QueueDb";
            private const string CollectionName = "QueueItems";

            public CosmosDbPersistenceProvider(IConfiguration configuration)
            {
                var endpointUrl = configuration["cosmosEndpointUri"];
                var primaryKey = configuration["cosmosKey"];
                _client = new DocumentClient(new System.Uri(endpointUrl), primaryKey);

                _client.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseName }).Wait();
                this._client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DatabaseName), new DocumentCollection { Id = CollectionName }).Wait();
            }

            public async Task<long> AddPackageAsync(SimpleQueueBufferEntry entry)
            {
                await this._client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), entry);
                return entry.Id;
            }

            public async Task DeletePackageIfExistsAsync(long instanceTaskId)
            {
                try
                {
                    await this._client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseName, CollectionName, instanceTaskId.ToString()));
                }
                catch
                {
                    return;
                }
            }

            public async Task<int> GetPackageNumberAsync()
            {
                return await Task.Run(() => _client.CreateDocumentQuery(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), "SELECT c.id FROM c").Count());
            }

            public async Task<IEnumerable<SimpleQueueBufferEntry>> GetPackages(int number)
            {
                return await Task.Run(() => _client.CreateDocumentQuery<SimpleQueueBufferEntry>(
                     UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName)).FromSql("SELECT TOP {0} FROM c ORDERBY c._ts").ToList());//.OrderBy(c => c.).Take(number).ToList());
            }

            public async Task RemovePackagesAsync(IEnumerable<SimpleQueueBufferEntry> entries)
            {
                var tasks = entries.Select(e => DeletePackageIfExistsAsync(e.Id));
                foreach (var task in tasks)
                {
                    await task;
                }
            }
        }*/
}