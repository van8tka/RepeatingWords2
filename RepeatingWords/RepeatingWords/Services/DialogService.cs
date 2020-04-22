using System;
using Acr.UserDialogs;
using RepeatingWords.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace RepeatingWords.Services
{
    public class DialogService : IDialogService
    {
        public Task<bool> ShowConfirmAsync(string message, string title, string buttonOk, string buttonCancel)
        {
            return UserDialogs.Instance.ConfirmAsync(message, title, buttonOk, buttonCancel);
        }

        public Task<string> ShowActionSheetAsync(string title, string cancel, string destructive,
            CancellationToken token = default, params string[] buttons)
        {
            return UserDialogs.Instance.ActionSheetAsync(title, cancel, destructive, token, buttons);
        }

        public Task ShowAlertDialog(string message, string oktext, string title = null)
        {
            return UserDialogs.Instance.AlertAsync(message, title, oktext);
        }

        public async Task<string> ShowInputTextDialog(string message, string title = null, string okText = null,
            string cancelText = null, string placeholder = "", CancellationToken cancelToken = default)
        {
            var result = await UserDialogs.Instance.PromptAsync(message, title, okText, cancelText, placeholder,
                InputType.Default, cancelToken);
            if(result.Ok)
                return result.Text;
            return string.Empty;
        }

        public void ShowLoadDialog(string loadMsg = null)
        {
            UserDialogs.Instance.ShowLoading(string.IsNullOrEmpty(loadMsg) ? Resource.WaitIndicator : loadMsg,
                MaskType.Clear);
        }

        public void ShowToast(string message)
        {
            //UserDialogs.Instance.Toast(message, TimeSpan.FromMilliseconds(3500));
            UserDialogs.Instance.Toast(message, TimeSpan.FromMilliseconds(1000));
        }

        public void HideLoadDialog()
        {
            UserDialogs.Instance.HideLoading();
        }

    }
}
