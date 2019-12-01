using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepeatingWords.Helpers.Interfaces;

namespace RepeatingWords.Helpers
{
    /// <summary>
    /// check entry word with original translate,
    /// if some original words, check to exist entry word with any original word
    /// </summary>
   public class EntryWordValidator:IEntryWordValidator
    {
        public bool IsValidWord(string entryWord, string originalWord)
        {
            if (string.IsNullOrEmpty(entryWord) || string.IsNullOrWhiteSpace(entryWord))
                return false;
            entryWord = TrimSpecialSymbals( entryWord );
            originalWord = TrimSpecialSymbals( originalWord );
            if (string.Equals(entryWord, originalWord, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                var separators = new char[] {',', '\\', '/', '(', ')', ';'};
                if (ContainsSeparator(entryWord, originalWord, separators))
                {
                    var originalArray = originalWord.Split(separators);
                    var entryArray = entryWord.Split(separators);
                    var originalArrayTrim = ArrayTrimSpecialSymbals(originalArray);
                    var entryArrayTrim = ArrayTrimSpecialSymbals(entryArray);
                    foreach (var original in originalArrayTrim)
                    {
                        if (entryArrayTrim.Any(x => x.Equals(original, StringComparison.OrdinalIgnoreCase)))
                            return true;
                    }
                }
            }
            return false;
        }

        public string ClearEntryWord(string entryWord, string originalWord)
        {
            if (string.IsNullOrEmpty(entryWord) || string.IsNullOrWhiteSpace(entryWord))
                return string.Empty;
            entryWord = TrimSpecialSymbals(entryWord);
            originalWord = TrimSpecialSymbals(originalWord);
            StringBuilder newEntryWord = new StringBuilder();
            for (int i = 0; i < entryWord.Length && i< originalWord.Length; i++)
            {
                Char entry = Char.ToLowerInvariant(entryWord[i]);
                Char origin = Char.ToLowerInvariant(originalWord[i]);
                if (entry.Equals(origin))
                    newEntryWord.Append(entry);
                else
                    return newEntryWord.ToString();
            }
            return newEntryWord.ToString();
        }

        private IEnumerable<string> ArrayTrimSpecialSymbals(string []original)
        {
            var list = new List<string>();
            foreach (var word in original)
            {
                var trimWord = TrimSpecialSymbals(word);
                if (!string.IsNullOrEmpty(trimWord))
                    list.Add(trimWord);
            }
            return list;
        }

        private string TrimSpecialSymbals(string original)
        {
            var badSymbals = new char[] {' ', '\r', '\n', '\t', '(',')',';'};
            return original.Trim(badSymbals);
        }

        private bool ContainsSeparator(string entryWord, string originalWord, char[] separators)
        {
            for (int i = 0; i < separators.Length; i++)
            {
                if (originalWord.Contains(separators[i]) || entryWord.Contains(separators[i]))
                    return true;
            }
            return false;
        }

       
    }
}
