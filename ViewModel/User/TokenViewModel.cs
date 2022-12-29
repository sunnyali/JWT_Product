using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.User
{
    public class TokenUser
    {
        public string UserName { get; set; } = string.Empty;
    }

    //JsonWebToken
    public class JWT
    {
        public string access_token { get; set; }

        public string token_type { get; set; } = "bearer";

        public int expires_in { get; set; }

        public string refresh_token { get; set; }
    }

    public class TokenRequest
    {
        [Required]
        public string SessionID { get; set; }

        [Required]
        public string UserID { get; set; }

        public string IPAddress { get; set; }
        public string refresh_token { get; set; }

        [Required]
        public string grant_type { get; set; }
    }

    public class JWTToken
    {
        public string Access_Token { get; set; }
        public string Refresh_Token { get; set; }
    }
}
