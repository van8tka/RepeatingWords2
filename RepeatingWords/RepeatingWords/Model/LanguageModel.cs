using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RepeatingWords.Annotations;
using RepeatingWords.DataService.Model;
using RepeatingWords.LoggerService;
using Xamarin.Forms;

namespace RepeatingWords.Model
{
   public class LanguageModel:ObservableCollection<DictionaryModel>,INotifyPropertyChanged
   {
       
        public LanguageModel( Language language, IEnumerable<Dictionary> dictionaries = null, bool expanded = false)
        {
            
            Id = language.Id;
            Name = language.NameLanguage;
            AddDictionariesToCash(dictionaries);
            AddRangeToCollection();
            ExpandCommand = new Command(ExpandChange);
            
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


        public void RemoveDictionary(Dictionary dictionary)
        {
            var dictionaryModel = _dictionariesCash.Where(x => x.Id == dictionary.Id).FirstOrDefault();
            if (dictionaryModel != null)
            {
                _dictionariesCash.Remove(dictionaryModel);
                this.Remove(dictionaryModel);
            }
        }
        public DictionaryModel AddDictionary(Dictionary dictionary)
        {
            var modelDict = new DictionaryModel(dictionary);
            _dictionariesCash.Add(modelDict);
            this.Add(modelDict);
            return modelDict;
        }

        public void UpdateDictionary(Dictionary dictionary)
        {
            try
            {
                var dictionaryToUpdate = _dictionariesCash.FirstOrDefault(x => x.Id == dictionary.Id);
                int index = _dictionariesCash.IndexOf(dictionaryToUpdate);
                _dictionariesCash.ElementAt(index).Id = dictionary.Id;
                _dictionariesCash.ElementAt(index).Name = dictionary.Name;
                _dictionariesCash.ElementAt(index).PercentOfLearned = dictionary.PercentOfLearned.ToString();
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       
   }
}
