namespace RepeatingWords.Helpers.Interfaces
{
    public interface IDictionaryTypeByName
    {
         bool IsUnlearningDictionary(string nameDictionary);

         bool IsContinueDictionary(string nameDictionary);

         bool IsContinueWithUnlearningDictionary(string nameDictionary);
    }
}
