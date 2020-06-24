using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormsControls.Base;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RepeatingWords.View
{
    public partial class FinishLearnView : IAnimationPage
    {
    public FinishLearnView()
    {
        InitializeComponent();
        //fullscreen advertizing
        //  DependencyService.Get<IAdmobInterstitial>().Show("ca-app-pub-5993977371632312/4024752876");
        var vm =    LocatorService.Container.GetInstance<FinishLearnViewModel>();
        vm.VCongratulation = stCongratulation;
        vm.VLearned = lbLearned;
        vm.VUnlerned = lbUnlearned;
        vm.VLearnMore = stLearnMore;
        BindingContext = vm;
    }
    public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromRight };
    public void OnAnimationStarted(bool isPopAnimation) { }
    public void OnAnimationFinished(bool isPopAnimation) { }
    }
}
