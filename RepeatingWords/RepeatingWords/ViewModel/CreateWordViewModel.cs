using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class CreateWordViewModel : ViewModelBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IKeyboardTranscriptionService _keyboardService;
        public CreateWordViewModel(INavigationService navigationServcie, IDialogService dialogService, IUnitOfWork unitOfWork, IKeyboardTranscriptionService keyboardService) : base(navigationServcie, dialogService)
        {
            _keyboardService = keyboardService;
            _unitOfWork = unitOfWork;
            SendCommand = new Command(SendWord);
            FocusedTranscriptionCommand = new Command(FocusedTranscription);
        }
      
        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            if (navigationData is Words word)
            {
                if (word.Id > -1)
                    _isChangeWord = true;
                TitleEditWord = Resource.TitleChangeWord;
                NativeWord = word.RusWord;
                TranslateWord = word.EngWord;
                TranscriptionWord = word.Transcription;              
                _wordChange = word;
                _dictionary = _unitOfWork.DictionaryRepository.Get(word.IdDictionary);
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
                string ModelWordAdd = Resource.ModelWordAdd;
                string ModelWordChange = Resource.ModelWordChange;
                string ModelNoFillFull = Resource.ModelNoFillFull;
                string ModelForAddingWord = Resource.ModelForAddingWord;

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
                    await NavigationService.RemoveLastFromBackStackAsync();
                    await NavigationService.NavigateToAsync<WordsListViewModel>(_dictionary);
                    await NavigationService.RemoveLastFromBackStackAsync();
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

        private Words CreateWord()
        {
            try
            {
                Words word;
                if(!_isChangeWord)
                {
                    word = _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = _dictionary.Id, RusWord = NativeWord, EngWord = TranslateWord, Transcription = TranscriptionWord });
                }
                else
                {
                    SetDataToChangeWord();
                    word = _unitOfWork.WordsRepository.Update(_wordChange);
                }
                _unitOfWork.Save();
                return word;
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
