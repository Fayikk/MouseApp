using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entity
{
    public class UserLike
    {
        public BusinessUser SourceUser { get; set; }
        public int SourceUserId { get; set; }
        public BusinessUser TargetUser { get; set; }
        public int TargetUserId { get; set; }
    }
}