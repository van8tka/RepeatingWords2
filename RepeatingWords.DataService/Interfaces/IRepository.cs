using System.Linq;

namespace RepeatingWords.DataService.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Get();
        T Get(int id);
        T Create(T item);
        T Update(T item);
        bool Delete(T item);
    }
}
