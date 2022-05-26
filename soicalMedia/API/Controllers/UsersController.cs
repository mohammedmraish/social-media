using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using social_media.Data;
using social_media.Entity;
using soicalMedia.Data;
using soicalMedia.DTOs;
using soicalMedia.Extensions;
using soicalMedia.Helpers;
using soicalMedia.Interfaces;
using System.Security.Claims;

namespace soicalMedia.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository userRepositry;
    private readonly IMapper mapper;
    private readonly IPhotoService photoService;

    public UsersController(IUserRepository userRepositry, IMapper mapper, IPhotoService photoService)
    {
        this.userRepositry = userRepositry;
        this.mapper = mapper;
        this.photoService = photoService;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDtos>>> GetUsers([FromQuery] UserParams userParams)
    {
        var users = await userRepositry.GetMembersAsync(userParams);

        Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

        return Ok(users);
    }

    [Authorize(Roles = "Member")]
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDtos>> GetUser(string username)
    {
        var user = await userRepositry.GetMemberByUsernameAsync(username);

        return Ok(user);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await userRepositry.GetUserByUsernameAsync(username);

        mapper.Map(memberUpdateDto, user);
        userRepositry.Update(user);

        if (await userRepositry.SaveAllAsync())
            return NoContent();
        else
            return BadRequest("failed to update user");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await userRepositry.GetUserByUsernameAsync(username);

        var result = await photoService.AddPhotoAsync(file);
        if (result.Error != null)
            return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
        };

        if (user.Photos.Count == 0)
        {
            photo.IsMain = true;
        }

        user.Photos.Add(photo);

        if (await userRepositry.SaveAllAsync())
            return mapper.Map<PhotoDto>(photo);

        return BadRequest("problem adding photo");
    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await userRepositry.GetUserByUsernameAsync(username);

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if (photo.IsMain) return BadRequest("this id already your main photo");

        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if(currentMain!=null)currentMain.IsMain = false;    

        photo.IsMain = true;    

        if(await userRepositry.SaveAllAsync())return NoContent();

        return BadRequest("Failed to set main photo");

    }

    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await userRepositry.GetUserByUsernameAsync(username);

        var photo=user.Photos.FirstOrDefault(x=>x.Id==photoId);

        if(photo ==null)return NotFound();

        if (photo.IsMain) return BadRequest("you cannot delete your main photo");

        if (photo.PublicId != null)
        {
           var result= await photoService.DeletePhotoAsync(photo.PublicId);
            if(result.Error!=null)return BadRequest(result.Error.Message);
        }

        user.Photos.Remove(photo);

        if (await userRepositry.SaveAllAsync())
            return Ok();

        return BadRequest("Failed to delete photo");
    }





}

