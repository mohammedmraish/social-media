using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using soicalMedia.Data;
using soicalMedia.DTOs;
using soicalMedia.Entity;
using soicalMedia.Extensions;
using soicalMedia.Helpers;
using soicalMedia.Interfaces;
using System.Security.Claims;

namespace soicalMedia.Controllers
{
    
    [Authorize]

    public class MassagesController : BaseApiController
    {
        private readonly IMessageRepository massageRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public MassagesController(IMessageRepository massageRepository,IUserRepository userRepository,IMapper mapper)
        {
            this.massageRepository = massageRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult<MassageDto>> CreateMassage(CreatMassageDto creatMassageDto)
        {
            var logedUsername = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (logedUsername == creatMassageDto.RecipientUsername.ToLower())
                return BadRequest("you cannot send message to yourself");

            var sender = await userRepository.GetUserByUsernameAsync(logedUsername);
            var recipient =await userRepository.GetUserByUsernameAsync(creatMassageDto.RecipientUsername);

            if(recipient==null)
                return NotFound();


            var massage = new Massage
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserme = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = creatMassageDto.Content,
            };

            massageRepository.AddMessage(massage);

            if (await massageRepository.SaveAllAsync()) return Ok(mapper.Map<MassageDto>(massage));

            return BadRequest("Faild");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MassageDto>>> GetMassagesForUser([FromQuery] MassageParams massageParams) 
        {
            massageParams.Username =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var massages = await massageRepository.GetMassagesForUser(massageParams);

            Response.AddPaginationHeader(massages.CurrentPage, massages.PageSize, massages.TotalCount, massages.TotalPages);

            return massages;
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MassageDto>>> GetMassageThred(string username)
        {
            var currentUsername = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var massages = await massageRepository.GetMassageThread(currentUsername, username);

            return Ok(massages);

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var message = await massageRepository.GetMassage(id);

            if (message.Sender.UserName != username && message.Recipient.UserName != username)
                return Unauthorized();

            if (message.Sender.UserName == username) message.SenderDeleted = true;

            if (message.Recipient.UserName == username) message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
                  massageRepository.DeleteMeassage(message);

            if (await massageRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem deleting the message");
        }
    }
}
