using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Model;
using RepeatingWords.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using RepeatingWords.Services;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class RepeatingWordsViewModel : ViewModelBase
    {
        public RepeatingWordsViewModel(INavigationService navigationServcie, IDialogService dialogService,
            IStudyService studyService, IAnimationService animationService, ITextToSpeech speechService,
            IShowLanguage showLanguageService) : base(navigationServcie, dialogService)
        {
            _animationService = animationService;
            _studyService = studyService;
            _showLanguageService = showLanguageService;
            _speechService = speechService;
            Model = new RepeatingWordsModel();
            VoiceActingCommand = new Command(async () => await _speechService.Speak(Model.CurrentWord.EngWord));
            EditCurrentWordCommand = new Command(async () =>
            {
                _isEditing = true;
                if (Model.CurrentWord != null)
                    await NavigationService.NavigateToAsync<CreateWordViewModel>(Model.CurrentWord);
            });
            EnterTranslateCommand = new Command(async () => await ShowEnterTranslate());
            SelectFromWordsCommand = new Command(async () => await ShowSelectFromWords());
            LearningCardsCommand = new Command(async () => await ShowLearningCards());
            AppearingCommand = new Command(async () => await AppearingPage());
            DisappearingCommand = new Command( Disappearing);
            _resetLearnedTask = null;
        }

        #region FIELDS

        //переменная для определения переключения в режим редактирования текущего слова
        private bool _isEditing;
        private readonly IAnimationService _animationService;
        private ICustomContentViewModel _workSpaceVM;
        private readonly IStudyService _studyService;
        private readonly ITextToSpeech _speechService;
        private readonly IShowLanguage _showLanguageService;
        private DictionaryModel _dictionary;

        private readonly string cardsActive = "icons_cardsbutton.png";
        private readonly string cardsUnActive = "icons_cardsbuttonGray.png";
        private readonly string selectActive = "icons_select_word.png";
        private readonly string selectUnActive = "icons_select_wordGray.png";
        private readonly string entryActive = "icons_keyboard.png";
        private readonly string entryUnActive = "icons_keyboardGray.png";

        #endregion

        #region PROPERTIES

        private string _dictionaryName;

        public string DictionaryName
        {
            get => _dictionaryName;
            set
            {
                _dictionaryName = value;
                OnPropertyChanged(nameof(DictionaryName));
            }
        }

        public Xamarin.Forms.View WorkContainerView { get; set; }

        private ContentView _workSpaceView;

        public ContentView WorkSpaceView
        {
            get => _workSpaceView;
            set
            {
                _workSpaceView = value;
                OnPropertyChanged(nameof(WorkSpaceView));
            }
        }

        private RepeatingWordsModel _model;

        public RepeatingWordsModel Model
        {
            get => _model;
            set
            {
                _model = value;
                OnPropertyChanged(nameof(Model));
            }
        }

        private string _cardsButtonBackground;

        public string CardsImage
        {
            get => _cardsButtonBackground;
            set
            {
                _cardsButtonBackground = value;
                OnPropertyChanged(nameof(CardsImage));
            }
        }

        private string _selectButtonBackground;

        public string SelectImage
        {
            get => _selectButtonBackground;
            set
            {
                _selectButtonBackground = value;
                OnPropertyChanged(nameof(SelectImage));
            }
        }

        private string _entryButtonBackground;

        public string EntryImage
        {
            get => _entryButtonBackground;
            set
            {
                _entryButtonBackground = value;
                OnPropertyChanged(nameof(EntryImage));
            }
        }

        private string _speackerLang;

        public string SpeackerLang
        {
            get => _speackerLang;
            set
            {
                _speackerLang = value;
                OnPropertyChanged(nameof(SpeackerLang));
            }
        }

        #endregion

        public ICommand VoiceActingCommand { get; set; }
        public ICommand EnterTranslateCommand { get; set; }
        public ICommand SelectFromWordsCommand { get; set; }
        public ICommand LearningCardsCommand { get; set; }
        public ICommand EditCurrentWordCommand { get; set; }
        public ICommand AppearingCommand { get; set; }
        public ICommand DisappearingCommand { get; set; }
        private WorkSpaceEnterWordView _enterWordView;

        private WorkSpaceEnterWordView EnterWordView =>
            _enterWordView ?? (_enterWordView = new WorkSpaceEnterWordView());

        private WorkSpaceSelectWordView _selectWordView;

        private WorkSpaceSelectWordView SelectWordView =>
            _selectWordView ?? (_selectWordView = new WorkSpaceSelectWordView());

        private WorkSpaceCardsView _cardsView;
        private WorkSpaceCardsView CardsView => _cardsView ?? (_cardsView = new WorkSpaceCardsView());

        private async Task WorkSurface(string nameSurface, ICustomContentView viewSurface)
        {
            if (Model.AllWordsCount == 0)
                return;
            await _animationService.AnimationFade(WorkContainerView, 0);
            WorkSpaceView = viewSurface as ContentView ?? throw new Exception("Error SurfaceView is null");
            _workSpaceVM = viewSurface.CustomVM;
            _workSpaceVM.Model = Model;
            await _workSpaceVM.ShowNextWord(isFirstShowAfterLoad: true);
            SetBackgroundButton(nameSurface);
            await _animationService.AnimationFade(WorkContainerView, 1);
        }

        private async Task ShowEnterTranslate() => await WorkSurface(nameof(EntryImage), EnterWordView);
        private async Task ShowSelectFromWords() => await WorkSurface(nameof(SelectImage), SelectWordView);
        private async Task ShowLearningCards() => await WorkSurface(nameof(CardsImage), CardsView);

        private void SetBackgroundButton(string button)
        {
            switch (button)
            {
                case nameof(CardsImage):
                {
                    CardsImage = cardsActive;
                    SelectImage = selectUnActive;
                    EntryImage = entryUnActive;
                    break;
                }
                case nameof(SelectImage):
                {
                    CardsImage = cardsUnActive;
                    SelectImage = selectActive;
                    EntryImage = entryUnActive;
                    break;
                }
                case nameof(EntryImage):
                {
                    CardsImage = cardsUnActive;
                    SelectImage = selectUnActive;
                    EntryImage = entryActive;
                    break;
                }
            }
        }

        private void ShakeWordsCollection(IEnumerable<WordsModel> words)
        {
            var tempWords = new List<WordsModel>(words);
            var random = new Random();
            int count = words.Count();
            //пока не первое слово
            while (count > 1)
            {
                count--;
                int i = random.Next(count + 1);
                WordsModel value = tempWords[i];
                tempWords[i] = tempWords[count];
                tempWords[count] = value;
            }
            Model.WordsLearningAll = tempWords;
            Model.WordsLeft = new List<WordsModel>(Model.WordsLearningAll);
        }

        public override async Task InitializeAsync(object navigationData)
        {
            try
            {
                _studyService.BeginTransaction();
                IsBusy = true;
                SetBackgroundButton(nameof(CardsImage));
                _dictionary = navigationData as DictionaryModel;
                Model.Dictionary = _dictionary;
                DictionaryName = _dictionary.Name;
                var wordsList = GetWordsCollection(_dictionary);
                ShakeWordsCollection(wordsList);
                Model.IsFromNative = _showLanguageService.GetFirstLanguage();
                Model.WordsLearningAll = wordsList.ToList();
                await ShowLearningCards();
                SpeackerLang = _speechService.Language;
                await base.InitializeAsync(navigationData);
            }
            catch (Exception e)
            {
                _studyService.RollBackTransaction();
                DialogService.ShowToast("Error loading words to study" + e.Message);
            }
        }

        private IEnumerable<WordsModel> GetWordsCollection(DictionaryModel dictionary) {
            if (_dictionary.IsStudyUnlearnedWords)
            {
                Model.AllWordsCount = _dictionary.CountUnlearned;
                return _dictionary.WordsUnlearnedCollection;
            }
            Model.AllWordsCount = _dictionary.CountWords;
            ResetLearnedWords(_dictionary.WordsLearnedCollection.ToList());
            return _dictionary.WordsCollection;
        }

        private Task _resetLearnedTask;
        /// <summary>
        /// reset words from islearned=true to false
        /// </summary>
        /// <param name="learnedCollection"></param>
        private void ResetLearnedWords(IList<WordsModel> learnedCollection)
        {
            var items = learnedCollection.Where(x => x.IsLearned);
            Parallel.ForEach(items, (w) => { w.IsLearned = false; });
            _dictionary.PercentOfLearned = "0";
           _resetLearnedTask = Task.Run(() =>
            {
                _studyService.ResetStudiedWords(_dictionary.Id);
            });
        }

        private async Task AppearingPage()
        {
            if(_disappiaring)
                _studyService.BeginTransaction();
            if (_isEditing)
                await _workSpaceVM.SetViewWords(Model.CurrentWord, Model.IsFromNative);
            _isEditing = false;
            _disappiaring = false;
        }

        private bool _disappiaring;
        private const int PERSENT = 100;
        private void Disappearing()
        {
            _disappiaring = true;
            if (_resetLearnedTask!=null)
                _resetLearnedTask.Wait();
            _dictionary.LastUpdated = DateTime.UtcNow;
            float proportion = (float)_dictionary.CountLearned / (float) _dictionary.CountWords;
            _dictionary.PercentOfLearned = ((int)(proportion * PERSENT)).ToString();
            _studyService.UpdateDictionary(_dictionary);
            _studyService.CommitTransaction();
        }
    }
}
