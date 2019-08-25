using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Interfaces;

namespace RepeatingWords.DataService.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
      
        public UnitOfWork(ISQLite sqlite)
        {
            _dbpath = sqlite.GetDatabasePath(DATABASE_NAME);
            _dbContext = new SQLiteContext(_dbpath); 
        }

        internal const string DATABASE_NAME = "repeatwords.db";
        private readonly string _dbpath;
        private SQLiteContext _dbContext;

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
