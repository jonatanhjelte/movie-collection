using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Domain
{
    public record User
    {
        public string UserName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
    }
}
