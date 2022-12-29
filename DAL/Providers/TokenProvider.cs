using DAL.Helper;
using Microsoft.IdentityModel.Tokens;
using Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ViewModel.User;

namespace DAL.Providers
{
   public class TokenProvider
    {


        private readonly string _issuer;
        private readonly string _audience;
        private readonly SymmetricSecurityKey _signingKey;
        public Properties _properties = new();
        public DBContext objCW = new();

        public TokenProvider()
        {
            _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_properties.JWTSecret));
            _issuer = _properties.Issuer;
            _audience = _properties.Audience;
        }

        public JWTToken CreateToken(string Customer_Id)
        {
            JWTToken result;
            if (!string.IsNullOrEmpty(Customer_Id))

            {
                var expiry = GetDateTimeNow().AddMinutes(_properties.AuthTokenTimeOut);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,Customer_Id)
                };

                var creds = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha512Signature);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = expiry,
                    SigningCredentials = creds,
                    Issuer = _issuer,
                    Audience = _audience,
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                result = new JWTToken
                {
                    Access_Token = tokenHandler.WriteToken(token),
                    Refresh_Token = GenerateRefreshToken()
                };
            }
            else
            {
                return null;
            }

            return result;
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token, out string errMsg)
        {
            errMsg = string.Empty;
            var tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = _signingKey,
                ValidAudience = _audience,
                ValidIssuer = _issuer,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = false,
                ClockSkew = TimeSpan.FromSeconds(0) // Identity and resource servers are the same.
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                errMsg = "Invalid token";

            return principal;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters
            {

                IssuerSigningKey = _signingKey,
                ValidAudience = _audience,
                ValidIssuer = _issuer,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.FromSeconds(0) // Identity and resource servers are the same.
            };
        }

        public static DateTime GetDateTimeNow()
        {
            return DateTime.UtcNow;
        }
    }
}
