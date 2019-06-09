using System;
using RepeatingWords.Model;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace RepeatingWords
{ 
    public partial class App : Application
    {
        //переменная для определения стиля темы приложения
        private bool originalStyle = true;


        //исходные данные для инициализации БД
        Dictionary dictInit = new Dictionary()
        {
            Id = 0,
            Name = "ExampleDictionary"
        };
        List<Words> lw = new List<Words>()
        {
           new Words() {Id=0,IdDictionary=1,RusWord="словарь", EngWord="dictionary", Transcription= "[ˈdɪkʃəneri]" },
           new Words() { Id = 0, IdDictionary = 1, RusWord = "книга", EngWord = "book", Transcription = "[bʊk]" },
           new Words() { Id = 0, IdDictionary = 1, RusWord = "стол", EngWord = "table", Transcription = "[teɪb(ə)l]" },
         };

        public const string DATABASE_NAME = "repeatwords.db";
        public static DictionaryRepository db;

        public static DictionaryRepository Db
        {
            get
            {
                if (db == null)
                {
                    db = new DictionaryRepository(DATABASE_NAME);
                }
                return db;
            }
        }
        public static WordRepositiry Wr { get; set; }
        public static LastActionRepository LAr { get; set; } 
        public static WordRepositiry WrAsync { get; set; }

        public App()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.UWP)
                InitDb();
            CleanStackAndGoRootPage();
            SetOriginalStyle();
            SetChooseTranscriptionKeyboard();

            try
            {
                Wr = new WordRepositiry(Db.DBConnection);
                LAr = new LastActionRepository(Db.DBConnection);       
                WrAsync = new WordRepositiry(Db.DBConnectionAsync);
            }
            catch(Exception er)
            {
                Debug.WriteLine(er);
                throw;
            }
        }


        public  void CleanStackAndGoRootPage()
        {
          MainPage = new NavigationPage(new MainPage());
        } 



        //method for set default chosse keyboard when add word
        private void SetChooseTranscriptionKeyboard()
        {
            try
            {
                const string TrKeyboard = "TrKeyboard";
                const string showKeyboard = "true";

                object propTrKeyb;
                if (!App.Current.Properties.TryGetValue(TrKeyboard, out propTrKeyb))
                {
                    App.Current.Properties.Add(TrKeyboard, showKeyboard);
                }
            }
            catch(Exception er)
            {
                ErrorHandlerCustom.getErrorMessage(er);
            }
        }

        const string Them = "theme";
        const string _whiteThem = "white";
        const string _blackThem = "black";

        //method for set default theme
        private void SetOriginalStyle()
        {
            try
            {
                object propThem;


                if (App.Current.Properties.TryGetValue(Them, out propThem))
                {
                    // выполняем действия, если в словаре есть ключ "propThem"
                    if (propThem.Equals(_whiteThem))
                    {
                        originalStyle = true;
                    }
                    else
                    {
                        originalStyle = false;
                    }
                }
                else
                {
                    //set default value for theme
                    originalStyle = true;
                    App.Current.Properties.Add(Them, _whiteThem);
                }


                if (originalStyle)
                {
                    Resources["TitleApp"] = Resources["TitleAppWhite"];
                    Resources["LableHeadApp"] = Resources["LableHeadAppBlack"];
                    Resources["LabelColor"] = Resources["LabelNavy"];
                    Resources["PickerColor"] = Resources["PickerColorNavy"];
                    Resources["LabelColorWB"] = Resources["LabelBlack"];
                    Resources["ColorWB"] = Resources["ColorBlack"];
                    Resources["ColorBlGr"] = Resources["ColorBlue"];                  
                }
                else
                {
                    Resources["TitleApp"] = Resources["TitleAppBlack"];
                    Resources["LableHeadApp"] = Resources["LableHeadAppWhite"];
                    Resources["LabelColor"] = Resources["LabelYellow"];
                    Resources["LabelColorWB"] = Resources["LabelWhite"];
                    Resources["PickerColor"] = Resources["PickerColorYellow"];
                    Resources["ColorWB"] = Resources["ColorWhite"];
                    Resources["ColorBlGr"] = Resources["ColorYellow"];
                }
            }
            catch (Exception er)
            {
                ErrorHandlerCustom.getErrorMessage(er);
            }
        }



        //метод инициализации БД тестовыми данными
        private void InitDb()
        {
            Dictionary dict = Db.GetDictionarys().FirstOrDefault();
            if (dict == null)
            {
                Db.CreateDictionary(dictInit);
                int z = Db.GetDictionarys().FirstOrDefault().Id;
                foreach (var w in lw)
                {
                    w.IdDictionary = z;
                    Wr.CreateWord(w);
                }

            }
        }

        protected override void OnStart()
        {
            //инициализация базы данных
            InitDb();
        }
        protected override void OnSleep()
        { }
        protected override void OnResume()
        { }
    }
}