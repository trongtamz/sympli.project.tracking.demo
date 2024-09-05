using Microsoft.Extensions.Caching.Memory;

namespace Sympli.Project.Tracking.Infrastructures.Cache.Interfaces
{
    public interface ICacheService
    {
        bool TryGetValue<TItem>(object key, out TItem value);

        void Set<TItem>(object key, TItem value, MemoryCacheEntryOptions options);
    }
}