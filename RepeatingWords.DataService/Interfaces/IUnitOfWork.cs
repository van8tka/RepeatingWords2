using System.Threading.Tasks;
using RepeatingWords.DataService.Model;

namespace RepeatingWords.DataService.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Dictionary> DictionaryRepository { get; }
        IRepository<Words> WordsRepository { get; }
        IRepository<Language> LanguageRepository { get; }
        void Save();
    }
}
