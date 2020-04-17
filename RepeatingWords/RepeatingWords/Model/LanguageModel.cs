using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using RepeatingWords.Annotations;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using Xamarin.Forms;

namespace RepeatingWords.Model
{
   public class LanguageModel:ObservableCollection<DictionaryModel>,INotifyPropertyChanged, ISerializebleJson
    {
       
        public LanguageModel( Language language, IEnumerable<DictionaryModel> dictionaries = null, bool expanded = true)
        {
            Id = language.Id;
            Name = language.NameLanguage;
            AddDictionariesToCash(dictionaries);
            AddRangeToCollection();
            ExpandCommand = new Command(ExpandChange);
            Expanded = expanded;
        }

        private void AddDictionariesToCash(IEnumerable<DictionaryModel> dictionaries)
        {
            if (dictionaries != null && dictionaries.Any())
            {
                int count = dictionaries.Count();
                for (int i = 0; i < count; i++)
                    _dictionariesCash.Add(dictionaries.ElementAt(i));
            }
        }

        private void ExpandChange()
        {
            Expanded = !Expanded;
        }

        public ICommand ExpandCommand { get; set; }
      
        private void AddRangeToCollection()
        {
            int count = _dictionariesCash.Count();
            for (int i = 0; i < count ; i++)
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

       
        public string Title => _name+" % ";
        

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


        public void RemoveDictionary(int id)
        {
            var dictionaryModel = _dictionariesCash.FirstOrDefault(x => x.Id ==id);
            if (dictionaryModel != null)
            {
                _dictionariesCash.Remove(dictionaryModel);
                this.Remove(dictionaryModel);
            }
        }
        public void AddDictionary(DictionaryModel dictionary)
        {
            _dictionariesCash.Add(dictionary);
            this.Add(dictionary);
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public JObject ToJson()
        {
            var item = new JObject();
            item.Add("id", Id);
            item.Add("name", Name);
            var jarray = new JArray();
            int count = _dictionariesCash.Count();
            for (int i = 0; i < count; i++)
            {
                jarray.Add(_dictionariesCash.ElementAt(i).ToJson());
            }
            item.Add("dictionaries", jarray);
            return item;
        }

        public T FromJson<T>(JObject jItem) where T : class
        {
            var item = new Language();
            item.Id = int.Parse(jItem["id"].ToString());
            item.NameLanguage = jItem["name"].ToString();
            return item as T;
        }
    }
}
