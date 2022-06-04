using MovieCollection.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Repositories.Models
{
    public record MovieModel
    {
        public int Id { get; set; }
        public string MovieDatabaseId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public Movie ToDomainMovie()
        {
            return new Movie()
            {
                MovieDatabaseId = MovieDatabaseId,
                Name = Name,
            };
        }
    }
}
