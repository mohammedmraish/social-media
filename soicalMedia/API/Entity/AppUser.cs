using Microsoft.AspNetCore.Identity;
using soicalMedia.Entity;
using soicalMedia.Extensions;
using System.ComponentModel.DataAnnotations;

namespace social_media.Entity
{
    public class AppUser:IdentityUser<int>
    {
        
        public string? KnownAs { get; set; }

        
        public DateTime DateOfBirth { get; set; }

        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? LastActive { get; set; } = DateTime.Now;

        public string? Gender { get; set; }

        public string? Introduction { get; set; }
        public string? LookingFor { get; set; }

        public string? Interests { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        public ICollection<Photo> Photos { get; set; }


        public ICollection<UserLike> LikedByUsers { get; set; }
        public ICollection<UserLike> LikedUsers { get; set; }


        //------------------messages
        public ICollection<Massage> MessagesSent { get; set; }
        public ICollection<Massage> MessagesReceived { get; set; }
        public int getAge()
        {
            return DateOfBirth.CalculateAge();
        }

        public ICollection<AppUserRole> UserRoles { get; set; }


    }
}
