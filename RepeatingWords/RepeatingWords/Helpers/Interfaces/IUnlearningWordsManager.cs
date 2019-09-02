using RepeatingWords.DataService.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface IUnlearningWordsManager
    {
       Task<int> SaveUnlearningDictionary(string nameDictionary, IEnumerable<Words> wordsUnlearn, IEnumerable<Words> wordsLeft, IEnumerable<Words> wordsAll);
    }
}
