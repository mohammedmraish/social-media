using social_media.Entity;
using soicalMedia.DTOs;
using soicalMedia.Entity;
using soicalMedia.Helpers;

namespace soicalMedia.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetuserLike(int sourceUserId, int likedUserId);
        Task<AppUser> GetUserWithLikes(int userId);

        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    }
}
