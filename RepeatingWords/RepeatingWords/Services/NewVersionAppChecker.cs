using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
    public class NewVersionAppChecker : INewVersionAppChecker
    {
        private readonly IWebClient _webService;
        private readonly IDialogService _dialogService;
        private readonly ICheckConnect _checkConnect;

        private readonly string uriAndroidVersion =
            "https://play.google.com/store/apps/details?id=cardsofwords.cardsofwords";

        public NewVersionAppChecker(IWebClient webService, IDialogService dialogService, ICheckConnect checkConnect)
        {
            _webService = webService;
            _dialogService = dialogService;
            _checkConnect = checkConnect;
        }

        /// <summary>
        /// проверка новой версии приложения
        /// </summary>
        public async Task CheckNewVersionApp()
        {
            try
            {
               if(await _checkConnect.CheckTheNet() )
               {
                    float webVersion = await _webService.GetVersionApp();
                    float currentVersion = DependencyService.Get<IVersionApp>().GetVersionApp();
                    if (webVersion > currentVersion)
                    {
                        var actionUpdate = await _dialogService.ShowConfirmAsync(Resource.ModalUpdateApp, "", Resource.Yes, Resource.No);
                        if (actionUpdate)
                        {
                            if (Device.RuntimePlatform == Device.Android)
                                if (await Launcher.CanOpenAsync(uriAndroidVersion))
                                    await Launcher.OpenAsync(uriAndroidVersion);
                            //if (Device.RuntimePlatform == "UWP")
                            //    Device.OpenUri(new Uri("https://www.microsoft.com/store/apps/9n55bwkgshnf"));
                        }
                    }
               }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }
    }
}
