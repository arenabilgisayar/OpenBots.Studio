using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace OpenBots.Commands.Documents.Models
{
    [Serializable]
    public class AuthenticationRequest
    {
        public AuthenticationRequest()
        {

        }

        public string UserNameOrEmailAddress { get; set; }

        public string Password { get; set; }

        public string TwoFactorVerificationCode { get; set; }

        public bool RememberClient { get; set; }

        public string TwoFactorRememberClientToken { get; set; }

        public bool? SingleSignIn { get; set; }

        public string ReturnUrl { get; set; }

        public bool IsTwoFactorVerification => !string.IsNullOrEmpty(TwoFactorVerificationCode);
    }
}
