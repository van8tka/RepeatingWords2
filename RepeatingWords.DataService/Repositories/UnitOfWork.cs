using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;

namespace RepeatingWords.DataService.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {      
        //ctor
        public UnitOfWork(string dbpath)
        {
            _dbContext = new SQLiteContext(dbpath);
        }

        private readonly SQLiteContext _dbContext;

        private IRepository<Dictionary> _dictionaryRepo;
        public IRepository<Dictionary> DictionaryRepository => _dictionaryRepo ?? (_dictionaryRepo = new GenericRepository<Dictionary>(_dbContext));
        private IRepository<Words> _wordsRepo;
        public IRepository<Words> WordsRepository => _wordsRepo ?? (_wordsRepo = new GenericRepository<Words>(_dbContext));
        private IRepository<LastAction> _lastAction;
        public IRepository<LastAction> LastActionRepository => _lastAction ?? (_lastAction = new GenericRepository<LastAction>(_dbContext));

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
