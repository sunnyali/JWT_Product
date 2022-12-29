using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helper
{
    public class Properties
    {
        #region AppSetting Properties
        public string JWTSecret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AuthTokenTimeOut { get; set; } 
        #endregion

        public Properties()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();

            var JWTConfig = root.GetSection("JWTConfig");
            {
                JWTSecret = JWTConfig.GetSection("Secret").Value;
                Issuer = JWTConfig.GetSection("Issuer").Value;
                Audience = JWTConfig.GetSection("Audience").Value;
                AuthTokenTimeOut = Convert.ToInt32(JWTConfig.GetSection("AuthTokenTimeOut")?.Value ?? "0");
            };


        }
    }
}
