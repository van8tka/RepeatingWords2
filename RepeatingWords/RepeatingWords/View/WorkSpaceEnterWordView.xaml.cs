using System;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;
 
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RepeatingWords.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkSpaceEnterWordView : ContentView, ICustomContentView
    {
        public WorkSpaceEnterWordView()
        {
            InitializeComponent();         
            _customContentViewModel = LocatorService.Container.GetInstance<WorkSpaceEnterWordViewModel>();
            BindingContext = _customContentViewModel as WorkSpaceEnterWordViewModel;             
        }
        private ICustomContentViewModel _customContentViewModel;
        public ICustomContentViewModel CustomVM => _customContentViewModel;
    }
}