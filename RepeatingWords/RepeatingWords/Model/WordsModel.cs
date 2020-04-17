using Newtonsoft.Json.Linq;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;

namespace RepeatingWords.Model
{
   public class WordsModel:BaseModel, ISerializebleJson
   {
       public WordsModel()
       {
           DictionaryParent = null;
           Id = 0;
           RusWord =string.Empty;
           EngWord = string.Empty;
            Transcription = string.Empty;
            IsLearned = false;
       }
       public WordsModel(DictionaryModel dictionaryParent, Words wordDb)
       {
           DictionaryParent = dictionaryParent;
           Id = wordDb.Id;
           RusWord = wordDb.RusWord;
           EngWord = wordDb.EngWord;
           Transcription = wordDb.Transcription;
           IsLearned = wordDb.IsLearned;
       }

       private DictionaryModel _dictionary;
       public DictionaryModel DictionaryParent { get=>_dictionary;
           set { _dictionary = value; OnPropertyChanged(nameof(DictionaryParent)); }
       }

       private string _rusWord;
       public string RusWord { get=>_rusWord;
           set { _rusWord = value; OnPropertyChanged(nameof(_rusWord)); }
       }

       private string _engWord;
       public string EngWord { get=>_engWord;
           set { _engWord = value;  OnPropertyChanged(nameof(EngWord)); }
       }

       private string _transcription;
       public string Transcription { get=>_transcription;
           set { _transcription = value;OnPropertyChanged(nameof(Transcription)); }
       }

       private bool _isLearned;
       public bool IsLearned { get=>_isLearned;
           set { _isLearned = value;OnPropertyChanged(nameof(IsLearned)); }
       }

       public JObject ToJson()
       {
           var item = new JObject();
           item.Add("id", Id);
           item.Add("is_learned", IsLearned);
           item.Add("native", RusWord);
           item.Add("foreign",EngWord);
           item.Add("transcript", string.IsNullOrEmpty(Transcription) ? string.Empty : Transcription);
           return item;
       }

       public T FromJson<T>(JObject jItem) where T : class
       {
           var item = new Words();
           item.Id = int.Parse(jItem["id"].ToString());
           item.IsLearned = bool.Parse(jItem["is_learned"].ToString());
           item.RusWord = jItem["native"].ToString();
           item.EngWord = jItem["foreign"].ToString();
           item.Transcription = jItem["transcript"]?.ToString() ?? string.Empty;
           return item as T;
       }
   }
}
