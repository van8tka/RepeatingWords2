using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Linq;

namespace RepeatingWords.Services
{
    public class InitDefaultDb : IInitDefaultDb
    {
        private readonly IUnitOfWork _unitOfWork;    

        public InitDefaultDb(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));          
        }

        public bool LoadDefaultData()
        {
            try
            {              
                if (_unitOfWork.DictionaryRepository.Get().Count()==0)
                {
                    Log.Logger.Info("Init new database");
                    int idDefdictionary = CreateDefaultDictionary();
                    CreateDefaultWords(idDefdictionary);
                }
                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;              
            }
        }


        private void CreateDefaultWords(int idDefdictionary)
        {
            if(string.Equals( Resource.IsCurrentLang, "ru"))
            {
                CreateDefaultRussianWords( idDefdictionary );
            }
            else
            {
                CreateDefaultEnglishWords( idDefdictionary );
            }
        }



        private void CreateDefaultRussianWords(int idDefdictionary)
        {
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "словарь", EngWord = "dictionary", Transcription = "[ˈdɪkʃəneri]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "книга", EngWord = "book", Transcription = "[bʊk]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "стол", EngWord = "table", Transcription = "[teɪb(ə)l]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "ручка", EngWord = "pen", Transcription = "[pen]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "примечание", EngWord = "note", Transcription = "[nəut]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "человек", EngWord = "people", Transcription = "[ˈpi:pl]" });
            _unitOfWork.Save();
        }


        private void CreateDefaultEnglishWords(int idDefdictionary)
        {
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "dictionary", EngWord = "ручка", Transcription = "[ˈdɪkʃəneri]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "book", EngWord = "примечание", Transcription = "[bʊk]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "table", EngWord = "человек", Transcription = "[teɪb(ə)l]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "pen", EngWord = "table", Transcription = "[pen]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "note", EngWord = "человек", Transcription = "[nəut]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "people", EngWord = "people", Transcription = "[ˈpi:pl]" });
            _unitOfWork.Save();
        }

        private int CreateDefaultDictionary()
        {             
            var dic = _unitOfWork.DictionaryRepository.Create(new Dictionary()
            {
                Id = 0,
                Name = Resource.DefaultDictionaryName
            });
            _unitOfWork.Save();
            return dic.Id;
        }
    }
}
