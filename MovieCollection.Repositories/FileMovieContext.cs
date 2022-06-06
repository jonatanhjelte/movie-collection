using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieCollection.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Repositories
{
    public class FileMovieContext : MovieContext
    {
        public FileMovieContext(IConfiguration configuration) : base(configuration) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(Configuration.GetConnectionString("Database"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .Property(u => u.UserName)
                    .UseCollation("NOCASE");
        }
    }
}
