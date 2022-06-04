using Microsoft.EntityFrameworkCore;
using MovieCollection.Domain;
using MovieCollection.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Repositories
{
    public class MovieContext : DbContext
    {
        public DbSet<MovieModel> Movies { get; set; } = null!;
        public DbSet<UserModel> Users { get; set; } = null!;

        public string DbPath { get; private set; } = string.Empty;

        private readonly string _dbName;

        public MovieContext()
            :this("movies.db")
        {
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
            modelBuilder.Entity<UserModel>()
                .Property(u => u.UserName)
                    .UseCollation("NOCASE");
        }
    }
}
