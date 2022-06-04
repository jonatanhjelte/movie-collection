using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Domain
{
    public record Movie
    {
        public string MovieDatabaseId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
