using Acr.UserDialogs;
using RepeatingWords.Helpers.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace RepeatingWords.Services
{
    public class DialogService : IDialogService
    {       
        public Task<bool> ShowConfirmAsync( string message, string title, string buttonOk, string buttonCancel )
        {
            return UserDialogs.Instance.ConfirmAsync( message, title, buttonOk, buttonCancel );
        }

        public Task<string> ShowActionSheetAsync( string title, string cancel, string destructive, CancellationToken token=default,params string [] buttons )
        {
            return UserDialogs.Instance.ActionSheetAsync( title, cancel, destructive, token, buttons );
        }

        public Task ShowAlertDialog(string message,  string oktext, string title = null)
        {
            return UserDialogs.Instance.AlertAsync(message, title, oktext);
        }
    }
}
