﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Model;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class EntryTranscriptionViewModel : ViewModelBase
    {
        public EntryTranscriptionViewModel(INavigationService navigationServcie, IDialogService dialogService) : base(navigationServcie, dialogService)
        {
            SendCommand = new Command(async()=>await Send());
        }
        private WordsModel _word;
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
            IsBusy = true;
            _word.Transcription = TextTranscription;
           await NavigationService.RemoveLastFromBackStackAsync();
           await NavigationService.NavigateToAsync<CreateWordViewModel>(_word);
           await NavigationService.RemoveLastFromBackStackAsync();
        }


        public override async Task InitializeAsync(object navigationData)
        {
            _word = navigationData as WordsModel;
            TextTranscription = _word?.Transcription??string.Empty;
            await CursorBlink();
            await base.InitializeAsync(navigationData);
        }

        private async Task CursorBlink()
        {
            for (;;)
            {
                await Task.Delay(500);
                VisibleStateCursor = !VisibleStateCursor;
            }
        }
    }
}
