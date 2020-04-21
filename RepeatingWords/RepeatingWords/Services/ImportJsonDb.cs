using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RepeatingWords.DataService.Model;
using RepeatingWords.LoggerService;
using RepeatingWords.Model;

namespace RepeatingWords.Services
{
    public interface IImport
    {
        Task<bool> import(JObject jobject);
    }
    public class ImportJsonDb:IImport
    {
        public ImportJsonDb(IStudyService studyService)
        {
            _studyService = studyService;
        }

        private readonly IStudyService _studyService;
        public async Task<bool> import(JObject jobject)
        {
            if (! _studyService.ClearDB())
                throw new Exception(Environment.NewLine + "Error clear Db");
            return await Task.Run(() =>
            {
                try
                {
                    var version = jobject["version_json"].ToString();
                    Log.Logger.Info($"version of json import: {version}");
                    JArray jarrayall = jobject["languages"] as JArray;
                    int count = jarrayall.Count;
                    _studyService.BeginTransaction();
                    var languageModel = new LanguageModel();
                    var dictionaryModel = new DictionaryModel();
                    var wordModel = new WordsModel();
                    for (int i = 0; i < count; i++)
                    {
                        //languages
                        var jobj_language = jarrayall.ElementAt(i) as JObject;
                        var language = languageModel.FromJson<Language>(jobj_language);
                        int id_language = _studyService.AddLanguage(language);
                        //dictionaries
                        var jarr_dictionary = jobj_language["dictionaries"] as JArray;
                        int count_dict = jarr_dictionary.Count;
                        for (int j = 0; j < count_dict; j++)
                        {
                            var jobj_dict = jarr_dictionary.ElementAt(j) as JObject;
                            var dictionary = dictionaryModel.FromJson<Dictionary>(jobj_dict);
                            dictionary.IdLanguage = id_language;
                            int id_dictionary = _studyService.AddDictionary(dictionary);
                            //words
                            var jarr_words = jobj_dict["words"] as JArray;
                            int count_words = jarr_words.Count;
                            var listWords = new List<Words>();
                            for (int k = 0; k < count_words; k++)
                            {
                                var jobj_word = jarr_words.ElementAt(k) as JObject;
                                var word = wordModel.FromJson<Words>(jobj_word);
                                word.IdDictionary = id_dictionary;
                                listWords.Add(word);
                            }
                            if(listWords.Any())
                                _studyService.AddWords(listWords);
                        }
                    }
                    _studyService.CommitTransaction();
                    return true;
                }
                catch (Exception e)
                {
                    _studyService.RollBackTransaction();
                    Log.Logger.Error(e);
                    return false;
                }
            });
        }
    }
}
