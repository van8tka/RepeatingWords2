using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Linq;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using Xamarin.Forms;

namespace RepeatingWords.Model
{
   public class DictionaryModel:BaseModel, ISerializebleJson
    {
        public DictionaryModel() { }
        public DictionaryModel(Dictionary dictionary, IEnumerable<Words> wordsDb)
        {
            Id = dictionary.Id;
            Name = dictionary.Name;
            PercentOfLearned = dictionary.PercentOfLearned.ToString();
            LastUpdated = dictionary.LastUpdated;
            IdLanguage = dictionary.IdLanguage;
            WordsCollection = GetCollectionWordsFromRawData(wordsDb);
        }
 


        ObservableCollection<WordsModel> GetCollectionWordsFromRawData(IEnumerable<Words> words)
        {
            var temp = new ObservableCollection<WordsModel>();
            int count = words.Count();
            for (int i = 0; i < count; i++)
            {
                var model = new WordsModel(this,words.ElementAt(i));
                temp.Add(model);
            }
            return temp;
        }


        /// <summary>
        /// properties
        /// </summary>
        private string _name;
        public string Name { get=>_name;
            set { _name = value;OnPropertyChanged(nameof(Name));}
        }
        private string _percent;
        public string PercentOfLearned
        {
            get => _percent;
            set
            {
                if (int.Parse(value) <= 0)
                    _percent = string.Empty;
                else
                    _percent = value + "%";
                OnPropertyChanged(nameof(PercentOfLearned));
            }
        }

        private int _idLanguage;
        public int IdLanguage
        {
            get => _idLanguage;
            set { _idLanguage = value;OnPropertyChanged(nameof(IdLanguage)); }
        }


        private DateTime _lastUpdated;
        public DateTime LastUpdated
        {
            get => _lastUpdated;
            set { _lastUpdated = value;OnPropertyChanged(nameof(LastUpdated)); }
        }


        private ObservableCollection<WordsModel> _words;

        public ObservableCollection<WordsModel> WordsCollection
        {
            get => _words;
            set { _words = value;
                OnPropertyChanged(nameof(WordsCollection));
            }
        }

        public IEnumerable<WordsModel> WordsUnlearnedCollection
        {
            get=> WordsCollection.Where(x => x.IsLearned == false);
        }
        public IEnumerable<WordsModel> WordsLearnedCollection
        {
            get => WordsCollection.Where(x => x.IsLearned == true);
        }
        
        public bool IsStudyUnlearnedWords { get; set; }

        public int CountWords => WordsCollection.Count();
        public int CountUnlearned => WordsCollection.Count(x => x.IsLearned==false);
        public int CountLearned => WordsCollection.Count(x => x.IsLearned == true);
        public JObject ToJson()
        {
            var item = new JObject();
            item.Add("id", Id);
            item.Add("name", Name);
            item.Add("percent", string.IsNullOrEmpty(PercentOfLearned) ? string.Empty : PercentOfLearned.TrimEnd('%'));
            item.Add("updated", LastUpdated.ToString());
            var jarray = new JArray();
            for (int i = 0; i < CountWords; i++)
            {
                jarray.Add(WordsCollection.ElementAt(i).ToJson());
            }
            item.Add("words",jarray);
            return item;
        }

        public T FromJson<T>(JObject jItem) where T : class
        {
            var item = new Dictionary();
            item.Id = int.Parse(jItem["id"].ToString());
            item.Name = jItem["name"].ToString();
            item.LastUpdated = DateTime.Parse(jItem["updated"].ToString());
            int percent = 0;
            int.TryParse(jItem["percent"].ToString(), out percent);
            item.PercentOfLearned = percent;
            return item as T;
        }
    }
}
