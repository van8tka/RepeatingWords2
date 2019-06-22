﻿using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Interfaces;
using System;
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
                var ter = _unitOfWork.DictionaryRepository.Get();
                if (_unitOfWork.DictionaryRepository.Get().Count()==0)
                {
                    int idDefdictionary = CreateDefaultDictionary();
                    CreateDefaultWords(idDefdictionary);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        private void CreateDefaultWords(int idDefdictionary)
        {
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "словарь", EngWord = "dictionary", Transcription = "[ˈdɪkʃəneri]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "книга", EngWord = "book", Transcription = "[bʊk]" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = idDefdictionary, RusWord = "стол", EngWord = "table", Transcription = "[teɪb(ə)l]" });
            _unitOfWork.Save();
        }

        private int CreateDefaultDictionary()
        {             
            var dic = _unitOfWork.DictionaryRepository.Create(new Dictionary()
            {
                Id = 0,
                Name = "ExampleDictionary"
            } );
            _unitOfWork.Save();
            return dic.Id;
        }
    }
}