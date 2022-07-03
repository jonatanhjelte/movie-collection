using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Services.TmdbModels
{
    public record TmdbMovieResult
    {
        public int page { get; set; }
        public List<TmdbMovie> results { get; set; } = new List<TmdbMovie> ();
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }
}
