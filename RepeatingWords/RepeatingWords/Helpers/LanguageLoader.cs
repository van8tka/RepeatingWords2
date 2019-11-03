using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RepeatingWords.Helpers
{
    public class LanguageLoader: ILanguageLoaderFacade
    {

        public LanguageLoader(IWebClient _webService, IUnitOfWork _unitOfWork)
        {
            this._webService = _webService ?? throw new ArgumentNullException(nameof(_webService));
            this._unitOfWork = _unitOfWork ?? throw new ArgumentNullException(nameof(_unitOfWork));
        }
        private readonly IWebClient _webService;
        private readonly IUnitOfWork _unitOfWork;


        public async Task LoadSelectedLanguageToDB(int idLanguage)
        {           
            try
            {
                var dataRaw = await _webService.GetLanguageWords(idLanguage);
                if (!string.IsNullOrEmpty(dataRaw) && !string.Equals(dataRaw, "[]", StringComparison.OrdinalIgnoreCase))
                {
                    var jDataRaw = JArray.Parse(dataRaw);
                    for (int i = 0; i < jDataRaw.Count(); i++)
                    {
                        await AddDictionaryToDb(jDataRaw[i] as JArray);
                    }
                }
            }
            catch (Exception e)
            {      
                Debug.WriteLine(e);
                throw;
            }
        }


        private Task AddDictionaryToDb(JArray jDictionary)
        {
            try
            {
                if (jDictionary == null)
                    throw new Exception("JSon dictionary is empty");
                return Task.Run(() =>
                {
                    int idNewDictionary = _unitOfWork.DictionaryRepository.Get().Last().Id + 1;
                    var dictionary = _unitOfWork.DictionaryRepository.Create(new Dictionary() { Id = idNewDictionary, Name = jDictionary[0].ToString() });
                    _unitOfWork.Save();
                    for(int i=2;i<jDictionary.Count();i++)
                         CreateWords(jDictionary[i] as JArray, idNewDictionary);
                    _unitOfWork.Save();
                });              
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        private void CreateWords(JArray jwords, int idNewDictionary)
        {
            try
            {
                if (jwords != null && jwords.Count() == 3)
                {
                    var badSymbals = new char[] { ' ', '\r', '\n', '\t' };
                    var newWord = new Words();
                    newWord.Id = 0;
                    newWord.IdDictionary = idNewDictionary;
                    newWord.RusWord = jwords[0].ToString().Trim(badSymbals);
                    newWord.Transcription = jwords[2].ToString().Trim(badSymbals);
                    newWord.EngWord = jwords[1].ToString().Trim(badSymbals);
                    _unitOfWork.WordsRepository.Create(newWord);
                }
                else
                     throw new ArgumentException(nameof(jwords));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }          
        }
    }
}
