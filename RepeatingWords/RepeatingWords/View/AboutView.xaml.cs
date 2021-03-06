﻿using FormsControls.Base;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Xamarin.Forms;
namespace RepeatingWords.View
{

    public partial class AboutView : ContentPage, IAnimationPage
    {
        public AboutView()
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.GetInstance<AboutViewModel>();
        }
        public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromRight };
        public void OnAnimationStarted(bool isPopAnimation) { }
        public void OnAnimationFinished(bool isPopAnimation) { }
    }

}
