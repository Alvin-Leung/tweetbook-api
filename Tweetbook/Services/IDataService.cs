using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tweetbook.Services
{
    public interface IDataService<TItem, TKey>
    {
        Task<IEnumerable<TItem>> GetAllAsync();

        Task<TItem> GetAsync(TKey key);

        Task<bool> CreateAsync(TItem item);

        Task<bool> UpdateAsync(TItem item);

        Task<bool> DeleteAsync(TKey key);
    }
}
