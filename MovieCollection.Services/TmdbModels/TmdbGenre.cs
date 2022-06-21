using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Services.TmdbModels
{
    public record TmdbGenre
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
    }
}
