using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ViewModel.User;

namespace DAL.Providers
{
    public interface ITokenProvider
    {
        JWTToken CreateToken(String User_Code);

        TokenValidationParameters GetValidationParameters();
    }
}
