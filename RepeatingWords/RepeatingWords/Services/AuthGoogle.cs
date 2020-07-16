using System;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Model;
using Xamarin.Auth;

namespace RepeatingWords.Services
{
    public class AuthGoogle
    { 
        
        private const string AuthorizeUrl = "https://accounts.google.com/o/oauth2/v2/auth";
        private const string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        private const bool IsUsingNativeUI = true;
        private OAuth2Authenticator _auth;
        private IGoogleAuthenticationDelegate _authenticationDelegate;

        public AuthGoogle(string clientId, string scope, string redirectUrl, IGoogleAuthenticationDelegate authenticationDelegate)
        {
            _authenticationDelegate = authenticationDelegate;
             _auth = new OAuth2Authenticator(
               clientId,
                string.Empty,
               scope,
                new Uri(AuthorizeUrl),
                new Uri(redirectUrl),
                new Uri(AccessTokenUrl),
                 null , IsUsingNativeUI);
           _auth.Completed += OnAuthenticationCompleted;
           _auth.Error += OnAuthenticationFailed;
        }

       private void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                // The user is authenticated
                // Extract the OAuth token
               var tokenData = new GoogleOAuthToken
                {
                    TokenType = e.Account.Properties["token_type"],
                    AccessToken = e.Account.Properties["access_token"],
                    TimeCreated = DateTime.UtcNow
                };
               _authenticationDelegate.OnAuthenticationCompleted(tokenData);
            }
            else
            {
                _authenticationDelegate.OnAuthenticationCanceled();
            }
        }

       private void OnAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
       {
           _authenticationDelegate.OnAuthenticationCanceled();
       }

       public OAuth2Authenticator GetAuthenticator()
       {
           return _auth;
       }

       public void OnPageLoading(Uri uri)
       {
           _auth.OnPageLoading(uri);
       }
    }

   
}
