using System;
using Android.App;
using Android.Content;
using Android.OS;

namespace RepeatingWords.Droid
{
    [Activity(Label= "GoogleAuthInterceptor")]
    [IntentFilter(actions:new []{Intent.ActionView}, Categories = new []{Intent.CategoryDefault, Intent.CategoryBrowsable}, DataSchemes = new []{"cardsofwords.cardsofwords"}, DataPaths = new []{ "/oauth2redirect" } )]
   public class GoogleAuthInterceptor:Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Android.Net.Uri uri_android = Intent.Data;
            var uri_ = new Uri(uri_android.ToString());
            MainActivity.Auth?.OnPageLoading(uri_);
            Finish();
        }
    }
}