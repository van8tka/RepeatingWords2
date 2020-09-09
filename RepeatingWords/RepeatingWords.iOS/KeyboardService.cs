using System;
using RepeatingWords.Helpers.Interfaces;
using Xamarin.Forms;
using RepeatingWords.iOS;

[assembly: Dependency(typeof(KeyboardService))]
namespace RepeatingWords.iOS
{
    public class KeyboardService : IKeyboardService
    {
        public void HideKeyboard()
        {
            throw new NotImplementedException("Not implemented HideKeyboard");
        }
        public void ShowKeyboard()
        {
            throw new NotImplementedException("Not implemented ShowKeyboard");
        }
    }
}
