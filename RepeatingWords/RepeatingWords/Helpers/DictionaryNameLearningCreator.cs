using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;

namespace RepeatingWords.Helpers
{
    internal class DictionaryNameLearningCreator:IDictionaryNameLearningCreator
    {
        public string CreateNameNotLearningDictionary(string name)
        {
            if (name.EndsWith(Resource.NotLearningPostfics))
                return name;
            else if (name.EndsWith(Constants.NAME_DB_FOR_CONTINUE))
            {
                return name.Replace(Constants.NAME_DB_FOR_CONTINUE, "") + Resource.NotLearningPostfics;
            }
            else
                return name + Resource.NotLearningPostfics;
        }

        public string CreateNameContinueDictionary(string name)
        {
            if (name.EndsWith(Constants.NAME_DB_FOR_CONTINUE))
                return name;
            else if (name.EndsWith(Resource.NotLearningPostfics))
            {
                return name.Replace(Resource.NotLearningPostfics, "") + Constants.NAME_DB_FOR_CONTINUE;
            }
            else
                return name + Constants.NAME_DB_FOR_CONTINUE;
        }
    }
}
