using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class AuthTokenResponse
    {
        public string Token { get; set; }
        public int TokenExpiresAfter { get; set; }

    }
}
