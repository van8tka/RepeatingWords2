using System.Threading.Tasks;
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
        private IRepository<Language> _languageRepo;
        public IRepository<Language> LanguageRepository => _languageRepo ?? (_languageRepo = new GenericRepository<Language>(_dbContext));
        public void BeginTransaction()
        {
            _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _dbContext.Database.CommitTransaction();
        }

        public void RollBackTransaction()
        {
            _dbContext.Database.RollbackTransaction();
        }


        public void Save()
        {
            _dbContext.SaveChanges();
        }

      
    }
}
