namespace RepeatingWords.Helpers.Interfaces
{
    public interface IEntryWordValidator
    {
        bool IsValidWord(string entryWord, string originalWord);
        string ClearEntryWord(string entryWord, string originalWord);
    }
}
