using MovieCollection.Repositories;
using MovieCollection.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Tests.Helpers
{
    internal class BaseTestHelper : IDisposable
    { 
        public IMovieRepository MovieRepository
        {
            get
            {
                if (_context == null)
                {
                    _dbName = Guid.NewGuid().ToString() + ".db";
                    _context = new MovieContext(_dbName);
                    _context.Database.EnsureDeleted();
                    _context.Database.EnsureCreated();
                }

                return _context;
            }
        }

        private MovieContext? _context;
        private string _dbName = string.Empty;
        private bool _disposedValue;

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing && _context != null)
                {
                    _context.Database.EnsureDeleted();
                    _context.Dispose();
                }

                _disposedValue = true;
            }
        }
    }
}
