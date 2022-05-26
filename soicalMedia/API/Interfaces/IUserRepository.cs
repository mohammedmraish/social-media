using social_media.Entity;
using soicalMedia.DTOs;
using soicalMedia.Helpers;

namespace soicalMedia.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);

        Task<bool> SaveAllAsync();

        Task<IEnumerable<AppUser>> GetUsersAsync();

        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);

        Task<PagedList<MemberDtos>> GetMembersAsync(UserParams userParams);

        Task<MemberDtos> GetMemberByUsernameAsync(string username);


    }
}
