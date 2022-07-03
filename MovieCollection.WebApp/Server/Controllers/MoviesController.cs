using Microsoft.AspNetCore.Mvc;
using MovieCollection.Domain;
using MovieCollection.Services;
using System.ComponentModel.DataAnnotations;

namespace MovieCollection.WebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("find")]
        public async Task<ActionResult<IEnumerable<Movie>>> Find([Required][MinLength(4)] string name)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var movies = await _movieService.FindMoviesByNameAsync(name);

            return Ok(movies);
        }
    }
}
