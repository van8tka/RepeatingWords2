using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void LoadDefaultData()
        {
            try
            {
                if (_unitOfWork.DictionaryRepository.Get().AsEnumerable().Any())
                {
                    //исходные данные для инициализации БД
                    var dictionary = new Dictionary()
                    {
                        Id = 0,
                        Name = "ExampleDictionary"
                    };

                    dictionary = _unitOfWork.DictionaryRepository.Create(dictionary);
                    var listWords = new List<Words>()
                    {
                       new Words() {Id=0,IdDictionary=dictionary.Id,RusWord="словарь", EngWord="dictionary", Transcription= "[ˈdɪkʃəneri]" },
                       new Words() { Id = 0, IdDictionary = dictionary.Id, RusWord = "книга", EngWord = "book", Transcription = "[bʊk]" },
                       new Words() { Id = 0, IdDictionary = dictionary.Id, RusWord = "стол", EngWord = "table", Transcription = "[teɪb(ə)l]" },
                    };
                    foreach (var word in listWords)
                    {
                        _unitOfWork.WordsRepository.Create(word);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }
    }
}
