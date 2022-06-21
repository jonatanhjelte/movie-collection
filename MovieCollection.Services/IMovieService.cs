using MovieCollection.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> FindMoviesByNameAsync(string name);
    }
}
