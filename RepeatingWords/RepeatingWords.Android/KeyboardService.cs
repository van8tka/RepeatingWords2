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
            MainActivity context;
            var inputManager = GetInputMethodManager(out context);
            if (inputManager != null && context is Activity activity)
            {
                var token = activity.CurrentFocus?.WindowToken;
                inputManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);
                activity.Window.DecorView.ClearFocus();
            }
        }

        private InputMethodManager GetInputMethodManager(out MainActivity context)
        {
            context = MainActivity.Instance;
            return context.ApplicationContext.GetSystemService(Context.InputMethodService) as InputMethodManager;
        }

        public void ShowKeyboard()
        {
            MainActivity context;
            var inputManager = GetInputMethodManager(out context);          
            inputManager.ShowSoftInput(context.CurrentFocus, 0);
        }
    }
}