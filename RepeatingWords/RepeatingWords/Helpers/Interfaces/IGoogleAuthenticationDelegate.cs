using System;
using RepeatingWords.Model;

namespace RepeatingWords.Helpers.Interfaces
{
   public interface IGoogleAuthenticationDelegate
    {
        void OnAuthenticationCompleted(GoogleOAuthToken token);
        void OnAuthenticationFailed(string message, Exception exception);
        void OnAuthenticationCanceled();
    }
}
