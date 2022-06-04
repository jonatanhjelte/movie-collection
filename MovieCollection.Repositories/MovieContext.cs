using Microsoft.EntityFrameworkCore;
using MovieCollection.Domain;
using MovieCollection.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Repositories
{
    public class MovieContext : DbContext, IMovieRepository
    {
        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<User> Users => Set<User>();

        public string DbPath { get; private set; } = string.Empty;

        private readonly string _dbName = "movies.db";

        public MovieContext()
        {
            var savePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            DbPath = Path.Join(savePath, _dbName);
        }

        public MovieContext(string dbName)
        {
            _dbName = dbName;
            var savePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            DbPath = Path.Join(savePath, _dbName);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasKey(m => m.MovieDatabaseId);
        }
    }
}
