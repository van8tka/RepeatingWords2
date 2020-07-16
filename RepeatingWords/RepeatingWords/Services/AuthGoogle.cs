using System;
using System.Collections.Generic;
using System.Text;
using RepeatingWords.Interfaces;
using Xamarin.Auth;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
    public class AuthGoogle
    {
        public AuthGoogle(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        private GoogleOAuthToken _tokenData;
        public GoogleOAuthToken GetAuthToken
        {
            get
            {
                if (_tokenData == null)
                {
                    CreateOauth();
                }
                else if (DateTime.UtcNow.Subtract(_tokenData.TimeCreated).TotalMinutes > 15)
                {
                    CreateOauth();
                }
                return _tokenData;
            }
        }

        private readonly IDialogService _dialogService;
        private readonly string  ClientId = "197917761798-qqgd5bg3kieinjmlvl81bl3h7u0kd22u.apps.googleusercontent.com";
        private readonly string RedirectUrl = "cardsofwords.cardsofwords:/oauth2redirect";
        private const string AuthorizeUrl = "https://accounts.google.com/o/oauth2/v2/auth";
        private const string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        private const bool IsUsingNativeUI = true;


        private void CreateOauth()
        {
           var auth = new OAuth2Authenticator(
                 ClientId,
                string.Empty,
                "email",
                new Uri(AuthorizeUrl),
                new Uri(RedirectUrl),
                new Uri(AccessTokenUrl),
                 null , IsUsingNativeUI);
           auth.Completed += OnAuthenticationCompleted;
           auth.Error += OnAuthenticationFailed;
        }

       private void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                // The user is authenticated
                // Extract the OAuth token
                var token = new GoogleOAuthToken
                {
                    TokenType = e.Account.Properties["token_type"],
                    AccessToken = e.Account.Properties["access_token"],
                    TimeCreated = DateTime.UtcNow
                };

                // Do something
            }
            else
            {
                _dialogService.ShowToast("User could not authorization to Google");
            }
        }

       private void OnAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
       {
          _dialogService.ShowToast("Error authorization to Google");
       }
    }

    public sealed class GoogleOAuthToken
    {
        public string TokenType { get; set; }
        public string AccessToken { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}
