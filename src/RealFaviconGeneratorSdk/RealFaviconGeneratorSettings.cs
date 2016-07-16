using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealFaviconGeneratorSdk
{
    public class RealFaviconGeneratorSettings
    {
        public RealFaviconGeneratorSettings(string apiKey)
        {
            ApiKey = apiKey;
        }

        public string ApiKey { get; }
    }
}
