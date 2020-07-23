using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Model;
using RepeatingWords.Services;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class CreateWordViewModel : ViewModelBase
    {
        private readonly IStudyService _studyService;
        private readonly IKeyboardTranscriptionService _keyboardService;
        public CreateWordViewModel(INavigationService navigationServcie, IDialogService dialogService, IStudyService studyService, IKeyboardTranscriptionService keyboardService) : base(navigationServcie, dialogService)
        {
            _keyboardService = keyboardService;
            _studyService = studyService;
            SendCommand = new Command(async()=>await SendWord());
            FocusedTranscriptionCommand = new Command(async()=>await FocusedTranscription());
        }
      
        public override Task InitializeAsync(object navigationData)
        {
            Log.Logger.Info("\n InitializeAsync CreateWordViewModel");
            IsBusy = true;
            if (navigationData is WordsModel word)
            {
                if (word.Id > -1)
                {
                    _isChangeWord = true;
                    TitleEditWord = Resource.TitleChangeWord;
                }
                else
                {
                    _isChangeWord = false;
                    TitleEditWord = Resource.TitleCreateWord;
                }
                NativeWord = word.RusWord;
                TranslateWord = word.EngWord;
                TranscriptionWord = word.Transcription;
                _wordChange = word;
                _dictionary = word.DictionaryParent;
            }
            else
            {
                TitleEditWord = Resource.TitleCreateWord;             
                _dictionary = navigationData as DictionaryModel;
                _isChangeWord = false;
            }
            MessagingCenter.Send<CreateWordViewModel>(this, "SetFocus");
            return base.InitializeAsync(navigationData);
        }

        private bool _isChangeWord { get; set; }
        private DictionaryModel _dictionary;
        private WordsModel _wordChange;



        private string _titleEditWord;       
        public string TitleEditWord { get=>_titleEditWord; set { _titleEditWord = value; OnPropertyChanged(nameof(TitleEditWord)); } }

        private string _nativeWord;
        private string _translateWord;
        private string _transcriptionWord;
        public string NativeWord { get => _nativeWord; set { _nativeWord = value; OnPropertyChanged(nameof(NativeWord)); } }
        public string TranslateWord { get => _translateWord; set { _translateWord = value; OnPropertyChanged(nameof(TranslateWord)); } }
        public string TranscriptionWord { get => _transcriptionWord; set { _transcriptionWord = value; OnPropertyChanged(nameof(TranscriptionWord)); } }

        public ICommand SendCommand { get; set; }
        public ICommand FocusedTranscriptionCommand { get; set; }

        private async Task SendWord()
        {
            try
            {
                Log.Logger.Info("\n Send new word");
                if (!String.IsNullOrEmpty(NativeWord) && !String.IsNullOrEmpty(TranslateWord))
                {
                    if (!String.IsNullOrEmpty(TranscriptionWord))
                    {
                        if (!TranscriptionWord.StartsWith("["))
                            TranscriptionWord = "[" + TranscriptionWord;
                        if (!TranscriptionWord.EndsWith("]"))
                            TranscriptionWord = TranscriptionWord + "]";
                    }
                    else
                    {
                        TranscriptionWord = "[]";
                    }
                   
                   CreateWord(); 
                   await GoBack();
                }
                else
                {
                      DialogService.ShowToast(Resource.ModelNoFillFull);
                }
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
            }
        }

        private async Task GoBack()
        {
            var lastPage = NavigationService.PreviousPageViewModel;
            if (lastPage is WordsListViewModel || lastPage is EntryTranscriptionViewModel)
            {
               Log.Logger.Info("Previous page is WordsListViewModel or EntryTranscriptionViewModel");
               await NavigationService.RemoveLastFromBackStackAsync();
               await NavigationService.NavigateToAsync<WordsListViewModel>(_dictionary);
               await NavigationService.RemoveLastFromBackStackAsync();
            }
            else
            {
                Log.Logger.Info("Previous page GoBackPage");
                await NavigationService.GoBackPage();
            }
        }

        private void CreateWord()
        {
            if (!_isChangeWord)
            {
                Log.Logger.Info("\n Create new WordsModel");
                var model = new WordsModel();
                model.DictionaryParent = _dictionary;
                model.RusWord = NativeWord;
                model.EngWord = TranslateWord;
                model.Transcription = TranscriptionWord; 
                model = _studyService.AddWord(model);
                _dictionary.WordsCollection.Add(model);
            }
            else
            {
                Log.Logger.Info("\n Update WordsModel");
                SetDataToChangeWord();
                _studyService.UpdateWord(_wordChange);
            }
        }

        private async Task FocusedTranscription()
        {
            Log.Logger.Info("\n Transcripton field get focus");
            if (_keyboardService.GetCurrentTranscriptionKeyboard())
            {
                DependencyService.Get<IKeyboardService>().HideKeyboard();
                await NavigationService.NavigateToAsync<EntryTranscriptionViewModel>(TranscriptionWord);
            }
        }

        private void SetDataToChangeWord()
        {
            _wordChange.RusWord = NativeWord;
            _wordChange.EngWord = TranslateWord;
            _wordChange.Transcription = TranscriptionWord;
            _wordChange.DictionaryParent = _dictionary;
        }
    }
}
