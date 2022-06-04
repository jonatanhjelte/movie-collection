using MovieCollection.Domain;
using MovieCollection.Services;
using MovieCollection.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Tests.Helpers
{
    internal class UserServiceTestHelper : BaseTestHelper
    {
        public readonly UserService UserService;

        public UserServiceTestHelper()
            :base()
        {
            UserService = new UserService(Context);
        }
    }
}
