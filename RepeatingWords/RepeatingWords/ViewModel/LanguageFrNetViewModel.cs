using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Model;
using System.Linq;
using RepeatingWords.LoggerService;

namespace RepeatingWords.ViewModel
{
    public class LanguageFrNetViewModel : ViewModelBase
    {
        //создаем класс для работы с WebApi сайта и получения данных
        private readonly IWebApiService _webService;
        public LanguageFrNetViewModel(INavigationService navigationServcie, IDialogService dialogService, IWebApiService webService) : base(navigationServcie, dialogService)
        {
            _webService = webService;
            LanguageList = new ObservableCollection<Language>();
            LoadData();
        }

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
                    GoToDictionaryFromNetPage(_selectedItem);
            }
        }


        private async void LoadData()
        {
            try
            {
                //получаем данные в формате Json, Диссериализуем их и получаем языки
                IEnumerable<Language> langList = (await _webService.GetLanguage())?.OrderBy(x => x.NameLanguage);
                if(langList!=null)
                {
                    var list = new ObservableCollection<Language>();
                    for (int i = 0; i < langList.Count(); i++)
                        list.Add(langList.ElementAt(i));
                    LanguageList = list;
                }              
            }
            catch(Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            return base.InitializeAsync(navigationData);
        }

      
        private async void GoToDictionaryFromNetPage(Language selectedLanguage)
        {
            SelectedItem = null;
            await NavigationService.NavigateToAsync<DictionaryFrNetViewModel>(selectedLanguage);         
        }
    }
}
