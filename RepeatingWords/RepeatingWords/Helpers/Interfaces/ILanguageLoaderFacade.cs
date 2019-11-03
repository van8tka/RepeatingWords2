using RepeatingWords.Model;
using System.Threading.Tasks;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface ILanguageLoaderFacade
    {
        Task LoadSelectedLanguageToDB(int idLanguage);
    }
}
