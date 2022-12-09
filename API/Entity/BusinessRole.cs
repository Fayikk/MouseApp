using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace API.Entity
{
    public class BusinessRole : IdentityRole<int>
    {
        public ICollection<BusinessUserRole> UserRoles { get; set; }        
    }
}