using RepeatingWords.DataService.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface IContinueWordsManager
    {
        Task<int> SaveContinueDictionary(string nameDictionary, IList<Words> words, bool isFromNative);
        Task<bool> RemoveContinueDictionary();
    }
}
