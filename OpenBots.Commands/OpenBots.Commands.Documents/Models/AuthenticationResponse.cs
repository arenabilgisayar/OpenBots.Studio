﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBots.Commands.Documents.Models
{
    [Serializable]
    public class AuthenticationResponse
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public bool ShouldResetPassword { get; set; }

        public string PasswordResetCode { get; set; }

        public long UserId { get; set; }

        public bool RequiresTwoFactorVerification { get; set; }

        public IList<string> TwoFactorAuthProviders { get; set; }

        public string TwoFactorRememberClientToken { get; set; }

        public string ReturnUrl { get; set; }

        public DateTime RefreshTokenExpireDate { get; set; }
    }
}
