using Android.App;
using Android.Net;
using Android.Content;
using System.Net;

[assembly: Xamarin.Forms.Dependency(typeof(RepeatingWords.Droid.CheckConnect))]

namespace RepeatingWords.Droid
{
    public class CheckConnect : ICheckConnect
    {
    
        public bool CheckTheNet()
        {
            bool isConnected;
            var cm = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            if (cm.ActiveNetworkInfo != null)
                isConnected = cm.ActiveNetworkInfo.IsConnected;                
            else
                isConnected = false;
            if (isConnected)
                return CheckGoogleConnected();
            else
                return isConnected;
        }

        private bool CheckGoogleConnected()
        {
            try
            {
                var url = "https://google.com";
                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Timeout = 3000;
                var resp = webRequest.GetResponse();
                resp.Close();
                return true;
            }
            catch(WebException e)
            {
                return false;
            }
        }
    }
}