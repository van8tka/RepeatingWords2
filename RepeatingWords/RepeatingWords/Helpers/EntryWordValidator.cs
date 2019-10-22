using System;
using RepeatingWords.Helpers.Interfaces;

namespace RepeatingWords.Helpers
{
   public class EntryWordValidator:IEntryWordValidator
    {
        public bool IsValidWord(string entryWord, string originalWord)
        {
            entryWord = entryWord.Trim();
            originalWord = originalWord.Trim();

            if (string.Equals(entryWord, originalWord, StringComparison.OrdinalIgnoreCase))
                return true;
            else
               return false;
        }
    }
}
