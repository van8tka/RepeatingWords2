using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using RepeatingWords.Annotations;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers;

namespace RepeatingWords.Model
{
   public class LanguageModel:INotifyPropertyChanged
    {
       

        public LanguageModel(){}

        public LanguageModel(Language language, IEnumerable<Dictionary> dictionaries, bool expanded = false)
        {
            Id = language.Id;
            Name = language.NameLanguage;
            Dictionaries = new ObservableCollection<DictionaryModel>();
            foreach (var dictionary in dictionaries)
            {
                _dictionariesCash.Add(new DictionaryModel( dictionary ));
            }
            if(expanded)
                Dictionaries = _dictionariesCash;
        }

        private ObservableCollection<DictionaryModel> _dictionariesCash = new ObservableCollection<DictionaryModel>();

        private int _id;
        public int Id { get=>_id;
            set { _id = value;OnPropertyChanged(nameof(Id)); }
        }

        private string _name;
        public string Name
        {
            get => Name;
            set { _name = value; OnPropertyChanged(nameof(Name));}
        }

        private ObservableCollection<DictionaryModel> _dictionariesOfLang;
        public ObservableCollection<DictionaryModel> Dictionaries
        {
            get => _dictionariesOfLang;
            set
            {
                _dictionariesOfLang = value;
                OnPropertyChanged(nameof(Dictionaries));
            }
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
                    Dictionaries = _dictionariesCash;
                else
                    Dictionaries.Clear();
                OnPropertyChanged(nameof(Dictionaries));
            }
        }

        
        public string StateIcon
        {
            get
            {
                if (_expanded)
                    return "arrow_up.png";
                else
                    return "arrow_down.png";
            }
        }


        public void AddDictionaryToLanguage(Dictionary newDictionary)
        {
            throw new NotImplementedException();
        }

        public void RemoveDictionaryFromLanguage(int idDictionary)
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
