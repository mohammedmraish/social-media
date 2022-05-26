using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using soicalMedia.DTOs;
using soicalMedia.Entity;
using soicalMedia.Extensions;
using soicalMedia.Helpers;
using soicalMedia.Interfaces;
using System.Security.Claims;

namespace soicalMedia.Controllers;

  [Authorize]
public class LikesController : BaseApiController
{
    private readonly IUserRepository userRepository;
    private readonly ILikesRepository likesRepository;

    public LikesController(IUserRepository userRepository, ILikesRepository likesRepository) {
        this.userRepository = userRepository;
        this.likesRepository = likesRepository;
    }


    [HttpPost("{username}")]
    public async Task<ActionResult> AddLike(string username)
    {
        var logedUsername= User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        MemberDtos sourceeUser = await userRepository.GetMemberByUsernameAsync(logedUsername);

        var likedUser = await userRepository.GetUserByUsernameAsync(username);
        var sourceUser = await likesRepository.GetUserWithLikes(sourceeUser.Id);

        if (likedUser == null) return NotFound();

        if (sourceUser.UserName == username) return BadRequest("you cannot like yourself");

        var userLike = await likesRepository.GetuserLike(sourceeUser.Id, likedUser.Id);
        if (userLike != null) return BadRequest("you already like this user");

        userLike = new UserLike
        {
            SourceUserId = sourceeUser.Id,
            LikedUserId = likedUser.Id,
        };

        sourceUser.LikedUsers.Add(userLike);

        if (await userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Failed to like user");
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserlikes([FromQuery]LikesParams likesParams)
    {
        var logedUsername = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var logedUser =await userRepository.GetMemberByUsernameAsync(logedUsername);
       
        likesParams.UserId=logedUser.Id;
        var users = await likesRepository.GetUserLikes(likesParams);

        Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);


        return Ok( users);
    }



}

