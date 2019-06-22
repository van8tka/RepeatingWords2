using RepeatingWords.DataService.Model;

namespace RepeatingWords.DataService.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Dictionary> DictionaryRepository { get; }
        IRepository<Words> WordsRepository { get; }
        IRepository<LastAction> LastActionRepository { get; }
        void Save();
    }
}
