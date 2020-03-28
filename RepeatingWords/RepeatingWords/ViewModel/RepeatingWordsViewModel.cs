﻿using RepeatingWords.DataService.Interfaces;
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
        public RepeatingWordsViewModel(INavigationService navigationServcie, IDialogService dialogService, IDictionaryStudyService studyService, IAnimationService animationService, ITextToSpeech speechService, IShowLanguage showLanguageService) : base(navigationServcie, dialogService)
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
            EnterTranslateCommand = new Command(async () =>await ShowEnterTranslate());
            SelectFromWordsCommand = new Command(async () =>await ShowSelectFromWords());
            LearningCardsCommand = new Command(async () =>await ShowLearningCards());
            AppearingCommand = new Command(async () => await AppearingPage());
            DisappearingCommand = new Command(async()=>await Disappearing());
        }
        #region FIELDS
        //переменная для определения переключения в режим редактирования текущего слова
        private bool _isEditing;
        private readonly IAnimationService _animationService;
        private ICustomContentViewModel _workSpaceVM;
        private readonly IDictionaryStudyService _studyService;
        private readonly ITextToSpeech _speechService;
        private readonly IShowLanguage _showLanguageService;
        private Dictionary _dictionary;
      
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
        
        private RepeatingWordsModel _model;
        public RepeatingWordsModel Model { get => _model; set { _model = value; OnPropertyChanged(nameof(Model)); } }
        private string _cardsButtonBackground;
        public string CardsImage { get => _cardsButtonBackground; set { _cardsButtonBackground = value; OnPropertyChanged(nameof(CardsImage)); } }
        private string _selectButtonBackground;
        public string SelectImage { get => _selectButtonBackground; set { _selectButtonBackground = value; OnPropertyChanged(nameof(SelectImage)); } }
        private string _entryButtonBackground;
        public string EntryImage { get => _entryButtonBackground; set { _entryButtonBackground = value; OnPropertyChanged(nameof(EntryImage)); } }
        private string _speackerLang;
        public string SpeackerLang { get => _speackerLang; set { _speackerLang = value; OnPropertyChanged(nameof(SpeackerLang)); } }
        #endregion

        public ICommand VoiceActingCommand { get; set; }
        public ICommand EnterTranslateCommand { get; set; }
        public ICommand SelectFromWordsCommand { get; set; }
        public ICommand LearningCardsCommand { get; set; }
        public ICommand EditCurrentWordCommand { get; set; }
        public ICommand AppearingCommand { get; set; }
        public ICommand DisappearingCommand { get; set; }
        private WorkSpaceEnterWordView _enterWordView;
        private WorkSpaceEnterWordView EnterWordView => _enterWordView ?? (_enterWordView = new WorkSpaceEnterWordView());
        private WorkSpaceSelectWordView _selectWordView;
        private WorkSpaceSelectWordView SelectWordView => _selectWordView ?? (_selectWordView = new WorkSpaceSelectWordView());
        private WorkSpaceCardsView _cardsView;
        private WorkSpaceCardsView CardsView => _cardsView ?? (_cardsView = new WorkSpaceCardsView());


        private async Task WorkSurface(string nameSurface, ICustomContentView viewSurface)
        {
            if(Model.AllWordsCount==0)
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

        public override async Task InitializeAsync(object navigationData)
        {
            try
            {
                IsBusy = true;
                SetBackgroundButton(nameof(CardsImage));
                List<Words> wordsList = new List<Words>();
                int count = 0;
                _dictionary = navigationData as Dictionary;
                Model.Dictionary = _dictionary;
                DictionaryName = _dictionary.Name;
                wordsList = _studyService.GetWordsByDictionary(_dictionary.Id).ToList();
                count = wordsList.Count();
                ShakeWordsCollection(wordsList);
                Model.IsFromNative = _showLanguageService.GetFirstLanguage();
                Model.WordsLearningAll = wordsList;
                Model.AllWordsCount = count;
                await ShowLearningCards();
                SpeackerLang = _speechService.Language;
                await base.InitializeAsync(navigationData);
            }
            catch (Exception e)
            {
                DialogService.ShowToast("Error loading words to study");
            }
        }

       

        private async Task AppearingPage()
        {
            if (_isEditing)
            {
               await (_workSpaceVM as WorkSpaceBaseViewModel).SetViewWords(Model.CurrentWord, Model.IsFromNative);
            }
            _isEditing = false;
        }
        private const int PERSENT = 100;
        private async Task<bool> Disappearing()
        {
            try
            {
                _dictionary.LastUpdated = DateTime.UtcNow;
                float proportion = (float)Model.AllLearnedWordsCount / (float)Model.AllWordsCount;
                _dictionary.PercentOfLearned = (int)(proportion * PERSENT);
                _studyService.UpdateDictionary(_dictionary);
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
               Log.Logger.Error(e);
               return await Task.FromResult(false);
            }
        }
    }
}
