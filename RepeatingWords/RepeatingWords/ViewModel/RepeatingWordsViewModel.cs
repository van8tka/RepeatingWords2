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
        public RepeatingWordsViewModel(INavigationService navigationServcie, IDialogService dialogService, IUnitOfWork unitOfWork, IVolumeLanguageService volumeService, IContinueWordsService continueWordsManager, IAnimationService animationService) : base(navigationServcie, dialogService)
        {
            _animationService = animationService ?? throw new ArgumentNullException(nameof(animationService));
            _unitOfWork = unitOfWork;
            _volumeService = volumeService ?? throw new ArgumentNullException(nameof(volumeService));
            _continueWordsManager = continueWordsManager ?? throw new ArgumentNullException(nameof(continueWordsManager));
            IsVisibleScore = false;
            Model = new RepeatingWordsModel();
            VoiceActingCommand = new Command(VoiceActing);
            EnterTranslateCommand = new Command(async ()=>
            {
                await _animationService.AnimationFade(WorkContainerView, 0);
                await ShowEnterTranslate();
                await _animationService.AnimationFade(WorkContainerView, 1);
            });
            SelectFromWordsCommand = new Command(async () =>
            {
                await _animationService.AnimationFade(WorkContainerView, 0);
                await ShowSelectFromWords();
                await _animationService.AnimationFade(WorkContainerView, 1);
            });
            LearningCardsCommand = new Command(async () =>
            {
                await _animationService.AnimationFade(WorkContainerView, 0); 
                await ShowLearningCards();
                await _animationService.AnimationFade(WorkContainerView, 1);
            });
            UnloadPageCommand = new Command(UnloadPage);
        }

        private readonly IAnimationService _animationService;
        private ICustomContentViewModel _workSpaceVM;
        private readonly IVolumeLanguageService _volumeService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IContinueWordsService _continueWordsManager;

        private Dictionary _dictionary;
        private string _currentVolumeLanguage;
        private string _dictionaryName;
        public string DictionaryName
        {
            get => _dictionaryName; set
            {
                _dictionaryName = value;
                if (_dictionaryName.EndsWith(Constants.NAME_DB_FOR_CONTINUE))
                    _dictionaryName = _dictionaryName.Replace(Constants.NAME_DB_FOR_CONTINUE, "");
                if (_dictionaryName.EndsWith(Resource.NotLearningPostfics))
                    _dictionaryName = _dictionaryName.Replace(Resource.NotLearningPostfics, "");
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

        private bool _isVisibleScore;
        public bool IsVisibleScore
        {
            get => _isVisibleScore;
            set { _isVisibleScore = value;OnPropertyChanged(nameof(IsVisibleScore)); }
        }


        private RepeatingWordsModel _model;
        public RepeatingWordsModel Model { get => _model; set { _model = value; OnPropertyChanged(nameof(Model)); } }

        private Color _cardsButtonBackground;
        public Color CardsButtonBackground { get => _cardsButtonBackground; set { _cardsButtonBackground = value; OnPropertyChanged(nameof(CardsButtonBackground)); } }
        private Color _selectButtonBackground;
        public Color SelectButtonBackground { get => _selectButtonBackground; set { _selectButtonBackground = value; OnPropertyChanged(nameof(SelectButtonBackground)); } }
        private Color _entryButtonBackground;
        public Color EntryButtonBackground { get => _entryButtonBackground; set { _entryButtonBackground = value; OnPropertyChanged(nameof(EntryButtonBackground)); } }

        public ICommand VoiceActingCommand { get; set; }
        public ICommand EnterTranslateCommand { get; set; }
        public ICommand SelectFromWordsCommand { get; set; }
        public ICommand LearningCardsCommand { get; set; }
        public ICommand UnloadPageCommand { get; set; }

        private WorkSpaceEnterWordView _enterWordView;
        private WorkSpaceEnterWordView EnterWordView => _enterWordView ?? (_enterWordView = new WorkSpaceEnterWordView());
        private WorkSpaceSelectWordView _selectWordView;
        private WorkSpaceSelectWordView SelectWordView => _selectWordView ?? (_selectWordView = new WorkSpaceSelectWordView());
        private WorkSpaceCardsView _cardsView;
        private WorkSpaceCardsView CardsView => _cardsView ?? (_cardsView = new WorkSpaceCardsView());
       
        private void VoiceActing()
        {
            DependencyService.Get<ITextToSpeech>().Speak(Model.CurrentWord.EngWord, _currentVolumeLanguage);
        }
       
        private async Task ShowEnterTranslate()
        {
            await SetViewWorkSpaceEnterWord();
            SetBackgroundButton(nameof(EntryButtonBackground));
        }
        private async Task SetViewWorkSpaceEnterWord()
        {
             WorkSpaceView = EnterWordView;
            _workSpaceVM = EnterWordView.CustomVM;
            _workSpaceVM.Model = Model; 
            await (_workSpaceVM as WorkSpaceEnterWordViewModel)?.ShowNextWord(isFirstShowAfterLoad: true);
        }

        private async Task ShowSelectFromWords()
        {
            await SetViewWorkSpaceSelectWord();
            SetBackgroundButton(nameof(SelectButtonBackground));
        }
        private async Task SetViewWorkSpaceSelectWord()
        {
             WorkSpaceView = SelectWordView;
            _workSpaceVM = SelectWordView.CustomVM;
            _workSpaceVM.Model = Model;
            await (_workSpaceVM as WorkSpaceSelectWordViewModel)?.ShowNextWord(isFirstShowAfterLoad: true);
        }

        private async Task ShowLearningCards()
        { 
            await SetViewWorkSpaceLearningCards();
            SetBackgroundButton(nameof(CardsButtonBackground));
        }
        private async Task SetViewWorkSpaceLearningCards()
        {
             WorkSpaceView = CardsView;
            _workSpaceVM = CardsView.CustomVM;
            _workSpaceVM.Model = Model; 
           await (_workSpaceVM as WorkSpaceCardsViewModel)?.ShowNextWord(isFirstShowAfterLoad: true);
        }

     

        private void SetBackgroundButton(string button)
        {
            switch (button)
            {
                case nameof(CardsButtonBackground):
                {
                    CardsButtonBackground = Color.LightGray;
                    SelectButtonBackground = Color.Transparent;
                    EntryButtonBackground = Color.Transparent; 
                    break;
                }
                case nameof(SelectButtonBackground):
                {
                    CardsButtonBackground = Color.Transparent;
                    SelectButtonBackground = Color.LightGray;
                    EntryButtonBackground = Color.Transparent;
                    break;
                }
                case nameof(EntryButtonBackground):
                {
                    CardsButtonBackground = Color.Transparent;
                    SelectButtonBackground = Color.Transparent;
                    EntryButtonBackground = Color.LightGray;
                    break;
                }
            }
        }

      


        private void ShakeWordsCollection(IEnumerable<Words> words)
        {
            var tempWords = new List<Words>(words);
            var random = new Random();
            int count = words.Count();
            //пока не первое слово
            while (count > 1)
            {
                count--;
                int i = random.Next(count + 1);
                Words value = tempWords[i];
                tempWords[i] = tempWords[count];
                tempWords[count] = value;
            }
            Model.WordsLearningAll = tempWords;
            Model.WordsLeft = new List<Words>(Model.WordsLearningAll);
        }

        private async Task<Dictionary> GetDictionary(int id) => await Task.Run(() => _unitOfWork.DictionaryRepository.Get(id));
        private async Task<bool> ShowFromLanguageNative() => !await DialogService.ShowConfirmAsync(message: Resource.ModalActFromTrtoF, title: "", buttonOk: Resource.Yes, buttonCancel: Resource.No);
        private async Task<IEnumerable<Words>> LoadWords(int id) => await Task.Run(() => _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == id).AsEnumerable());

        public override async Task InitializeAsync(object navigationData)
        {
            SetBackgroundButton(nameof(CardsButtonBackground));         
            IsBusy = true;
            if (navigationData is Dictionary dictionary)
            {
                Model.IsFromNative = await ShowFromLanguageNative();
                _dictionary = dictionary;
                DictionaryName = _dictionary.Name;
            }
            else if (navigationData is LastAction lastAct)
            {
                Model.IsFromNative = lastAct.FromRus;
                _dictionary = await GetDictionary(lastAct.IdDictionary);
                DictionaryName = _dictionary.Name;
            }
            else
                throw new Exception("Error load RepeatingWordsViewModel, bad params navigationData");
            _currentVolumeLanguage = _volumeService.GetSysAbbreviationVolumeLanguage();
            Model.WordsLearningAll = (await LoadWords(_dictionary.Id)).ToList();
            _continueWordsManager.RemoveContinueDictionary(Model.WordsLearningAll);
            Model.AllWordsCount = Model.WordsLearningAll.Count();
            Model.Dictionary = _dictionary;
            ShakeWordsCollection(Model.WordsLearningAll);
            SetViewWorkSpaceLearningCards();
            IsVisibleScore = true;
            await base.InitializeAsync(navigationData);
        }


        /// <summary>
        /// сохранение невыученных слов и сохранение слов для продолжения 
        /// </summary>
        public void UnloadPage()
        {
            try
            {
                if (Model.AllWordsCount == Model.AllShowedWordsCount)
                    _continueWordsManager.RemoveContinueDictionary(Model.WordsLearningAll);
                else
                    _continueWordsManager.SaveContinueDictionary(_dictionary.Name, Model.WordsLeft, Model.IsFromNative);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }
    }
}
