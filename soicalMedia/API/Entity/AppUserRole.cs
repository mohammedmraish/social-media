using Microsoft.AspNetCore.Identity;
using social_media.Entity;

namespace soicalMedia.Entity
{
    public class AppUserRole:IdentityUserRole<int>
    {
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
    }
}
