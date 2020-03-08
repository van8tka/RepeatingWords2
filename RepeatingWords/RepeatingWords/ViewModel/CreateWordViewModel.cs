using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FormsControls.Base;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Services;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class CreateWordViewModel : ViewModelBase
    {
        private readonly IDictionaryStudyService _studyService;
        private readonly IKeyboardTranscriptionService _keyboardService;
        public CreateWordViewModel(INavigationService navigationServcie, IDialogService dialogService, IDictionaryStudyService studyService, IKeyboardTranscriptionService keyboardService) : base(navigationServcie, dialogService)
        {
            _keyboardService = keyboardService;
            _studyService = studyService;
            SendCommand = new Command(SendWord);
            FocusedTranscriptionCommand = new Command(FocusedTranscription);
        }
      
        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            if (navigationData is Words word)
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
                _dictionary = _studyService.GetDictionary(word.IdDictionary);
            }
            else if (navigationData is Dictionary dictionary)
            {
                 TitleEditWord = Resource.TitleCreateWord;             
                _dictionary = dictionary;
                _isChangeWord = false;
            }               
            else
                throw new Exception("Can't initialize CreateWordViewModel, bad input parameter");
            return base.InitializeAsync(navigationData);
        }

        private bool _isChangeWord { get; set; }
        private Dictionary _dictionary;
        private Words _wordChange;



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

        private async void SendWord()
        {
            try
            {               
                string ModelNoFillFull = Resource.ModelNoFillFull;
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
                    var word = CreateWord();                   
                    await GoBack();
                }
                else
                {
                      DialogService.ShowToast(ModelNoFillFull);
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
                await NavigationService.RemoveLastFromBackStackAsync();
                await NavigationService.NavigateToAsync<WordsListViewModel>(_dictionary);
                await NavigationService.RemoveLastFromBackStackAsync();
            }
            else
            {
                await NavigationService.GoBackPage();
            }
        }

        private Words CreateWord()
        {
            try
            {
                if (!_isChangeWord)
                {
                    var word = new Words()
                    {
                        Id = 0,
                        IdDictionary = _dictionary.Id,
                        RusWord = NativeWord,
                        EngWord = TranslateWord,
                        Transcription = TranscriptionWord
                    };
                    word = _studyService.AddWord(word);
                    return word;
                }
                SetDataToChangeWord();
                _studyService.UpdateWord(_wordChange);
                return _wordChange;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }            
        }

        private async void FocusedTranscription()
        {
            try
            {
                if (_keyboardService.GetCurrentTranscriptionKeyboard())
                {
                    DependencyService.Get<IKeyboardService>().HideKeyboard();
                    if (_isChangeWord)
                    {
                        SetDataToChangeWord();
                        _wordChange.IdDictionary = _dictionary.Id;
                        await NavigationService.NavigateToAsync<EntryTranscriptionViewModel>(_wordChange);
                    }
                    else
                        await NavigationService.NavigateToAsync<EntryTranscriptionViewModel>(new Words() { Id = -1, Transcription = TranscriptionWord, RusWord = NativeWord, EngWord = TranslateWord, IdDictionary = _dictionary.Id });                  
                }
            }
            catch(Exception e)
            {              
                Log.Logger.Error(e);
            }
        }

        private void SetDataToChangeWord()
        {
            _wordChange.RusWord = NativeWord;
            _wordChange.EngWord = TranslateWord;
            _wordChange.Transcription = TranscriptionWord;
        }
    }
}
