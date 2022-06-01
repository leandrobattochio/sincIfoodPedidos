using System;

namespace Financas.Ifood
{
    public class NotAuthorizedIfoodException : Exception
    {
        public string RefreshToken { get; private set; }
        public string Email { get; private set; }

        public NotAuthorizedIfoodException(string refreshToken, string email)
        {
            RefreshToken = refreshToken;
            Email = email;
        }
    }
}
