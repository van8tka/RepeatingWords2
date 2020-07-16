using System;

namespace RepeatingWords.Model
{
    public sealed class GoogleOAuthToken
    {
        public string TokenType { get; set; }
        public string AccessToken { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}
