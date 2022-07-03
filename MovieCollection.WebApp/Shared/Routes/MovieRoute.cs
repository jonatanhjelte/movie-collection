using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.WebApp.Shared.Routes
{
    public static class MovieRoute
    {
        public static readonly string CurrentUser = "users/current";
        public static readonly string LoginUser = "users/login";
        public static readonly string LogoutUser = "users/logout";
        public static readonly string CreateUser = "users/create";
        public static readonly string FindMovie = "movies/find";
    }
}
