using System.Net;
using RepeatingWords.LoggerService;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace RepeatingWords.Services
{
    public class CheckConnect : ICheckConnect
    {
        public Task<bool> CheckTheNet()
        {
            return Task.Run(()=>
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet && CheckGoogleConnected())
                {
                    Log.Logger.Info("\n Internet connection is AVAILABLE");
                    return true;
                }
                Log.Logger.Info("\n Internet connection is DISABLE");
                return false;
            });
        }

        private bool CheckGoogleConnected()
        {
            try
            {
                var url = "https://google.com";
                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Timeout = 5000;
                var resp = webRequest.GetResponse();
                resp.Close();
                return true;
            }
            catch (WebException e)
            {
                Log.Logger.Error(e);
                return false;
            }
        }
    }
}
