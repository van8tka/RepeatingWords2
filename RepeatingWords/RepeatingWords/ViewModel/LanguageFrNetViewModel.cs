using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RepeatingWords.Helpers.Interfaces;
using System.Linq;
using RepeatingWords.DataService.Model;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class LanguageFrNetViewModel : ViewModelBase
    {
        //создаем класс для работы с WebApi сайта и получения данных
        private readonly IWebClient _webService;
        public LanguageFrNetViewModel(INavigationService navigationServcie, IDialogService dialogService, IWebClient webService, ILanguageLoaderFacade languageLoader, ICheckConnect checkConnect) : base(navigationServcie, dialogService)
        {
            _webService = webService;
            LanguageList = new ObservableCollection<Language>();
            DialogService.ShowLoadDialog();
            _languageLoader = languageLoader ?? throw new ArgumentNullException(nameof(languageLoader));
            _checkConnect = checkConnect;
        }

        private readonly ICheckConnect _checkConnect;
        private readonly ILanguageLoaderFacade _languageLoader;

        private ObservableCollection<Language> _languageList;
        public ObservableCollection<Language> LanguageList { get => _languageList; set { _languageList = value; OnPropertyChanged(nameof(LanguageList)); } }

        private Language _selectedItem;
        public Language SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                if (_selectedItem != null)
                {
                    DialogService.ShowLoadDialog();
                    LoadLanguage(_selectedItem);                   
                }                   
            }
        }


        public async Task LoadLanguage(Language selectedLanguage)
        {
            SelectedItem = null;
            try
            {
                await _languageLoader.LoadLanguageFromApi(selectedLanguage.Id, selectedLanguage.NameLanguage);
                DialogService.HideLoadDialog();
                await NavigationService.GoBackPage();
            }
            catch (Exception e)
            {
                DialogService.HideLoadDialog();
                DialogService.ShowToast(e.Message);
                Log.Logger.Error(e);
            }           
        }


        private async Task LoadData()
        {
            try
            {
              
               if(await _checkConnect.CheckTheNet())
               {
                    //получаем данные в формате Json, Диссериализуем их и получаем языки
                    IEnumerable<Language> langList = (await _webService.GetLanguage())?.OrderBy(x => x.NameLanguage);
                    if (langList != null)
                    {
                        var list = new ObservableCollection<Language>();
                        for (int i = 0; i < langList.Count(); i++)
                            list.Add(langList.ElementAt(i));
                        LanguageList = list;
                    }
               }
               else 
               {
                   DialogService.ShowToast(Resource.ModalCheckNet);                   
               }                 
            }
            catch(Exception e)
            {
                Log.Logger.Error(e);
                DialogService.ShowToast(e.Message);
            }
        }

        public override async Task InitializeAsync(object navigationData)
        {
            try
            {
                IsBusy = true;
                await LoadData();
                await base.InitializeAsync(navigationData);
                DialogService.HideLoadDialog();
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
            }
        }
    }
}
