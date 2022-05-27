using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Domain
{
    public record BaseEntity
    {
        public int Id { get; set; }
    }
}
