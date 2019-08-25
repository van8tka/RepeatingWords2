using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepeatingWords.Helpers
{
    public class LanguageLoader: ILanguageLoaderFacade
    {

        public LanguageLoader(IWebApiService _webService, IUnitOfWork _unitOfWork)
        {
            this._webService = _webService ?? throw new ArgumentNullException(nameof(_webService));
            this._unitOfWork = _unitOfWork ?? throw new ArgumentNullException(nameof(_unitOfWork));
        }
        private readonly IWebApiService _webService;
        private readonly IUnitOfWork _unitOfWork;


        public async Task LoadSelectedLanguageToDB(Language selectedLanguage)
        {           
            try
            {
                IEnumerable<Dictionary> data = (await _webService.GetLanguage(selectedLanguage.Id))?.OrderBy(x => x.Name);
                if (data != null)
                {
                    for (int i = 0; i < data.Count(); i++)
                    {
                        await AddDictionaryToDb(data.ElementAt(i));
                    }
                }

            }
            catch (Exception e)
            {              
                throw;
            }
        }


        private async Task AddDictionaryToDb(Dictionary selectedDictionary)
        {
            try
            {                
                var words = (await _webService.Get(selectedDictionary.Id)).OrderBy(x => x.RusWord);
                int idNewDictionary = _unitOfWork.DictionaryRepository.Get().Last().Id + 1;
                var dictionary = _unitOfWork.DictionaryRepository.Create(new Dictionary() { Id = idNewDictionary, Name = selectedDictionary.Name });
                _unitOfWork.Save();
                CreateWords(words, idNewDictionary);
                _unitOfWork.Save();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private void CreateWords(IEnumerable<Words> words, int idNewDictionary)
        {
            try
            {
                var badSymbals = new char[] { ' ', '\r', '\n', '\t' };
                for (int i = 0; i < words.Count(); i++)
                {
                    var newWord = new Words();
                    var netWord = words.ElementAt(i);
                    newWord.Id = 0;
                    newWord.IdDictionary = idNewDictionary;
                    newWord.RusWord = netWord.RusWord.Trim(badSymbals);
                    newWord.Transcription = netWord.Transcription.Trim(badSymbals);
                    newWord.EngWord = netWord.EngWord.Trim(badSymbals);
                    _unitOfWork.WordsRepository.Create(newWord);
                }
            }
            catch (Exception e)
            {
                throw;
            }          
        }
    }
}
