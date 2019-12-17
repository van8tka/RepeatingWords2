using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using RepeatingWords.Annotations;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers;
using Xamarin.Forms;

namespace RepeatingWords.Model
{
   public class LanguageModel:ObservableCollection<DictionaryModel>,INotifyPropertyChanged
    {
       

        public LanguageModel(){}

        public LanguageModel(Language language, IEnumerable<Dictionary> dictionaries, bool expanded = false)
        {
            Id = language.Id;
            Name = language.NameLanguage;
            foreach (var dictionary in dictionaries)
            {
                _dictionariesCash.Add(new DictionaryModel( dictionary ));
            }
            AddRangeToCollection();
            ExpandCommand = new Command( ExpandChange);
        }

        private void ExpandChange()
        {
            Expanded = !Expanded;
        }

        public ICommand ExpandCommand { get; set; }

        private void AddRangeToCollection()
        {
            for (int i = 0; i < _dictionariesCash.Count(); i++)
            {
                this.Add(_dictionariesCash.ElementAt(i));
            }
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
