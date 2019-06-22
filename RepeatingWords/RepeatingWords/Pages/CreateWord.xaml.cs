using System;
using RepeatingWords.Pages;
using Xamarin.Forms;
using RepeatingWords.DataService.Model;
using System.Diagnostics;
 

namespace RepeatingWords
{
    public partial class CreateWord : ContentPage
    {
        int idDiction;
        internal string TranscriptionValue { get; set; }
        //1 конструктор для создания
        public CreateWord(int iddiction)
        {
            InitializeComponent();
            TranscriptionValue = string.Empty;
            idDiction = iddiction;
            //переменная для опеределения количества установок курсора на поле транскрипции
            FocusCoutTransc = 1;
        }

        //2 констр для изм слова
        public CreateWord(int iddiction, Words changeword)
        {
            InitializeComponent();
            idDiction = iddiction;
            TranscriptionValue = changeword.Transcription;
            this.BindingContext = changeword;
            FocusCoutTransc = 1;
        }


      

        protected override void OnAppearing()
        {
            base.OnAppearing();
            EntryTranscription.Text = TranscriptionValue;
        }





        //вызов главной страницы и чистка стека страниц
        private   void ClickedHomeCustomButton(object sender, EventArgs e)
        {
            //выход на главную страницу
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        private async void CreateWordButtonClick(object sender, System.EventArgs e)
        {
            try
            {
                var words = (Words)BindingContext;

                string ModelWordAdd = Resource.ModelWordAdd;
                string ModelWordChange = Resource.ModelWordChange;
                string ModelNoFillFull = Resource.ModelNoFillFull;
                string ModelForAddingWord = Resource.ModelForAddingWord;

                if (!String.IsNullOrEmpty(words.RusWord) && !String.IsNullOrEmpty(words.EngWord))
                {
                    if (!String.IsNullOrEmpty(words.Transcription))
                    {
                        if (!words.Transcription.StartsWith("["))
                            words.Transcription = "[" + words.Transcription;
                        if (!words.Transcription.EndsWith("]"))
                            words.Transcription = words.Transcription + "]";
                    }
                    else
                    {
                        words.Transcription = "[]";
                    }
                                     
                    words.IdDictionary = idDiction;
                    Debugger.Break();
                    // App.Wr.CreateWord(words);                               
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert(ModelNoFillFull, ModelForAddingWord, "Ok");
                }
            }
            catch(Exception er) {
                Log.Logger.Error(er);
            }
        }

        int FocusCoutTransc = 1;

        private async void EntryTransc_Focused(object sender, FocusEventArgs e)
        {
            try
            {
                const string TrKeyboard = "TrKeyboard";
                const string showKeyboard = "true";
                object propTrKeyb;
                //проверяем если первый клик то предлогать выбор клав если 2 и более то только сист клавиат
                if (FocusCoutTransc == 1)
                {//получаем значение из настроек сохраненных в приложении
                    App.Current.Properties.TryGetValue(TrKeyboard, out propTrKeyb);
                    //если настроено на показ то показываем окно выбора клавиатуры
                    if (propTrKeyb.Equals(showKeyboard))
                    {
                        // FocusCoutTransc++;

                      

                        string ModalActChooseKeyboard = Resource.ModalActChooseKeyboard;
                        string ModalActCancel = Resource.ModalActCancel;
                        string ModalActFromTrtoF = Resource.ModalActFromTrtoF;
                        string ModalActSysKeyboard = Resource.ModalActSysKeyboard;
                        string ModalActTranscKeyboard = Resource.ModalActTranscKeyboard;

                        var action = await DisplayActionSheet(ModalActChooseKeyboard, ModalActCancel, null, ModalActSysKeyboard, ModalActTranscKeyboard);
                        if (action == ModalActTranscKeyboard)
                        {
                            EntryTranscription.Unfocus();
                            var words = (Words)BindingContext;
                            EntryTranscription tr = new EntryTranscription(words.Transcription, this);
                            await Navigation.PushModalAsync(tr);
                        }
                    }

                }
            }
            catch(Exception er) { Log.Logger.Error(er); }
        }
    }

}