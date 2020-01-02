﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RepeatingWords.Annotations;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Xamarin.Forms;

namespace RepeatingWords.Model
{
   public class LanguageModel:ObservableCollection<DictionaryModel>,INotifyPropertyChanged
   {
       private IDialogService _dialogService;
       private IUnitOfWork _unitOfWork;
 
        public LanguageModel(IDialogService dialogService, IUnitOfWork unitOfWork ,Language language, IEnumerable<Dictionary> dictionaries = null, bool expanded = false)
        {
            _dialogService = dialogService;
            _unitOfWork = unitOfWork;
            Id = language.Id;
            Name = language.NameLanguage;
            AddDictionariesToCash(dictionaries);
            AddRangeToCollection();
            ExpandCommand = new Command( ExpandChange);
            AddCommand = new Command(async()=>
            {
                await AddDictionaryToLanguage();
            });
        }

        private void AddDictionariesToCash(IEnumerable<Dictionary> dictionaries)
        {
            if (dictionaries != null && dictionaries.Any())
                foreach (var dictionary in dictionaries)
                    _dictionariesCash.Add(new DictionaryModel(dictionary));
        }

        private void ExpandChange()
        {
            Expanded = !Expanded;
        }

        public ICommand ExpandCommand { get; set; }
        public ICommand AddCommand { get; set; }

        private void AddRangeToCollection()
        {
            for (int i = 0; i < _dictionariesCash.Count(); i++)
                this.Add(_dictionariesCash.ElementAt(i));
        }

        private ObservableCollection<DictionaryModel> _dictionariesCash = new ObservableCollection<DictionaryModel>();

        private int _id;
        public int Id { get=>_id;
            set { _id = value;OnPropertyChanged(nameof(Id)); }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name));}
        }

       
        public string Title
        {
            get => _name+" % ";
        }

        private bool _expanded;

        public bool Expanded
        {
            get => _expanded;
            set
            {
                if (_expanded != value)
                {
                    OnPropertyChanged(nameof(Expanded));
                    OnPropertyChanged(nameof(StateIcon));
                }
                if(_expanded)
                   AddRangeToCollection();
                else
                    this.Clear();
                _expanded = value;
            }
        }

        
        public string StateIcon
        {
            get
            {
                if (_expanded)
                    return "arrow_up.png";
                return "arrow_down.png";
            }
        }


        public async Task<bool> AddDictionaryToLanguage()
        {
            try
            {
                var result = await _dialogService.ShowInputTextDialog(Resource.EntryNameDict, Resource.ButtonAddDict, Resource.ButtonCreate, Resource.ModalActCancel);
                if (!string.IsNullOrEmpty(result) || !string.IsNullOrWhiteSpace(result))
                {
                    var dictionary = _unitOfWork.DictionaryRepository.Create(new Dictionary() { Id = 0, IdLanguage = Id,Name = result, PercentOfLearned = 0, LastUpdated = DateTime.UtcNow });
                    _unitOfWork.Save();
                    AddDictionary(dictionary);
                }
                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                return false;
            }
        }

      

        public async Task<bool> RemoveDictionaryFromLanguage(Dictionary dictionary)
        {
            try
            {
                var unlearned = GetUnlearningDictionary(dictionary.Name);
                if (unlearned != null)
                   await RemoveDictionaryFromLanguage(unlearned);
                var words = await Task.Run(() => _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == dictionary.Id).AsEnumerable());
                if (words != null && words.Any())
                    for (int i = 0; i < words.Count(); i++)
                        await Task.Run(() => _unitOfWork.WordsRepository.Delete(words.ElementAt(i)));
                bool success = await Task.Run(() => _unitOfWork.DictionaryRepository.Delete(dictionary));
                _unitOfWork.Save();
                RemoveDictionary(dictionary);
                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                return false;
            }
        }

        private void RemoveDictionary(Dictionary dictionary)
        {
            var dictionaryModel = _dictionariesCash.Where(x => x.Id == dictionary.Id).FirstOrDefault();
            if (dictionaryModel != null)
            {
                _dictionariesCash.Remove(dictionaryModel);
                this.Remove(dictionaryModel);
            }
        }
        private void AddDictionary(Dictionary dictionary)
        {
            var modelDict = new DictionaryModel(dictionary);
            _dictionariesCash.Add(modelDict);
            this.Add(modelDict);
        }

        private Dictionary GetUnlearningDictionary(string name)
        {
            return _unitOfWork.DictionaryRepository.Get().FirstOrDefault(x => x.Name.Equals(name + Resource.NotLearningPostfics, StringComparison.OrdinalIgnoreCase));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
