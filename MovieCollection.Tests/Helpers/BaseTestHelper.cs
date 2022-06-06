using Microsoft.EntityFrameworkCore;
using MovieCollection.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Tests.Helpers
{
    internal class BaseTestHelper : IDisposable
    { 
        public MovieContext Context { get; private set; }

        private bool _disposedValue;

        public BaseTestHelper()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MovieContext>();
            optionsBuilder.UseSqlite($"Data Source={Guid.NewGuid().ToString() + ".db"}");
            Context = new MovieContext(optionsBuilder.Options);
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing && Context != null)
                {
                    Context.Database.EnsureDeleted();
                    Context.Dispose();
                }

                _disposedValue = true;
            }
        }
    }
}
