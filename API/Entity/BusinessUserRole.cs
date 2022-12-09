using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace API.Entity
{
    public class BusinessUserRole : IdentityUserRole<int>
    {
        public BusinessUser User { get; set; }
        public BusinessRole Role { get; set; }
    }
}