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
            var currentLanguage = _volumeService.GetVolumeLanguage();
            var tempLanguages = new ObservableCollection<VolumeLanguageModel>()
            {
                new VolumeLanguageModel()
                    {Name = "Albanian", CountryCode = "AL", LanguageCode = "sq", IsChecked = false},
                new VolumeLanguageModel() {Name = "Arabic", CountryCode = "AE", LanguageCode = "ar", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Belarusian", CountryCode = "BY", LanguageCode = "be", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Bulgarian", CountryCode = "BG", LanguageCode = "bg", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Catalan", CountryCode = "ES", LanguageCode = "ca", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Chinese", CountryCode = "CN", LanguageCode = "zh", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Croatian", CountryCode = "HR", LanguageCode = "hr", IsChecked = false},
                new VolumeLanguageModel() {Name = "Czech", CountryCode = "CZ", LanguageCode = "cs", IsChecked = false},
                new VolumeLanguageModel() {Name = "Danish", CountryCode = "DK", LanguageCode = "da", IsChecked = false},
                new VolumeLanguageModel() {Name = "Dutch", CountryCode = "NL", LanguageCode = "nl", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "English(GB)", CountryCode = "GB", LanguageCode = "en", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "English(US)", CountryCode = "US", LanguageCode = "en", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Estonian", CountryCode = "EE", LanguageCode = "et", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Finnish", CountryCode = "FI", LanguageCode = "fi", IsChecked = false},
                new VolumeLanguageModel() {Name = "French", CountryCode = "FR", LanguageCode = "fr", IsChecked = false},
                new VolumeLanguageModel() {Name = "German", CountryCode = "DE", LanguageCode = "de", IsChecked = false},
                new VolumeLanguageModel() {Name = "Greek", CountryCode = "GR", LanguageCode = "el", IsChecked = false},
                new VolumeLanguageModel() {Name = "Hebrew", CountryCode = "IL", LanguageCode = "iw", IsChecked = false},
                new VolumeLanguageModel() {Name = "Hindi", CountryCode = "IN", LanguageCode = "hi", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Hungarian", CountryCode = "HU", LanguageCode = "hu", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Icelandic", CountryCode = "IS", LanguageCode = "is", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Indonesian", CountryCode = "ID", LanguageCode = "in", IsChecked = false},
                new VolumeLanguageModel() {Name = "Irish", CountryCode = "IE", LanguageCode = "ga", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Italian", CountryCode = "IT", LanguageCode = "it", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Japanese", CountryCode = "JP", LanguageCode = "ja", IsChecked = false},
                new VolumeLanguageModel() {Name = "Korean", CountryCode = "KR", LanguageCode = "ko", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Latvian", CountryCode = "LV", LanguageCode = "lv", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Lithuanian", CountryCode = "LT", LanguageCode = "lt", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Macedonian", CountryCode = "MK", LanguageCode = "mk", IsChecked = false},
                new VolumeLanguageModel() {Name = "Malay", CountryCode = "MY", LanguageCode = "ms", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Maltese", CountryCode = "MT", LanguageCode = "mt", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Norwegian", CountryCode = "NO", LanguageCode = "no", IsChecked = false},
                new VolumeLanguageModel() {Name = "Polish", CountryCode = "PL", LanguageCode = "pl", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Portuguese", CountryCode = "PT", LanguageCode = "pt", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Romanian", CountryCode = "RO", LanguageCode = "ro", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Russian", CountryCode = "RU", LanguageCode = "ru", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Serbian", CountryCode = "RS", LanguageCode = "sr", IsChecked = false},
                new VolumeLanguageModel() {Name = "Slovak", CountryCode = "SK", LanguageCode = "sk", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Slovenian", CountryCode = "SI", LanguageCode = "sl", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Spanish", CountryCode = "ES", LanguageCode = "es", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Swedish", CountryCode = "SE", LanguageCode = "sv", IsChecked = false},
                new VolumeLanguageModel() {Name = "Thai", CountryCode = "TH", LanguageCode = "th", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Turkish", CountryCode = "TR", LanguageCode = "tr", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Ukrainian", CountryCode = "UA", LanguageCode = "uk", IsChecked = false},
                new VolumeLanguageModel()
                    {Name = "Vietnamese", CountryCode = "VN", LanguageCode = "vi", IsChecked = false},
            };
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
