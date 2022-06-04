﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Domain
{
    public record User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public IList<Movie> Movies { get; set; } = new List<Movie>();
    }
}