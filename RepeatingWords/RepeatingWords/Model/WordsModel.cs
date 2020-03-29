namespace RepeatingWords.Model
{
   public class WordsModel:BaseModel
   {
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
    }
}
