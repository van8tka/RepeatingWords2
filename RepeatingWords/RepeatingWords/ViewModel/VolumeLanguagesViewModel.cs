using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Model;

namespace RepeatingWords.ViewModel
{
    public class VolumeLanguagesViewModel : ViewModelBase
    {
        private readonly IVolumeLanguageService _volumeService;
        public VolumeLanguagesViewModel(INavigationService navigationServcie, IDialogService dialogService, IVolumeLanguageService volumeService) : base(navigationServcie, dialogService)
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
            var currentLanguage = _volumeService.GetVolumeLanguage();
            var tempLanguages = new ObservableCollection<VolumeLanguageModel>()
            {
                new VolumeLanguageModel(){ Name = "English", IsChecked=false },
                new VolumeLanguageModel(){ Name = "Русский", IsChecked=false },
                new VolumeLanguageModel(){ Name = "French", IsChecked=false },
                new VolumeLanguageModel(){ Name = "German", IsChecked=false },
                new VolumeLanguageModel(){ Name = "Polish", IsChecked=false },
                new VolumeLanguageModel(){ Name = "Ukrainian", IsChecked=false },
                new VolumeLanguageModel(){ Name = "Italian", IsChecked=false },
                new VolumeLanguageModel(){ Name = "Chinese", IsChecked=false },
                new VolumeLanguageModel(){ Name = "Japanese", IsChecked=false },
                new VolumeLanguageModel(){ Name = "Portuguese(Brazil)", IsChecked=false },
                new VolumeLanguageModel(){ Name = "Spanish", IsChecked=false },
                new VolumeLanguageModel(){ Name = "Turkish", IsChecked=false }
            };
            for(int i=0;i< tempLanguages.Count();i++)
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
                if(value!=null && !value.IsChecked)
                {
                     Languages.First(x => x.IsChecked).IsChecked = false;
                    _selectedItem = value;
                    _selectedItem.IsChecked = true;
                    _volumeService.SetVolumeLanguage(_selectedItem.Name);
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
