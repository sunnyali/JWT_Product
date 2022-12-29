﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AppConfiguration
    {
        public readonly string _connectionString = string.Empty;

        public AppConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();
            _connectionString = root.GetSection("ConnectionStrings").GetSection("DBConnectionString").Value;
            var appSetting = root.GetSection("ApplicationSettings");
        }

        public string ConnectionString
        {
            get => _connectionString;
        }
    }
}
