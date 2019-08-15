using System;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Unity;
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
            _customContentViewModel = LocatorService.Container.Resolve<WorkSpaceEnterWordViewModel>();
            BindingContext = _customContentViewModel as WorkSpaceEnterWordViewModel;             
        }
        private ICustomContentViewModel _customContentViewModel;
        public ICustomContentViewModel CustomVM => _customContentViewModel;
    }
}