using RepeatingWords.DataService.Model;
using RepeatingWords.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface IWebApiService
    {
        /// <summary>
        /// получение списка словарей(устаревший метод)
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Dictionary>> Get();
        Task<IEnumerable<Words>> Get(int idDict);
        Task<IEnumerable<Language>> GetLanguage();
        Task<IEnumerable<Dictionary>> GetLanguage(int idLang);
    }
}
