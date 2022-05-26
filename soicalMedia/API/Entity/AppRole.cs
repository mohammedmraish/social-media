using Microsoft.AspNetCore.Identity;

namespace soicalMedia.Entity
{
    public class AppRole:IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
