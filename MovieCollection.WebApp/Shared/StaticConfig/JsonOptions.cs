using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieCollection.WebApp.Shared.StaticConfig
{
    public static class JsonOptions
    {
        public static JsonSerializerOptions Default => new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        };
    }
}
