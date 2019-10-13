using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Model;
using System.Linq;
using RepeatingWords.LoggerService;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class LanguageFrNetViewModel : ViewModelBase
    {
        //создаем класс для работы с WebApi сайта и получения данных
        private readonly IWebClient _webService;
        public LanguageFrNetViewModel(INavigationService navigationServcie, IDialogService dialogService, IWebClient webService, ILanguageLoaderFacade languageLoader) : base(navigationServcie, dialogService)
        {
            _webService = webService;
            LanguageList = new ObservableCollection<Language>();
            DialogService.ShowLoadDialog();
            _languageLoader = languageLoader ?? throw new ArgumentNullException(nameof(languageLoader));
        }

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


        public async void LoadLanguage(Language selectedLanguage)
        {
            SelectedItem = null;
            try
            {
                await _languageLoader.LoadSelectedLanguageToDB(selectedLanguage);
                DialogService.HideLoadDialog();
                await NavigationService.RemoveLastFromBackStackAsync();
                await NavigationService.NavigateToAsync<DictionariesListViewModel>();
                await NavigationService.RemoveLastFromBackStackAsync();
            }
            catch(Exception e)
            {
                DialogService.HideLoadDialog();
                Log.Logger.Error(e);
            }           
        }


        private async Task LoadData()
        {
            try
            {
                bool isNet = await Task.Run(() => DependencyService.Get<ICheckConnect>().CheckTheNet());
               if(isNet)
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
                    await DialogService.ShowAlertDialog(Resource.ModalCheckNet, Resource.Continue, Resource.ModalException);                   
                }                 
            }
            catch(Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        public override async Task InitializeAsync(object navigationData)
        {
                   
            IsBusy = true;
            await LoadData();
            await base.InitializeAsync(navigationData);
            DialogService.HideLoadDialog();
        }
    }
}
