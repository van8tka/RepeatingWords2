using RepeatingWords.DataService.Model;
using RepeatingWords.Model;
using RepeatingWords.Pages;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace RepeatingWords
{
    public partial class CreateDb : ContentPage
    {
        public CreateDb()
        {
            InitializeComponent();
        }


        ////для создания директории 
        //bool IsCreateFolder;
        //string rootPath;
        //ChooseFile chFilePage;
        //public CreateDb(bool IsCreateFolder, string rootPath, ChooseFile chFilePage)
        //{
        //    InitializeComponent();
        //    this.IsCreateFolder = IsCreateFolder;
        //    this.rootPath = rootPath;
        //    this.chFilePage = chFilePage;
        //    LabelName.Text = "Введите название каталога";
        //    EnterName.Placeholder = "Введите название каталога";
        //    EnterName.Text = "Новая папка";
        //}




        //вызов главной страницы и чистка стека страниц
        private   void ClickedHomeCustomButton(object sender, EventArgs e)
        {
            //выход на главную страницу
            Application.Current.MainPage = new NavigationPage(new MainView());
        }

        private async void CreateDbButtonClick(object sender, System.EventArgs e)
        {          
                var dictionary = (Dictionary)BindingContext;
                if (!String.IsNullOrEmpty(dictionary.Name))
                {
                    dictionary.Id = 0;
                Debugger.Break();
             //   App.Db.CreateDictionary(dictionary);
                    await Navigation.PopAsync();
                }        
        }
    }
}