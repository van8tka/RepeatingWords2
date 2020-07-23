using System;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Windows.Input;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Model;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class EntryTranscriptionViewModel : ViewModelBase
    {
        public EntryTranscriptionViewModel(INavigationService navigationServcie, IDialogService dialogService) : base(navigationServcie, dialogService)
        {
            SendCommand = new Command(async()=> await Send());
            CursorBlink();
        }
        public ICommand SendCommand { get; set; }
        private string _textTranscription;
        public string TextTranscription { get => _textTranscription; set { _textTranscription = value; OnPropertyChanged(nameof(TextTranscription)); } }

        private bool _isVisibleCursor;

        public bool VisibleStateCursor
        {
            get => _isVisibleCursor;
            set
            {
                _isVisibleCursor = value;
                OnPropertyChanged(nameof(VisibleStateCursor));
            }
        }


        private async Task Send()
        {
            Log.Logger.Info("\n Send Transcription");
            var vm = NavigationService.PreviousPageViewModel as CreateWordViewModel;
            if (vm != null)
                vm.TranscriptionWord = TextTranscription;
            else
                Log.Logger.Error("Error write transcription to privious viewmodel");
            await NavigationService.GoBackPage();
        }


        public override Task InitializeAsync(object navigationData)
        {
                Log.Logger.Info("\n InitializeAsync EntryTranscriptionViewModel");
                IsBusy = true;
                TextTranscription = navigationData as string ?? string.Empty;
                 return base.InitializeAsync(navigationData);
        }

        private async void CursorBlink()
        {
            for (;;)
            {
                await Task.Delay(500);
                VisibleStateCursor = !VisibleStateCursor;
            }
        }
    }
}
