using social_media.Entity;

namespace soicalMedia.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
