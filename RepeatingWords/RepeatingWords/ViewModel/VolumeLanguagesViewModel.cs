using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;

namespace RepeatingWords.ViewModel
{
    public class VolumeLanguagesViewModel : ViewModelBase
    {
        private readonly IVolumeLanguageService _volumeService;

        public VolumeLanguagesViewModel(INavigationService navigationServcie, IDialogService dialogService, IVolumeLanguageService volumeService) : base(navigationServcie, dialogService)
        {
            _volumeService = volumeService;
            Languages = new ObservableCollection<VolumeLanguageModel>();
            _volumeLangList = new VolumeLanguageList();
        }

        public override async Task InitializeAsync(object navigationData)
        {
            SetLanguages();
            _settingsVm = (navigationData as SettingsViewModel);
            await base.InitializeAsync(navigationData);
        }

     
        private SettingsViewModel _settingsVm;
        private VolumeLanguageList _volumeLangList;

        private void SetLanguages()
        {
            var currentLanguage = _volumeService.GetVolumeLanguage();

            var tempLanguages = new ObservableCollection<VolumeLanguageModel>(_volumeLangList.VolumeLanguages);
            for (int i = 0; i < tempLanguages.Count(); i++)
            {
                if (tempLanguages[i].Name.Equals(currentLanguage, StringComparison.OrdinalIgnoreCase))
                {
                    tempLanguages[i].IsChecked = _volumeLangList.CheckedColor;
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
                if (value != null)
                {
                    Languages.First(x => x.IsChecked.Equals(_volumeLangList.CheckedColor)).IsChecked = _volumeLangList.UncheckedColor;
                    _selectedItem = value;
                    _selectedItem.IsChecked = _volumeLangList.CheckedColor;
                    Log.Logger.Info("Set language voice "+value.Name+" "+value.CountryCode+" " +value.LanguageCode);
                    OnPropertyChanged(nameof(SelectedItem));
                    SetVoiceLangToSettings();
                }
            }
        }

        private void SetVoiceLangToSettings()
        {
            try
            {
                _volumeService.SetVolumeLanguage(SelectedItem.Name);
                _settingsVm.CurrentVoiceLanguage = SelectedItem.Name;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
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
