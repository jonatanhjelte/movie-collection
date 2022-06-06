using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
            var inMemorySettings = new Dictionary<string, string> {
                {"ConnectionStrings:Database", $"Data Source={Guid.NewGuid().ToString() + ".db"}"},
                };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            Context = new FileMovieContext(configuration);
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
