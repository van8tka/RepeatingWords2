using Android.Gms.Ads;
using Xamarin.Forms;
using RepeatingWords;
using RepeatingWords.Droid;
using Xamarin.Forms.Platform.Android;
using Android.Content;
 

[assembly: ExportRenderer(typeof(AdMobView), typeof(AdMobRenderer))]
namespace RepeatingWords.Droid
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class AdMobRenderer : ViewRenderer<AdMobView, AdView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<AdMobView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var ad = new AdView(Context);
                ad.AdSize = AdSize.Banner;
                ad.AdUnitId = "ca-app-pub-5993977371632312/4711503801";
                var requestbuilder = new AdRequest.Builder();
                ad.LoadAd(requestbuilder.Build());

                SetNativeControl(ad);
            }
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}
