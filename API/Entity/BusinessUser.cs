using Microsoft.AspNetCore.Identity;

namespace API.Entity
{
    public class BusinessUser : IdentityUser<int>
    {
        public DateTime DateOfBirth { get; set; } 
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string Gender { get; set; }
        public string KnownAs { get; set; }
        public string Interests { get; set; }
        public string Introduction { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Position { get; set; }

        public List<Photo> Photos { get; set; } = new();

        public List<UserLike> LikedByUsers { get; set; }
        public List<UserLike> LikedUsers { get; set; }

        public List<Message> MessageSent { get; set; } 
        public List<Message> MessageReceived { get; set; }

        public ICollection<BusinessUserRole> UserRoles { get; set; }
    }
}   