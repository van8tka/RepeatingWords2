using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RepeatingWords.Services
{
    public interface IImport
    {
        Task<JObject> import();
    }
    public class ImportDb:IImport
    {
        public ImportDb(IStudyService studyService)
        {
            _studyService = studyService;
        }

        private readonly IStudyService _studyService;
        public Task<JObject> import()
        {
            
        }
    }
}
