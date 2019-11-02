using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RepeatingWords.ViewModel
{
    public class VolumeLanguagesViewModel : ViewModelBase
    {
        private readonly IVolumeLanguageService _volumeService;

        public VolumeLanguagesViewModel(INavigationService navigationServcie, IDialogService dialogService,
            IVolumeLanguageService volumeService) : base(navigationServcie, dialogService)
        {
            _volumeService = volumeService;
            Languages = new ObservableCollection<VolumeLanguageModel>();
        }

        public override async Task InitializeAsync(object navigationData)
        {
            SetLanguages();
            _settingsVm = (navigationData as SettingsViewModel);
            await base.InitializeAsync(navigationData);
        }

        private SettingsViewModel _settingsVm;

        private void SetLanguages()
        {
            var currentLanguage = _volumeService.GetVolumeLanguage().Name;
            var tempLanguages = new ObservableCollection<VolumeLanguageModel>(VolumeLanguageList.VolumeLanguages);
            for (int i = 0; i < tempLanguages.Count(); i++)
            {
                if (tempLanguages[i].Name.Equals(currentLanguage, StringComparison.OrdinalIgnoreCase))
                {
                    tempLanguages[i].IsChecked = true;
                    break;
                }
            }
            Languages = tempLanguages;
        }

        private VolumeLanguageModel _selectedItem;

        public VolumeLanguageModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value != null && !value.IsChecked)
                {
                    Languages.First(x => x.IsChecked).IsChecked = false;
                    _selectedItem = value;
                    _selectedItem.IsChecked = true;
                    _volumeService.SetVolumeLanguage(_selectedItem);
                    OnPropertyChanged(nameof(SelectedItem));
                    _settingsVm.CurrentVoiceLanguage = value.Name;
                }
            }
        }

        private ObservableCollection<VolumeLanguageModel> _languages;

        public ObservableCollection<VolumeLanguageModel> Languages
        {
            get => _languages;
            set
            {
                _languages = value;
                OnPropertyChanged(nameof(Languages));
            }
        }

    }
}
