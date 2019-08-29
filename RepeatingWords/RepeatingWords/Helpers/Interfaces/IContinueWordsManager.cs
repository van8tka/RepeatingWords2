using RepeatingWords.DataService.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface IContinueWordsManager
    {
        int SaveContinueDictionary(string nameDictionary, IList<Words> words, bool isFromNative);
        bool RemoveContinueDictionary();
    }
}
