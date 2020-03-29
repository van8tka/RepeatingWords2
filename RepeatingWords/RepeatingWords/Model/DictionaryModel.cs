using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RepeatingWords.DataService.Model;

namespace RepeatingWords.Model
{
   public class DictionaryModel:BaseModel
    {
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
            set { _words = value; OnPropertyChanged(nameof(WordsCollection)); }
        }

        public int CountWords => WordsCollection.Count();
        public int CountUnlearned => WordsCollection.Count(x => x.IsLearned==false);
    }
}
