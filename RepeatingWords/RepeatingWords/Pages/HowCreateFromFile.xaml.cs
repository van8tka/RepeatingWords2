﻿
using System;
using Xamarin.Forms;

namespace RepeatingWords.Pages
{
    public partial class HowCreateFromFile : ContentPage
    {
        public HowCreateFromFile()
        {
            InitializeComponent();
        }


        //вызов главной страницы и чистка стека страниц
        private   void ClickedHomeCustomButton(object sender, EventArgs e)
        {
            //выход на главную страницу
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}
