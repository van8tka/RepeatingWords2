using System.Threading;
using System.Threading.Tasks;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface IDialogService
    {
        Task<bool> ShowConfirmAsync(string message, string title, string buttonOk, string buttonCancel);
        Task<string> ShowActionSheetAsync(string title, string cancel, string destructive, CancellationToken token = default, params string[] buttons);
        Task ShowAlertDialog(string message, string oktext, string title = null);
        Task<string> ShowInputTextDialog(string message, string title = null, string okText = null, string cancelText = null, string placeholder = "", CancellationToken cancelToken = default);
    }
}
