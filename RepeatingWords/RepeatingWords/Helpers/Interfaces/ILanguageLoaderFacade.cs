using RepeatingWords.Model;
using System.Threading.Tasks;

namespace RepeatingWords.Helpers.Interfaces
{
    /// <summary>
    /// интерфейс загрузки словарей из интернета (с сервера)
    /// </summary>
    public interface ILanguageLoaderFacade
    {
        /// <summary>
        /// загрузка словарей выбранного языка с сервера
        /// </summary>
        /// <param name="idLanguage">ID языка на сервере</param>
        /// <param name="nameLanguage">имя языка</param>
        /// <returns></returns>
        Task LoadLanguageFromApi(int idLanguage, string nameLanguage);
    }
}
