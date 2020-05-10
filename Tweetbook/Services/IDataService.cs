using System;
using System.Collections.Generic;

namespace Tweetbook.Services
{
    public interface IDataService<T>
    {
        IEnumerable<T> GetAll();

        T GetById(Guid Id);

        void Add(T item);

        bool Update(T item);

        bool Delete(Guid Id);
    }
}
