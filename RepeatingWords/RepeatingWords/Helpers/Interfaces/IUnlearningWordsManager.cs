using RepeatingWords.DataService.Model;
using System.Collections.Generic;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface IUnlearningWordsManager
    {
        int SaveDictionary(string nameDictionary, IEnumerable<Words> words);
        void CreateLastAction(int idDictionary, bool isFromNative);
    }
}
