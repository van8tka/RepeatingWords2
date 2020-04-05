using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RepeatingWords.Services;
using Xamarin.Forms.Internals;
using RepeatingWords.LoggerService;
using Log = RepeatingWords.LoggerService.Log;

namespace RepeatingWords.Helpers
{
    public class LanguageLoader : ILanguageLoaderFacade
    {

        public LanguageLoader(IWebClient webService, IStudyService studyService)
        {
            this._webService = webService ?? throw new ArgumentNullException(nameof(webService));
            this._studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
        }

        private readonly IWebClient _webService;
        private readonly IStudyService _studyService;


        public async Task LoadLanguageFromApi(int idLanguageServer, string nameLanguage)
        {
            try
            {
                _studyService.BeginTransaction();
                var isExist = _studyService.GetLanguage(nameLanguage) != null;
                if (!isExist)
                {
                    int newLanguageId = _studyService.AddLanguage(nameLanguage);
                    var dataRaw = await _webService.GetLanguageWords(idLanguageServer);
                    if (!string.IsNullOrEmpty(dataRaw) &&
                        !string.Equals(dataRaw, "[]", StringComparison.OrdinalIgnoreCase))
                    {
                        await GetWordsByLanguage(dataRaw, newLanguageId);
                    }
                }
                _studyService.CommitTransaction();
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                _studyService.RollBackTransaction();
            }
        }

        private Task GetWordsByLanguage(string dataRaw, int newLanguageId)
        {
            return Task.Run(() =>
            {
                var jDataRaw = JArray.Parse(dataRaw);
                int count = jDataRaw.Count();
             
                for (int i = 0; i < count; i++)
                {
                    var listWords = new List<Words>();
                    AddDictionaryToDb(jDataRaw[i] as JArray, newLanguageId, listWords);
                    _studyService.AddWords(listWords);
                }
            });
        }

        private void AddDictionaryToDb(JArray jDictionary, int langId, IList<Words> listWords)
        {
            int idDictionary = _studyService.AddDictionary(jDictionary[0].ToString(), langId);
            int count = jDictionary.Count();
            for (int i = 2; i < count; i++)
                listWords.Add(CreateWords(jDictionary[i] as JArray, idDictionary));
        }

        private Words CreateWords(JArray jwords, int idNewDictionary)
        {
            var newWord = new Words();
            newWord.IdDictionary = idNewDictionary;
            newWord.RusWord = jwords[0].ToString();
            newWord.Transcription = jwords[2].ToString();
            newWord.EngWord = jwords[1].ToString();
            newWord.IsLearned = false;
            return newWord;
        }
    }
}
