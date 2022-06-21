using MovieCollection.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Services.TmdbModels
{
    public record TmdbMovie
    {
        public int id { get; set; }
        public bool adult { get; set; }
        public string? backdrop_path { get; set; }
        public string? poster_path { get; set; }
        public TmdbCollection? belongs_to_collection { get; set; }
        public int budget { get; set; }
        public IEnumerable<TmdbGenre> genres { get; set; } = new List<TmdbGenre>();
        public string? homepage { get; set; }
        public string? imdb_id { get; set; }
        public string original_language { get; set; } = string.Empty;
        public string original_title { get; set; } = string.Empty;
        public string? overview { get; set; }
        public decimal popularity { get; set; }
        public DateTime release_date { get; set; }
        public int revenue { get; set; }
        public int? runtime { get; set; }
        public TmdbStatus status { get; set; }
        public string? tagline { get; set; }
        public string title { get; set; } = string.Empty;
        public bool video { get; set; }
        public decimal vote_average { get; set; }
        public int vote_count { get; set; }

        public Movie ToMovie()
        {
            return new Movie()
            {
                MovieDatabaseId = imdb_id ?? "tt0000000",
                Name = title,
            };
        }
    }
}
