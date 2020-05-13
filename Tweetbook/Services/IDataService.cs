using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tweetbook.Services
{
    public interface IDataService<T>
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid Id);

        Task<bool> CreateAsync(T item);

        Task<bool> UpdateAsync(T item);

        Task<bool> DeleteAsync(Guid Id);
    }
}
