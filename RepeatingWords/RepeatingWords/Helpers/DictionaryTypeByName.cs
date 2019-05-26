using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;

namespace RepeatingWords.Helpers
{
    public class DictionaryTypeByName:IDictionaryTypeByName
    {
        public bool IsUnlearningDictionary(string nameDictionary) => nameDictionary.EndsWith(Resource.NotLearningPostfics);

        public bool IsContinueDictionary(string nameDictionary) => nameDictionary.EndsWith(Constants.NAME_DB_FOR_CONTINUE);

        public bool IsContinueWithUnlearningDictionary(string nameDictionary) => nameDictionary.EndsWith(Resource.NotLearningPostfics + Constants.NAME_DB_FOR_CONTINUE);
    }
}
