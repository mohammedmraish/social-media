using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using social_media.Data;
using social_media.Entity;

namespace soicalMedia.Controllers
{
   
    public class BuggyController : BaseApiController
    {
        private readonly DataContext context;

        public BuggyController(DataContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret() 
        {
            return "sectret text";
        }



        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = context.Users.Find(-1);

            if(thing==null)return NotFound();

            return BadRequest(thing);
        }



        [HttpGet("server-error")]
        public ActionResult<string> GetSserverError()
        {
            var thing = context.Users.Find(-1);

            var thingToReturn=thing.ToString();

            return thingToReturn;
        }



        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("this was not a good request");
        }


    }
}
