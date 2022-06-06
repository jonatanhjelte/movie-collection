using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.WebApp.Shared.Requests
{
    public record CreateUserRequest
    {
        [Required]
        public string UserName { get; init; } = string.Empty;
        [Required]
        public string Password { get; init; } = string.Empty;
        [Required]
        public string Email { get; init; } = string.Empty;
    }
}
