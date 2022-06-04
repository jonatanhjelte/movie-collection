using Microsoft.EntityFrameworkCore;
using MovieCollection.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Repositories
{
    public class MovieContext : DbContext
    {
        public DbSet<Movie> Movies => Set<Movie>();
    }
}
