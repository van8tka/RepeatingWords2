using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace RepeatingWords.Model
{
    public class VolumeLanguageModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get; set; }
        public string LanguageCode { get; set; }
        public string CountryCode { get; set; }
        private Color _isChecked;
        public Color IsChecked { get=> _isChecked; set { _isChecked = value; OnPropertyChanged(nameof(IsChecked)); } }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class VolumeLanguageList
    {
        public VolumeLanguageList()
        {
            CheckedColor = Color.FromHex("#6bafef");
            UncheckedColor = Color.FromHex("#CDC9C9");
            InitLanguages();
        }

        private Color _checkedColor;
        public Color CheckedColor
        {
            get => _checkedColor;
            private set => _checkedColor = value;
        }

        private Color _defaultCheckBoxColor;
        public Color UncheckedColor
        {
            get => _defaultCheckBoxColor;
            private set => _defaultCheckBoxColor = value;
        }



        private static List<VolumeLanguageModel> _languages;
        public IReadOnlyCollection<VolumeLanguageModel> VolumeLanguages => _languages;

        private void InitLanguages()
        {
            _languages = new List<VolumeLanguageModel>()
            {
                 new VolumeLanguageModel()
                    {Name = "Albanian", CountryCode = "AL", LanguageCode = "sq", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel() {Name = "Arabic", CountryCode = "AE", LanguageCode = "ar", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Belarusian", CountryCode = "BY", LanguageCode = "be", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Bulgarian", CountryCode = "BG", LanguageCode = "bg", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Catalan", CountryCode = "ES", LanguageCode = "ca", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Chinese", CountryCode = "CN", LanguageCode = "zh", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Croatian", CountryCode = "HR", LanguageCode = "hr", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel() {Name = "Czech", CountryCode = "CZ", LanguageCode = "cs", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel() {Name = "Danish", CountryCode = "DK", LanguageCode = "da", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel() {Name = "Dutch", CountryCode = "NL", LanguageCode = "nl", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "English(GB)", CountryCode = "GB", LanguageCode = "en", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "English(US)", CountryCode = "US", LanguageCode = "en", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Estonian", CountryCode = "EE", LanguageCode = "et", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Finnish", CountryCode = "FI", LanguageCode = "fi", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel() {Name = "French", CountryCode = "FR", LanguageCode = "fr", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel() {Name = "German", CountryCode = "DE", LanguageCode = "de", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel() {Name = "Greek", CountryCode = "GR", LanguageCode = "el", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel() {Name = "Hebrew", CountryCode = "IL", LanguageCode = "iw", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel() {Name = "Hindi", CountryCode = "IN", LanguageCode = "hi", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Hungarian", CountryCode = "HU", LanguageCode = "hu", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Icelandic", CountryCode = "IS", LanguageCode = "is", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Indonesian", CountryCode = "ID", LanguageCode = "in", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel() {Name = "Irish", CountryCode = "IE", LanguageCode = "ga", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Italian", CountryCode = "IT", LanguageCode = "it", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Japanese", CountryCode = "JP", LanguageCode = "ja", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel() {Name = "Korean", CountryCode = "KR", LanguageCode = "ko", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Latvian", CountryCode = "LV", LanguageCode = "lv", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Lithuanian", CountryCode = "LT", LanguageCode = "lt", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Macedonian", CountryCode = "MK", LanguageCode = "mk", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel() {Name = "Malay", CountryCode = "MY", LanguageCode = "ms", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Maltese", CountryCode = "MT", LanguageCode = "mt", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Norwegian", CountryCode = "NO", LanguageCode = "no", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel() {Name = "Polish", CountryCode = "PL", LanguageCode = "pl", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Portuguese", CountryCode = "PT", LanguageCode = "pt", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Romanian", CountryCode = "RO", LanguageCode = "ro", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Russian", CountryCode = "RU", LanguageCode = "ru", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Serbian", CountryCode = "RS", LanguageCode = "sr", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel() {Name = "Slovak", CountryCode = "SK", LanguageCode = "sk", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Slovenian", CountryCode = "SI", LanguageCode = "sl", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Spanish", CountryCode = "ES", LanguageCode = "es", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Swedish", CountryCode = "SE", LanguageCode = "sv", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel() {Name = "Thai", CountryCode = "TH", LanguageCode = "th", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Turkish", CountryCode = "TR", LanguageCode = "tr", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Ukrainian", CountryCode = "UA", LanguageCode = "uk", IsChecked = _defaultCheckBoxColor},
                new VolumeLanguageModel()
                    {Name = "Vietnamese", CountryCode = "VN", LanguageCode = "vi", IsChecked = _defaultCheckBoxColor}
            };
        }
}
}
