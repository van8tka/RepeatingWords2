using RepeatingWords.DataService.Model;
using RepeatingWords.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface IWebClient
    {
        /// <summary>
        /// получение списка словарей(устаревший метод)
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Dictionary>> Get();
        Task<IEnumerable<Words>> Get(int idDict);
        Task<IEnumerable<Language>> GetLanguage();
        /// <summary>
        /// метод получает JArray в виде:
        /// <param name="idLang">id выбранного языка</param>
        /// </summary>
        /// <returns> jarray[jarrayDictionary[jArrayWord,jarrayWord],jarrayDictionary..]
        ///[
        ///    [
        ///        "100 Basic Expressions",   имя Словаря
        ///        108,   Количество слов в словаре
        ///            [
        ///            "i", - оригинал
        ///            "я", - перевод
        ///            "[ya]" - транскрипция
        ///            ],
        ///            [
        ///            "you (informal)",
        ///            "ты",
        ///            "[tee]"
        ///            ]
        ///    ],
        ///    [
        ///        "Abstract Terms",
        ///        67,
        ///            [
        ///            "administration",
        ///            "управление",
        ///            "[upravleniye]"
        ///            ],
        ///            [
        ///            "advertising",
        ///            "реклама",
        ///            "[reklama]"
        ///            ]
        ///    ]
        ///]  
        /// </returns>

        Task<string> GetLanguageWords(int idLang);
        /// <summary>
        /// получение онлайн версии приложения  
        /// </summary>
        /// <returns></returns>
        Task<float> GetVersionApp();
    }
}
