using RepeatingWords.DataService.Model;
using System.Collections.Generic;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface IContinueWordsService
    {
        int SaveContinueDictionary(string nameDictionary, IEnumerable<Words> words, bool isFromNative);
        bool RemoveContinueDictionary(IEnumerable<Words> words);
    }
}
