﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGatewayServices.ExtensionMethods
{
    
    public interface IRead
    {
        string ReadFromAppsettings(string key);
    }
    public  class Config:IRead
    {
       private readonly IConfiguration _configuration;
        public Config(IConfiguration configuration)
        {
                _configuration = configuration;
        }
        public  string ReadFromAppsettings( string key)
        {
            var apiKey = _configuration.GetSection(key).Value;
            if (apiKey == null)
            {
                throw new Exception($"Appsetings Key:{key} is missing.");
            }

            return apiKey;
        }
    }
}
