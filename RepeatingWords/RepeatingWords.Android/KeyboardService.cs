using Android.App;
using Android.Content;
using Android.Views.InputMethods;
using RepeatingWords.Droid;
using RepeatingWords.Helpers.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(KeyboardService))]
namespace RepeatingWords.Droid
{
    
    public class KeyboardService : IKeyboardService
    {
        public void HideKeyboard()
        {
            var context =  MainActivity.Instance;
            var inputManager = context.ApplicationContext.GetSystemService(Context.InputMethodService) as InputMethodManager;
            if(inputManager !=null && context is Activity activity)
            {
                var token = activity.CurrentFocus?.WindowToken;
                inputManager.HideSoftInputFromInputMethod(token, HideSoftInputFlags.None);
                activity.Window.DecorView.ClearFocus();
            }
        }
    }
}