using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class RepeatingWordsViewModel : ViewModelBase
    {
        public RepeatingWordsViewModel(INavigationService navigationServcie, IDialogService dialogService, IUnitOfWork unitOfWork) : base(navigationServcie, dialogService)
        {
          _unitOfWork = unitOfWork;
          AllWordsCount = 0;
          AllShowedWordsCount = 0;
          AllOpenedWordsCount = 0;
            SwipeWordCommand = new Command<string>((direction)=> { SwipeWord(direction); });        
          VoiceActingCommand = new Command(VoiceActing);
          EnterTranslateCommand = new Command(EnterTranslate);
          SelectFromWordsCommand = new Command(SelectFromWords);
        }

        private void SwipeWord(string direction)
        {
           var enumDirection = (SwipeDirection)Enum.Parse(typeof(SwipeDirection), direction);
            switch (enumDirection)
            {
                case SwipeDirection.Down:
                case SwipeDirection.Up:
                    {
                        ShowTranslateWord();
                        break;
                    }
                case SwipeDirection.Left:
                    {
                        ShowNextWord();
                        break;
                    }
                case SwipeDirection.Right:
                    {
                        ShowPreviousWord();
                        break;
                    }
            }
        }

        private readonly IUnitOfWork _unitOfWork;
        private Dictionary _dictionary;
        private IEnumerable<Words> _wordsCollection;
        private bool _isFromNative;
        private Words _currentWord;
        const string NAME_DB_FOR_CONTINUE = "ContinueDictionary";
     
        private string _dictionaryName;
        public string DictionaryName { get => _dictionaryName; set { _dictionaryName = value; OnPropertyChanged(nameof(DictionaryName)); } }

        private string _currentShowingWord;
        public string CurrentShowingWord { get => _currentShowingWord; set { _currentShowingWord = value; OnPropertyChanged(nameof(CurrentShowingWord)); } }
        private string _currentTranscriptWord;
        public string CurrentTranscriptWord { get => _currentTranscriptWord; set { _currentTranscriptWord = value; OnPropertyChanged(nameof(CurrentTranscriptWord)); } }

        private int _allWordsCount;
        public int AllWordsCount { get => _allWordsCount; set { _allWordsCount = value; OnPropertyChanged(nameof(AllWordsCount)); } }
        private int _allShowedWordsCount;
        public int AllShowedWordsCount { get => _allShowedWordsCount; set { _allShowedWordsCount = value; OnPropertyChanged(nameof(AllShowedWordsCount)); } }
        private int _allOpenedWordsCount;
        public int AllOpenedWordsCount { get => _allOpenedWordsCount; set { _allOpenedWordsCount = value; OnPropertyChanged(nameof(AllOpenedWordsCount)); } }
        private bool _isTranscriptionShow;
        public bool IsTranscriptionShow { get => _isTranscriptionShow; set { _isTranscriptionShow = value; OnPropertyChanged(nameof(IsTranscriptionShow)); } }
        

        public ICommand SwipeWordCommand { get; set; }    
        public ICommand VoiceActingCommand { get; set; }
        public ICommand EnterTranslateCommand { get; set; }
        public ICommand SelectFromWordsCommand { get; set; }

        private int _indexWord = 0;
        private void ShowNextWord()
        {
            _countOpenedOneWord = 0;
            _currentWord = _wordsCollection.ElementAt(_indexWord);
            SetViewWords(_currentWord, _isFromNative);
            _indexWord++;
            AllShowedWordsCount++;
        }

        private void SetViewWords(Words word, bool isNative, bool isOpened = false)
        {
            if (isOpened)
                isNative = !isNative;
            if(isNative)
                CurrentShowingWord = word.RusWord;             
            else
                CurrentShowingWord = word.EngWord;         
            IsTranscriptionShow = !isNative;
            CurrentTranscriptWord = word.Transcription;
        }

       
        private void ShowPreviousWord()
        {
            _indexWord--;
            _currentWord = _wordsCollection.ElementAt(_indexWord);
            SetViewWords(_currentWord, _isFromNative);
            _indexWord++;
        }

        private bool _isOpened = false;
        private int _countOpenedOneWord;
        private void ShowTranslateWord()
        {          
            if(_countOpenedOneWord == 0)
                AllOpenedWordsCount++;
            _isOpened = !_isOpened;
            SetViewWords(_currentWord, _isFromNative, _isOpened);
            _countOpenedOneWord++;
        }

        private void VoiceActing()
        {
            Debugger.Break();
        }

        private void EnterTranslate()
        {
            Debugger.Break();
        }
        private void SelectFromWords()
        {
            Debugger.Break();
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
            _wordsCollection = tempWords.AsEnumerable();
        }

        private async Task<Dictionary> GetDictionary(int id) => await Task.Run(() => _unitOfWork.DictionaryRepository.Get(id));
        private async Task<bool> ShowFromLanguageNative() => !await DialogService.ShowConfirmAsync(message: Resource.ModalActFromTrtoF, title: "", buttonOk: Resource.Yes, buttonCancel: Resource.No);                      
        private async Task<IEnumerable<Words>> LoadWords(int id) => await Task.Run(() => _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == id).AsEnumerable());

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            if (navigationData is Dictionary dictionary)
            {
                _isFromNative = await ShowFromLanguageNative();
                _dictionary = dictionary;
                DictionaryName = _dictionary.Name;
            }
            else if (navigationData is LastAction lastAct)
            {
                _isFromNative = lastAct.FromRus;
                _dictionary = await GetDictionary(lastAct.IdDictionary);
                DictionaryName = _dictionary.Name;
            }
            else
                throw new Exception("Error load RepeatingWordsViewModel, bad params navigationData");
            _wordsCollection = await LoadWords(_dictionary.Id);
            AllWordsCount = _wordsCollection.Count();
            ShakeWordsCollection(_wordsCollection);
            ShowNextWord();
            await base.InitializeAsync(navigationData);
        }







        ////для определения языка
        //bool FromRus;
        ////получения ID словаря
        //int iDdictionary;
        ////для определения поворота карточки
        //bool Turn;
        ////для списка слов
        //IEnumerable<Words> words;
        ////номер слова в списке IEnumerable<words>
        //int Count = 0;
        //List<Words> TurnedWords = new List<Words>();
        //int countW = 1;
        ////язык озвучки
        //string lang;
        ////кол-во перевернутых(невыученных слов)
        //int countTurned = 0;
        //string TextTurned = Resource.LabelCountTurned;
        //const string NameDbForContinued = "ContinueDictionary";
        ////переменная для обработки страницы при выходе из нее
        //bool OnDisapearOverride = true;





        //public RepeatWordView(int iDdictionary, bool FromRus)
        //{
        //    InitializeComponent();
        //    this.FromRus = FromRus;
        //    //переменная для многократного переворота карточки
        //    Turn = FromRus;
        //    this.iDdictionary = iDdictionary;
        //    //получаем карточки и сортируем их рандомно
        //    Debugger.Break();
        //    //   words = App.Wr.GetWords(iDdictionary);
        //    RandomWordList();
        //    //кол во слов и пройденных слов
        //    LabelCountOfWords.Text = words.Count().ToString() + "/" + (countW + Count).ToString() + "/" + countTurned.ToString();
        //    //LabelCountOfWordsTurn.Text = TextTurned+" "+ countTurned.ToString();
        //    // код для Android
        //    if (Device.RuntimePlatform == Device.Android)
        //    {
        //        lang = "en_GB";
        //        DependencyService.Get<IAdmobInterstitial>().Show("ca-app-pub-5993977371632312/2459277342");
        //    }
        //    else
        //         if (Device.RuntimePlatform == Device.UWP)
        //    { lang = "en-GB"; }

        //    UpdateWord(Count, FromRus);
        //    ButtonVoice.BackgroundColor = new Color(0, 0, 0, 0);
        //    ButtonVoice.Image = "voice.png";
        //    picker.SelectedIndex = 0;//its english language default
        //}





        ////конструктор при продолжении изучения словаря
        //public RepeatWordView(LastAction la)
        //{
        //    InitializeComponent();
        //    FromRus = la.FromRus;
        //    //переменная для многократного переворота карточки
        //    Turn = FromRus;
        //    iDdictionary = la.IdDictionary;
        //    Debugger.Break();
        //    //  words = App.Wr.GetWords(la.IdDictionary);
        //    //определяем чему равен Count;
        //    Count = la.IdWord;
        //    //сколько слов всего и пройдено
        //    LabelCountOfWords.Text = words.Count().ToString() + "/" + (countW + Count).ToString() + "/" + countTurned.ToString();
        //    if (Device.RuntimePlatform == Device.Android)
        //    {
        //        lang = "en_GB";
        //    }
        //    else if (Device.RuntimePlatform == Device.UWP)
        //    { lang = "en-GB"; }
        //    UpdateWord(Count, FromRus);
        //    ButtonVoice.BackgroundColor = new Color(0, 0, 0, 0);
        //    ButtonVoice.Image = "voice.png";
        //    picker.SelectedIndex = 0;//its english language
        //}



        ////метод смешивания слов перед выводом
        //private void RandomWordList()
        //{
        //    List<Words> tempWords = (List<Words>)words;
        //    Random rng = new Random();
        //    int n = words.Count();
        //    //пока не первое слово
        //    while (n > 1)
        //    {
        //        n--;
        //        int k = rng.Next(n + 1);
        //        Words value = tempWords[k];
        //        tempWords[k] = tempWords[n];
        //        tempWords[n] = value;
        //    }
        //    words = null;
        //    words = (IEnumerable<Words>)tempWords;
        //}

        ////метод для обновления слова
        //private void UpdateWord(int count, bool lang)
        //{
        //    try
        //    {
        //        if (lang)
        //        {
        //            WordRepeat.TextColor = (Color)Application.Current.Resources["ColorWB"];
        //            WordRepeat.Text = GetWords(count).RusWord;
        //            //управление видимостью озвучки               
        //            //  ButtonVoice.IsEnabled = false;
        //            picker.IsVisible = true;
        //            //   ButtonVoice.Image = "voiceX.png";
        //            TranscriptionRepeat.IsVisible = false;
        //        }
        //        else
        //        {
        //            WordRepeat.TextColor = (Color)Application.Current.Resources["ColorBlGr"];
        //            Words w = GetWords(count);
        //            //если транскрипции нет 
        //            if (w.Transcription == "[]")
        //            {//выводим только перевод
        //                WordRepeat.Text = w.EngWord;
        //                TranscriptionRepeat.IsVisible = false;
        //            }
        //            else
        //            {//перевод и транскрипцию
        //                WordRepeat.Text = w.EngWord;
        //                TranscriptionRepeat.Text = w.Transcription;
        //                TranscriptionRepeat.IsVisible = true;
        //            }
        //        }
        //    }
        //    catch (Exception er)
        //    {
        //        Log.Logger.Error(er);
        //    }
        //}




        ////для получения слова
        //Words GetWords(int index)
        //{
        //    if (index > words.Count())
        //        index = 0;
        //    Words item = words.ElementAt(index);
        //    return item;
        //}





        //private async void NextWordButtonClick(object sender, EventArgs e)
        //{//для сброса количества перево 
        //    try
        //    {
        //        Turn = FromRus;

        //        int z = words.Count() - 1;
        //        if (z > Count)
        //        {
        //            Count++;
        //            UpdateWord(Count, FromRus);
        //        }
        //        else
        //        {
        //            string ModalAllWordsComplete = Resource.ModalAllWordsComplete;
        //            string ModalFinish = Resource.ModalFinish;
        //            string ModalRestart = Resource.ModalRestart;
        //            string ModalLerningTurnedWords = Resource.ModalLerningTurnedWords;


        //            var action = await DisplayActionSheet(ModalAllWordsComplete, "", "", ModalFinish, ModalRestart, ModalLerningTurnedWords);

        //            if (action == ModalFinish)
        //            {
        //                //вызовем метод для проверки есть ли в списке перевернутые слова
        //                //если есть то создадим БД и добавим в нее слова
        //                await CreateTurnedDB();
        //                OnDisapearOverride = false;
        //                //перреход на главную страницу
        //                await Navigation.PopToRootAsync();
        //            }
        //            //если начать заново словарь 
        //            if (action == ModalRestart)
        //            {
        //                //то список перевернутых слов обновляем
        //                TurnedWords.Clear();
        //                //и количество перевернутых слов обнуляем
        //                countTurned = 0;
        //                //индекс слова обновляем
        //                Count = 0;
        //                //метод смешивания слов
        //                RandomWordList();
        //                //вызываем метод обновления слова
        //                UpdateWord(Count, FromRus);
        //            }
        //            //если выбрали повторение невыученных в этом словаре     
        //            if (action == ModalLerningTurnedWords)
        //            {
        //                //вызовем метод для проверки есть ли в списке перевернутые слова
        //                //если есть то создадим БД и добавим в нее слова
        //                await CreateTurnedDB();
        //                //обнулим индекс слов
        //                Count = 0;
        //                //обнулим кол-во перевернутых слов
        //                countTurned = 0;
        //                LabelCountOfWords.Text = words.Count().ToString() + "/" + (countW + Count).ToString() + "/" + countTurned.ToString();
        //                //LabelCountOfWordsTurn.Text = TextTurned + " " + countTurned.ToString();
        //                //получим последнюю БД и очистим список перевернутых слов
        //                TurnedWords.Clear();
        //                Debugger.Break();
        //                //   Dictionary di = App.Db.GetDictionarys().LastOrDefault();
        //                //   iDdictionary = di.Id;//получим новый список слов из БД
        //                //   words = App.Wr.GetWords(di.Id);//обновим экран
        //                UpdateWord(Count, FromRus);
        //            }

        //        }
        //        //сколько слов всего и пройдено сколько
        //        LabelCountOfWords.Text = words.Count().ToString() + "/" + (countW + Count).ToString() + "/" + countTurned.ToString();
        //    }
        //    catch (Exception er)
        //    {
        //        await DisplayAlert(Resource.ModalException, er.Message.ToString(), "Ok");
        //    }
        //}


        //string postFixNotLearningWords = Resource.NotLearningPostfics;

        ////===============================метод создания словаря перевернутых слов(невыученных)=================
        //private async Task CreateTurnedDB()
        //{
        //    try
        //    {
        //        if (TurnedWords.Any())
        //        {
        //            Debugger.Break();
        //            string NameD = string.Empty;
        //            //string NameD = App.Db.GetDictionary(iDdictionary).Name;
        //            string NameDictionary = string.Empty;
        //            //проверим это словарь который уже изучали или нет(содержит приставку lerning)
        //            if (NameD.Contains(postFixNotLearningWords))
        //            {
        //                NameDictionary = NameD;
        //            }
        //            else
        //            {
        //                Debugger.Break();
        //                //  NameDictionary = App.Db.GetDictionary(iDdictionary).Name + postFixNotLearningWords;
        //            }
        //            Debugger.Break();
        //            //  Dictionary deldict = App.Db.GetDictionarys().Where(x => x.Name == NameDictionary).FirstOrDefault();
        //            //if (deldict != null)
        //            //{
        //            //    App.Wr.DeleteWords(deldict.Id);
        //            //    App.Db.DeleteDictionary(deldict.Id);

        //            //}

        //            Dictionary dict = new Dictionary()
        //            {
        //                Id = 0,
        //                Name = NameDictionary
        //            };
        //            Debugger.Break();
        //            //App.Db.CreateDictionary(dict);
        //            //  int IdLastDictionary = App.Db.GetDictionarys().LastOrDefault().Id;
        //            foreach (var i in TurnedWords)
        //            {
        //                i.Id = 0;
        //                Debugger.Break();
        //                //i.IdDictionary = IdLastDictionary;
        //                //App.Wr.CreateWord(i);
        //            }
        //        }
        //    }
        //    catch (Exception er)
        //    {
        //        await DisplayAlert(Resource.ModalException, er.Message.ToString(), "Ok");
        //    }
        //}







        //private void SaveLastWorld()
        //{
        //    Task.Run(() =>
        //    {
        //        try
        //        {
        //            //проверим наличие словаря для продолжения изучения слов
        //            Debugger.Break();
        //            //if (!App.Db.GetDictionarys().Where(x => x.Name == NameDbForContinued).Any())
        //            //{//если нету то создадим
        //            //    App.Db.CreateDictionary(new Dictionary() { Id = 0, Name = NameDbForContinued });
        //            //}
        //            //обновим слова(или добавим)
        //            Debugger.Break();
        //            //int IdContinueDict = App.Db.GetDictionarys().Where(x => x.Name == NameDbForContinued).FirstOrDefault().Id;
        //            //App.Wr.DeleteWords(IdContinueDict);
        //            foreach (Words w in words)
        //            {
        //                w.Id = 0;
        //                Debugger.Break();
        //                //w.IdDictionary = IdContinueDict;
        //                //App.Wr.CreateWord(w);
        //            }
        //            //при нажатии  на кн выхода сохраняется последнее слово
        //            Debugger.Break();
        //            //LastAction i = new LastAction()
        //            //{
        //            //    Id = 0,
        //            //    IdDictionary = IdContinueDict,
        //            //    FromRus = FromRus,
        //            //    IdWord = Count
        //            //};
        //            Debugger.Break();
        //            // App.LAr.SaveLastAction(i);

        //        }
        //        catch (Exception er)
        //        {
        //            Log.Logger.Error(er);
        //        }

        //    });
        //}



        //int idTurn = -1;
        //private void TurnAroundWordButtonClick(object sender, EventArgs e)
        //{
        //    if (Count != idTurn)
        //    {
        //        countTurned++;
        //        idTurn = Count;
        //        //добавим перевернутое слово в список для дальнейшего добавления в БД
        //        TurnedWords.Add(GetWords(Count));
        //    }
        //    UpdateWord(Count, !Turn);
        //    Turn = !Turn;//перевернули карточку         
        //}


        //private void BtnClickSpeech(object sender, EventArgs e)
        //{
        //    DependencyService.Get<ITextToSpeech>().Speak(GetWords(Count).EngWord, lang);
        //}





        //#region ChoseLanguage
        //private void picker_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    switch (picker.Items[picker.SelectedIndex].ToString())
        //    {
        //        case "English":
        //            {

        //                if (Device.RuntimePlatform == Device.Android)
        //                {
        //                    lang = "en_GB";
        //                }
        //                else if (Device.RuntimePlatform == Device.UWP)
        //                {
        //                    lang = "en-GB";
        //                }

        //                break;
        //            }
        //        case "French":
        //            {
        //                if (Device.RuntimePlatform == Device.Android)
        //                {
        //                    lang = "fr_FR";
        //                }
        //                else if (Device.RuntimePlatform == Device.UWP)
        //                {
        //                    lang = "fr-FR";
        //                }
        //                break;
        //            }
        //        case "German":
        //            {
        //                if (Device.RuntimePlatform == Device.Android)
        //                {
        //                    lang = "de_DE";
        //                }
        //                else if (Device.RuntimePlatform == Device.UWP)
        //                {
        //                    lang = "de-DE";
        //                }

        //                break;
        //            }
        //        case "Polish":
        //            {
        //                if (Device.RuntimePlatform == Device.Android)
        //                {
        //                    lang = "pl_PL";
        //                }
        //                else if (Device.RuntimePlatform == Device.UWP)
        //                {
        //                    lang = "pl-PL";
        //                }

        //                break;
        //            }
        //        case "Ukrainian":
        //            {
        //                if (Device.RuntimePlatform == Device.Android)
        //                {
        //                    lang = "uk_UK";
        //                }
        //                else if (Device.RuntimePlatform == Device.UWP)
        //                {
        //                    lang = "uk-UK";
        //                }

        //                break;
        //            }
        //        case "Italian":
        //            {
        //                if (Device.RuntimePlatform == Device.Android)
        //                {
        //                    lang = "it_IT";
        //                }
        //                else if (Device.RuntimePlatform == Device.UWP)
        //                {
        //                    lang = "it-IT";
        //                }

        //                break;
        //            }
        //        case "Русский":
        //            {
        //                if (Device.RuntimePlatform == Device.Android)
        //                {
        //                    lang = "ru_RU";
        //                }
        //                else if (Device.RuntimePlatform == Device.UWP)
        //                {
        //                    lang = "ru-RU";
        //                }

        //                break;
        //            }
        //        case "Chinese":
        //            {
        //                if (Device.RuntimePlatform == Device.Android)
        //                {
        //                    lang = "zh_CN";
        //                }
        //                else if (Device.RuntimePlatform == Device.UWP)
        //                {
        //                    lang = "zh-CN";
        //                }

        //                break;
        //            }
        //        case "Japanese":
        //            {
        //                if (Device.RuntimePlatform == Device.Android)
        //                {
        //                    lang = "ja_JP";
        //                }
        //                else if (Device.RuntimePlatform == Device.UWP)
        //                {
        //                    lang = "ja-JP";
        //                }

        //                break;
        //            }
        //        case "Portuguese(Brazil)":
        //            {
        //                if (Device.RuntimePlatform == Device.Android)
        //                {
        //                    lang = "pt_BR";
        //                }
        //                else if (Device.RuntimePlatform == Device.UWP)
        //                {
        //                    lang = "pt-BR";
        //                }

        //                break;
        //            }
        //        case "Spanish":
        //            {
        //                if (Device.RuntimePlatform == Device.Android)
        //                {
        //                    lang = "es_ES";
        //                }
        //                else if (Device.RuntimePlatform == Device.UWP)
        //                {
        //                    lang = "es-ES";
        //                }

        //                break;
        //            }
        //        default: break;
        //    }
        //    #endregion





    }
    }
