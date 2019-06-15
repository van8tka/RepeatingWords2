using RepeatingWords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RepeatingWords.Pages
{
    public partial class DictionarysFrNet : ContentPage
    {
        //создаем класс для работы с WebApi сайта и получения данных
        OnlineDictionaryService onService = new OnlineDictionaryService();
       Language langItem { get; set; }

        public DictionarysFrNet(Language lang)
        {
            InitializeComponent();
            NameLanguage.Text = lang.NameLanguage;
            langItem = lang;          
        }

        protected async override void OnAppearing()
        {
            progressBar.IsVisible = true;
            await progressBar.ProgressTo(0.9, 1000, Easing.CubicInOut);
            //метод загрузки списка словаерй выбранного языка по ID языка
           await ListLoad(langItem.Id);
            progressBar.Progress = 0.1;
            progressBar.IsVisible = false;
        }
        //загрузка списка словарей выбранного языка
        private async Task ListLoad(int idLang)
        {
            //получаем данные в формате Json, Диссериализуем их и получаем словари
            IEnumerable<Dictionary> dictionaryList = await onService.GetLanguage(idLang);
            //передаем словари(список) в xaml элементу Listview
            dictionaryNetList.ItemsSource = dictionaryList.OrderBy(x => x.Name);
        }

        //вызов главной страницы и чистка стека страниц
        private   void ClickedHomeCustomButton(object sender, EventArgs e)
        {
            //выход на главную страницу
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
        
       

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                progressBar.IsVisible = true;
                await progressBar.ProgressTo(0.9, 1000, Easing.CubicInOut);
                Dictionary dictionaryNet = (Dictionary)e.SelectedItem;
                int id = dictionaryNet.Id;
                //создаем этот словарь локально
                App.Db.CreateDictionary(dictionaryNet);             
                //получаем список слов выбранного словаря в интеренете
                IEnumerable<Words> wordsList = await onService.Get(id);
                //получаем последний словарь(который только что создали)
                int idLast = App.Db.GetDictionarys().LastOrDefault().Id;
                //проходим по списку слов и создаем слова для этого словаря
                await GreateWords(idLast, wordsList);
                AddWord adw = new AddWord(dictionaryNet);
                await Navigation.PushAsync(adw);
                progressBar.IsVisible = false;
                progressBar.Progress = 0.1;
            }
            catch (Exception er)
            {
                await DisplayAlert("Error", er.Message, "Ok");
            }
        }
        //асинхронный метод добавления слов в выбранный словарь
        private async Task GreateWords(int idLast, IEnumerable<Words> wordsList)
        {
            foreach (var i in wordsList)
            {
                Words newW = new Words()
                {
                    Id = 0,
                    IdDictionary = idLast,
                    RusWord = i.RusWord,
                    Transcription = i.Transcription,
                    EngWord = i.EngWord
                };
                 await Task.Run(()=> App.Wr.CreateWord(newW));
            }
        }
    }
}