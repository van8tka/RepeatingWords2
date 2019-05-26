using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RepeatingWords.Model
{
    public class VolumeLanguageModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get; set; }
        private bool _isChecked;
        public bool IsChecked { get=> _isChecked; set { _isChecked = value; OnPropertyChanged(nameof(IsChecked)); } }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       
    }
}
