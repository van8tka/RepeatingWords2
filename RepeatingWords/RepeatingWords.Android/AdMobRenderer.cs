using Android.Gms.Ads;
using Xamarin.Forms;
using RepeatingWords;
using RepeatingWords.Droid;
using Xamarin.Forms.Platform.Android;
using Android.Content;
 

[assembly: ExportRenderer(typeof(AdMobView), typeof(AdMobRenderer))]
namespace RepeatingWords.Droid
{
    public class AdMobRenderer : ViewRenderer<AdMobView, AdView>
    {
      //  public AdMobRenderer(Context context) : base(context) { }
        protected override void OnElementChanged(ElementChangedEventArgs<AdMobView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var ad = new AdView(Context);
                ad.AdSize = AdSize.Banner;
                ad.AdUnitId = "ca-app-pub-5351987413735598/7586925463";

                var requestbuilder = new AdRequest.Builder();
                ad.LoadAd(requestbuilder.Build());

                SetNativeControl(ad);
            }
        }
    }
}

//    public class AdMobRenderer : ViewRenderer<AdMobView, AdView>
//{
//    string idAbMobBanner = "ca-app-pub-5351987413735598/7586925463";
//    public AdMobRenderer(Context context):base(context)  { }

//    protected override void OnElementChanged(ElementChangedEventArgs<AdMobView> e)
//    {
//        base.OnElementChanged(e);

//        if (e.NewElement != null && Control == null)
//            SetNativeControl(CreateAdView());
//    }

//        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            base.OnElementPropertyChanged(sender, e);

//            if (e.PropertyName == nameof(AdView.AdUnitId))
//                Control.AdUnitId = idAbMobBanner;
//        }

//        private AdView CreateAdView()
//    {
//        var adView = new AdView(Context)
//        {
//            AdSize = AdSize.SmartBanner,
//            AdUnitId = idAbMobBanner
//        };

//        adView.LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

//        adView.LoadAd(new AdRequest.Builder().Build());

//        return adView;
//    }
//}
//}