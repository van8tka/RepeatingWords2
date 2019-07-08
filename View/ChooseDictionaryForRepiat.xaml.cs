using RepeatingWords.DataService.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RepeatingWords.View
{
    public partial class ChooseDictionaryForRepiat : ContentPage
    {
         string NameDbForContinued = "ContinueDictionary";
         string NameDbForContinuedLearn = "ContinueDictionary"+Resource.NotLearningPostfics;

        public ChooseDictionaryForRepiat()
        {
            InitializeComponent();
           
        }

        protected async override void OnAppearing()
        {
            progressBar.IsVisible = true;
            await progressBar.ProgressTo(0.9, 1000, Easing.CubicInOut);
            IOrderedEnumerable<Dictionary> items = await LoadData();
            this.BindingContext = items;
            progressBar.IsVisible = false;
            progressBar.Progress  = 0.1;
           base.OnAppearing();
        }

        private Task<IOrderedEnumerable<Dictionary>> LoadData()
        {
            return Task.Run(() => {
                Debugger.Break();
                return new List<Dictionary>().OrderBy(x=>x);
                // return App.Db.GetDictionarys().Where(x => x.Name != NameDbForContinued && x.Name != NameDbForContinuedLearn).OrderBy(x => x.Name);
            });
        }


        //вызов главной страницы и чистка стека страниц
        private   void ClickedHomeCustomButton(object sender, EventArgs e)
        {
            //выход на главную страницу
            Application.Current.MainPage = new NavigationPage(new MainView());
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            bool FromRussia = false;
            Debugger.Break();
          //  var words = App.Wr.GetWords(((Dictionary)e.SelectedItem).Id);

            string ModalChooseLang = Resource.ModalChooseLang;
            string ModalActFromFtoTr = Resource.ModalActFromFtoTr;
            string ModalActFromTrtoF = Resource.ModalActFromTrtoF;
            string ModalException = Resource.ModalException;
            string ModalNoWord = Resource.ModalNoWord;
            string ModalActList = Resource.ModalActList;
            Debugger.Break();
            //if (words.Any())
            //{
            //    var action = await DisplayActionSheet(ModalChooseLang, "", "", ModalActFromFtoTr, ModalActFromTrtoF, ModalActList);
            //    bool which = true;//для проверки выбрана ли кнопка выбора языка или нет

            //    if (action == ModalActFromFtoTr)
            //    {
            //        FromRussia = true;
            //    }
            //    else
            //    {
            //        if (action == ModalActFromTrtoF)
            //        {
            //            FromRussia = false;
            //        }
            //        else
            //        {
            //            if (action == ModalActList)
            //            {
            //                Dictionary d = App.Db.GetDictionary(((Dictionary)e.SelectedItem).Id);
            //                AddWord ad = new AddWord(d);
            //                await Navigation.PushAsync(ad);
            //            }
            //            which = false;
            //        }
            //    }

            //    if (which)
            //    {
            //        RepeatWord rw = new RepeatWord(((Dictionary)e.SelectedItem).Id, FromRussia);
            //        await Navigation.PushAsync(rw);
            //    }
            //}
            //else
            //    await DisplayAlert(ModalException, ModalNoWord, "Ок");

        }

        //обр нажатия добавления словарей из интернета
        protected async void AddWordsFromNetButtonClick(object sender, System.EventArgs e)
        {
            //проверим состояние сети.. вкл или выкл
            bool isConnect = DependencyService.Get<ICheckConnect>().CheckTheNet();
            if (!isConnect)
            {
                await DisplayAlert(Resource.ModalException, Resource.ModalCheckNet, "Ok");
            }
            else
            {
                LanguageFrNet lng = new LanguageFrNet();
                await Navigation.PushAsync(lng);
            }
        }

    }
}