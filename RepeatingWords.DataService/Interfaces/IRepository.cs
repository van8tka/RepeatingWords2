using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RepeatingWords.DataService.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Get();
        T Get(int id);
        T Create(T item);
        void Create(IEnumerable<T> items);
        T Update(T item);
        bool Delete(T item);
    }
}
