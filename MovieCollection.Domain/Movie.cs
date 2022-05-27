using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Domain
{
    public record Movie : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
    }
}
