﻿using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RepeatingWords.Helpers.Interfaces;

namespace RepeatingWords.Services
{
 
    public class ExportJsonDb:IExport
    {
        public ExportJsonDb(IStudyService studyService)
        {
            _studyService = studyService;
        }

        private readonly IStudyService _studyService;
        public Task<JObject> Export()
        {
            return Task.Run(() =>
            {
                var jobject = new JObject();
                jobject.Add("version_json", "1");
                var jarrayall = new JArray();
                var languages = _studyService.DictionaryList;
                Parallel.ForEach(languages, (language) => { jarrayall.Add(language.ToJson()); });
                jobject.Add("languages",jarrayall);
                return jobject;
            });
        }
    }
}
