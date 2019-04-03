using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace TourManagement.API.Authorization
{
    public class UserMustBeTourManagerRequirement : IAuthorizationRequirement
    {
        public string Role { get; }

        public UserMustBeTourManagerRequirement(string role)
        {
            Role = role;
        }
    }
}
