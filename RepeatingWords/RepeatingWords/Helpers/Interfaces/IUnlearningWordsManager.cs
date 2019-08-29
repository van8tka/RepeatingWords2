using RepeatingWords.DataService.Model;
using System.Collections.Generic;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface IUnlearningWordsManager
    {
        int SaveUnlearningDictionary(string nameDictionary, IEnumerable<Words> wordsUnlearn, IEnumerable<Words> wordsLeft, IEnumerable<Words> wordsAll);
    }
}
