using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
    public class NewVersionAppChecker : INewVersionAppChecker
    {
        private readonly IWebClient _webService;
        private readonly IDialogService _dialogService;


        public NewVersionAppChecker(IWebClient webService, IDialogService dialogService)
        {
            _webService = webService;
            _dialogService = dialogService;
        }

        /// <summary>
        /// проверка новой версии приложения
        /// </summary>
        public async Task CheckNewVersionApp()
        {
            try
            {               
               if( DependencyService.Get<ICheckConnect>().CheckTheNet() )
                {
                    float webVersion = await _webService.GetVersionApp();
                    if (webVersion > Constants.NUMBER_VERSION_ANDROID)
                    {
                        var actionUpdate = await _dialogService.ShowConfirmAsync(Resource.ModalUpdateApp, "", Resource.Yes, Resource.No);
                        if (actionUpdate)
                        {
                            if (Device.RuntimePlatform == "Android")
                                Device.OpenUri(new Uri("https://play.google.com/store/apps/details?id=cardsofwords.cardsofwords"));
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
