using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RepeatingWords.Annotations;
using RepeatingWords.DataService.Model;

namespace RepeatingWords.Model
{
   public class DictionaryModel:BaseModel
    {
        public DictionaryModel(Dictionary dictionary)
        {
            Id = dictionary.Id;
            Name = dictionary.Name;
            PercentOfLearned = dictionary.PercentOfLearned.ToString();
        }

     

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
