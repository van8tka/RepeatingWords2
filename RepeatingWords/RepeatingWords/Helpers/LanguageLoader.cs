using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RepeatingWords.Services;

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
                    await GetWordsByLanguage(idLanguageServer, newLanguageId);
                }
                _studyService.CommitTransaction();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                _studyService.RollBackTransaction();
            }
        }

        private async Task GetWordsByLanguage(int idLanguageServer, int newLanguageId)
        {
            var dataRaw = await _webService.GetLanguageWords(idLanguageServer);
            await Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(dataRaw) && !string.Equals(dataRaw, "[]", StringComparison.OrdinalIgnoreCase))
                {
                    var jDataRaw = JArray.Parse(dataRaw);
                    int count = jDataRaw.Count();
                    var listWords = new List<Words>();
                    for (int i = 0; i < count; i++)
                        AddDictionaryToDb(jDataRaw[i] as JArray, newLanguageId, listWords);
                    _studyService.AddWords(listWords.AsEnumerable());
                }
            });
        }

        private void AddDictionaryToDb(JArray jDictionary, int langId, IList<Words> listWords)
        {
            int idDictionary = _studyService.AddDictionary(jDictionary[0].ToString(), langId);
            int count = jDictionary.Count();
            var badSymbals = new char[] {' ', '\r', '\n', '\t'};
            for (int i = 2; i < count; i++)
                listWords.Add(CreateWords(jDictionary[i] as JArray, idDictionary, badSymbals));
        }

        private Words CreateWords(JArray jwords, int idNewDictionary, char[] badSymbals)
        {
            var newWord = new Words();
            newWord.IdDictionary = idNewDictionary;
            newWord.RusWord = jwords[0].ToString().Trim(badSymbals);
            newWord.Transcription = jwords[2].ToString().Trim(badSymbals);
            newWord.EngWord = jwords[1].ToString().Trim(badSymbals);
            newWord.IsLearned = false;
            return newWord;
        }
    }
}
