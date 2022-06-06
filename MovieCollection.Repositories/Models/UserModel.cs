using Microsoft.EntityFrameworkCore.Metadata;
using MovieCollection.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Repositories.Models
{
    public record UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public ICollection<MovieModel> Movies { get; set; } = new List<MovieModel>();

        public User ToDomainUser()
        {
            return new User()
            {
                UserName = UserName,
                Email = Email,
            };
        }
    }
}
