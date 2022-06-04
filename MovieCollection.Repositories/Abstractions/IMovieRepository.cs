using Microsoft.EntityFrameworkCore;
using MovieCollection.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Repositories.Abstractions
{
    public interface IMovieRepository
    {
        DbSet<Movie> Movies { get; }
        DbSet<User> Users { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
    }
}
