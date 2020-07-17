using System;
using System.Threading.Tasks;
using RepeatingWords.Model;

namespace RepeatingWords.Helpers.Interfaces
{
   public interface IGoogleAuthenticationDelegate
    {
        Task OnAuthenticationCompleted(GoogleOAuthToken token);
        void OnAuthenticationFailed(string message, Exception exception);
        void OnAuthenticationCanceled();
    }
}
