using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class EntryTranscriptionViewModel : ViewModelBase
    {
        public EntryTranscriptionViewModel(INavigationService navigationServcie, IDialogService dialogService) : base(navigationServcie, dialogService)
        {
            SendCommand = new Command(Send);
        }
        private Words _word;
        public ICommand SendCommand { get; set; }
        private string _textTranscription;
        public string TextTranscription { get => _textTranscription; set { _textTranscription = value; OnPropertyChanged(nameof(TextTranscription)); } }

        private async void Send()
        {
            _word.Transcription = TextTranscription;
            await NavigationService.RemoveLastFromBackStackAsync();
            await NavigationService.NavigateToAsync<CreateWordViewModel>(_word);
            await NavigationService.RemoveLastFromBackStackAsync();
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is Words word)
            {
                _word = word;
            }
            else
                throw new Exception("Error load EntryTranscriptionVM, bad parameter navigationData");           
            TextTranscription = _word.Transcription;
            return base.InitializeAsync(navigationData);
        }        
    }
}
