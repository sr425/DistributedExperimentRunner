using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskScheduler.Services
{
    public interface IPersistenceProvider
    {
        Task DeletePackageIfExistsAsync(long instanceTaskId);
        Task<long> AddPackageAsync(SimpleQueueBufferEntry entry);

        Task<long> UpdateOrAddPackageAsync(long instanceTaskId, SimpleQueueBufferEntry entry);

        Task<int> GetPackageNumberAsync();
        Task<IEnumerable<SimpleQueueBufferEntry>> GetPackages(int number);

        Task RemovePackagesAsync(IEnumerable<SimpleQueueBufferEntry> entries);
    }
}
