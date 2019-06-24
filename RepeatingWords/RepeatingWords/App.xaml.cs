using System;
using Xamarin.Forms;
using Unity;
using RepeatingWords.DataService.Repositories;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.Interfaces;
using RepeatingWords.Services;
using RepeatingWords.LoggerService;
using RepeatingWords.ViewModel;
using RepeatingWords.Helpers.Interfaces;

namespace RepeatingWords
{
    public partial class App : Application
    {
        //переменная для определения стиля темы приложения
        private bool originalStyle = true;
        public const string DATABASE_NAME = "repeatwords.db";
        private readonly IUnityContainer _container;

        const string Them = "theme";
        const string _whiteThem = "white";
        const string _blackThem = "black";
 

        public App(ISQLite sqlitePath)
        {            
            InitializeComponent();
            _container = new UnityContainer();
            Boot(sqlitePath);
            CleanStackAndGoRootPage();
            SetOriginalStyle();
            SetChooseTranscriptionKeyboard();
            InitDb();          
        }

        private void InitDb()
        {
            var init = _container.Resolve<IInitDefaultDb>();
            init.LoadDefaultData();
        }

        private void Boot(ISQLite sqlitePath)
        {           
            _container.RegisterInstance(typeof(ISQLite), sqlitePath);
            _container.RegisterInstance(typeof(ILogger), DependencyService.Get<ILogManager>().GetLog());
            _container.RegisterType<ILoggerService, Log>();
            _container.Resolve<ILoggerService>();
            _container.RegisterInstance(typeof(IUnitOfWork),new UnitOfWork(sqlitePath.GetDatabasePath(DATABASE_NAME)));
            _container.RegisterType<IInitDefaultDb, InitDefaultDb>();
            _container.RegisterInstance(typeof(INavigation), )
            _container.RegisterType<IMainPage, MainPageVM>();
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
                Log.Logger.Error(er);
            }
        }

      

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
                Log.Logger.Error(er);
            }
        }


        protected override void OnStart()
        { }
        protected override void OnSleep()
        { }
        protected override void OnResume()
        { }       
    }
}