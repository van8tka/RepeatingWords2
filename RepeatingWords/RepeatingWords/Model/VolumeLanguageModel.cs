using RepeatingWords.LoggerService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RepeatingWords.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RepeatingWords.Model
{
    public class VolumeLanguageModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get; set; }
        public string LanguageCode { get; set; }
        public string CountryCode { get; set; }
        private Color _isChecked;

        public Color IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }

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
            var locales = VolumeLanguageService.Locales;
            _languages = new List<VolumeLanguageModel>();
            for (int i = 0; i < locales.Count(); i++)
            {
                var locale = locales.ElementAtOrDefault(i);
                _languages.Add(new VolumeLanguageModel()
                {
                    Name = locale.Name, CountryCode = locale.Country, LanguageCode = locale.Language,
                    IsChecked = _defaultCheckBoxColor
                });
            }
        }
    }
}
