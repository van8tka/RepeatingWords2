using Microsoft.EntityFrameworkCore;
using RepeatingWords.DataService.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
 

namespace RepeatingWords.DataService.Model
{
    public class GenericRepository<T>:IRepository<T> where T : class
    {
        private readonly SQLiteContext _dbContext;
        public GenericRepository(SQLiteContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IQueryable<T> Get()
        {
           return _dbContext.Set<T>();
        }

        public T Get(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public T Create(T item)
        {
            return _dbContext.Set<T>().Add(item).Entity;
        }

        public void Create(IEnumerable<T> items)
        {
            _dbContext.Set<T>().AddRange(items);
        }

        public void Update(IEnumerable<T> items)
        {
            _dbContext.Set<T>().UpdateRange(items);
        }

        public T Update(T item)
        {
           _dbContext.Entry(item).State = EntityState.Modified;
            return item;
        }

        public bool Delete(T item)
        {
            try
            {              
                _dbContext.Set<T>().Remove(item);                
                return true;
            }
            catch(Exception e)
            {
                //fixme: change Debug to Log
                Debug.WriteLine(e);
                return false;
            }            
        }
    }
}
