using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;

namespace RepeatingWords.Helpers
{
    public class DictionaryNameLearningCreator:IDictionaryNameLearningCreator
    {
        public string CreateNameNotLearningDictionary(string name)
        {
            if (name.EndsWith(Resource.NotLearningPostfics))
                return name;
            else if (name.EndsWith(Constants.NAME_DB_FOR_CONTINUE))
            {
                return CreateNameNotLearningDictionary(name.Replace(Constants.NAME_DB_FOR_CONTINUE, ""));
            }
            else
                return name + Resource.NotLearningPostfics;
        }

        public string CreateNameContinueDictionary(string name)
        {
            if (name.EndsWith(Constants.NAME_DB_FOR_CONTINUE))
                return name;
            else
                return name + Constants.NAME_DB_FOR_CONTINUE;
        }
    }
}
