using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Services.TmdbModels
{
    internal record TmdbCollection
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string? poster_path { get; set; }
        public string? backdrop_path { get; set; }
    }
}
