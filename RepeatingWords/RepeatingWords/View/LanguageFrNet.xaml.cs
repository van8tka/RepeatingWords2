using RepeatingWords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
 

namespace RepeatingWords.View
{
    public partial class LanguageFrNet : ContentPage
    {
        //создаем класс для работы с WebApi сайта и получения данных
        OnlineDictionaryService onService = new OnlineDictionaryService();
       

        public LanguageFrNet()
        {
            InitializeComponent();              
        }


        protected async override void OnAppearing()
        {
          
            progressBar.IsVisible = true;
            await progressBar.ProgressTo(0.9, 10000, Easing.CubicInOut);
            await ListLoad();
        }



        async Task ListLoad()
        {
           
           //получаем данные в формате Json, Диссериализуем их и получаем языки
           IEnumerable<Language> langList = await onService.GetLanguage();
           //передаем языки(список) в xaml элементу Listview
           languageNetList.ItemsSource = langList.OrderBy(x => x.NameLanguage);
            progressBar.IsVisible = false;
            progressBar.Progress = 0.1;
        }



        //вызов главной страницы и чистка стека страниц
        private   void ClickedHomeCustomButton(object sender, EventArgs e)
        {
            //выход на главную страницу
            Application.Current.MainPage = new NavigationPage(new MainView());
        }

       

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                Language lng = (Language)e.SelectedItem;
                DictionarysFrNet dfn = new DictionarysFrNet(lng);
                await Navigation.PushAsync(dfn);        
            }
            catch (Exception er)
            {
               await DisplayAlert("Error", er.Message, "Ok");
            }
        }


    }
}
