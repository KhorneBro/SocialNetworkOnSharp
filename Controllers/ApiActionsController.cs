using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialNetworkOnSharp.Models;
using SocialNetworkOnSharp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SocialNetworkOnSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiActionsController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService userService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ApplicationContext applicationContext;

        public ApiActionsController(
               ILogger<HomeController> logger,
               UserService userService,
               IWebHostEnvironment webHostEnvironment,
               ApplicationContext applicationContext
               )
        {
            _logger = logger;
            this.userService = userService;
            this.webHostEnvironment = webHostEnvironment;
            this.applicationContext = applicationContext;
        }

        [HttpPost("addFriend/{id}")]
        public async Task<ActionResult> MakeRequestToAddUserInAFriendList(int? id)
        {
            if (id is null)
                return BadRequest("Id не может быть null");


            Participant principalUser = await userService.FindByLoginAsync(User.Identity.Name.ToString());
            Participant friendParticipant = await applicationContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            Friend friend = await applicationContext.Friends.FirstOrDefaultAsync(f => f.ParticipantId == friendParticipant.Id);

            if (principalUser == null || friendParticipant == null || friendParticipant.IsUserDeleted is true)
                return NotFound("Пользователя не существует");
            if (friend is not null)
            {
                if (principalUser.FriendList.Contains(friend))
                    return BadRequest("пользователь уже в списке");
                else
                {
                    principalUser.FriendList.Add(friend);
                    friendParticipant.FriendList.Add(new Friend { ParticipantId = principalUser.Id, FriendStatus = FriendStatus.RequestToMe, NickName = principalUser.NickName, Avatar = principalUser.Avatar });
                    await applicationContext.SaveChangesAsync();
                    return Ok();
                }
            }
            else
            {
                principalUser.FriendList.Add(new Friend { FriendStatus = FriendStatus.RequestFromMe, NickName = friendParticipant.NickName, Avatar = friendParticipant.Avatar });
                friendParticipant.FriendList.Add(new Friend { FriendStatus = FriendStatus.RequestToMe, NickName = principalUser.NickName, Avatar = principalUser.Avatar });
                await applicationContext.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpDelete("deleteFriend/{id}")]
        public async Task<ActionResult> DeleteFriendFromFriendList(int? id)
        {
            if (id is null)
                return BadRequest();


            Participant principalUser = await userService.FindByLoginAsync(User.Identity.Name.ToString());
            Participant friendParticipant = await applicationContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (principalUser == null || friendParticipant == null)
                return NotFound();


            Friend friend = await applicationContext.Friends.FirstOrDefaultAsync(f => f.ParticipantId == friendParticipant.Id);
            Friend user = await applicationContext.Friends.FirstOrDefaultAsync(f => f.ParticipantId == principalUser.Id);

            if (!principalUser.FriendList.Contains(friend) || !friendParticipant.FriendList.Contains(user))
                return BadRequest();


            principalUser.FriendList.Remove(friend);
            friendParticipant.FriendList.Remove(user);
            await applicationContext.SaveChangesAsync();
            return NoContent();
        }

        //отметить собственный запрос
        [HttpPost("cancelRequest/{id}")]
        public async Task<ActionResult> CancelRequestFroAddingFriend(int? id)
        {
            if (id is null)
                return BadRequest();


            Participant principalUser = await userService.FindByLoginAsync(User.Identity.Name.ToString());
            Participant friendParticipant = await applicationContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (principalUser == null || friendParticipant == null)
                return NotFound();


            Friend friend = await applicationContext.Friends.FirstOrDefaultAsync(f => f.ParticipantId == friendParticipant.Id);
            Friend user = await applicationContext.Friends.FirstOrDefaultAsync(f => f.ParticipantId == principalUser.Id);

            if (!principalUser.FriendList.Contains(friend) || !friendParticipant.FriendList.Contains(user))
                return BadRequest();


            principalUser.FriendList.Remove(friend);
            friendParticipant.FriendList.Remove(user);
            await applicationContext.SaveChangesAsync();
            return Ok();
        }

        //отменить чужой запрос
        [HttpPost("declineRequest/{id}")]
        public async Task<ActionResult> DeclineRequestFroAddingFriend(int? id)
        {
            if (id is null)
                return BadRequest();


            Participant principalUser = await userService.FindByLoginAsync(User.Identity.Name.ToString());
            Participant friendParticipant = await applicationContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (principalUser == null || friendParticipant == null)
                return NotFound();


            Friend friend = await applicationContext.Friends.FirstOrDefaultAsync(f => f.ParticipantId == friendParticipant.Id);
            Friend user = await applicationContext.Friends.FirstOrDefaultAsync(f => f.ParticipantId == principalUser.Id);

            if (!principalUser.FriendList.Contains(friend) || !friendParticipant.FriendList.Contains(user))
                return BadRequest();


            principalUser.FriendList.Remove(friend);
            friendParticipant.FriendList.Remove(user);
            await applicationContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("acceptRequest/{id}")]
        public async Task<ActionResult> AcceptRequestFroAddingFriend(int? id)
        {
            if (id is null)
                return BadRequest();

            Participant principalUser = await userService.FindByLoginAsync(User.Identity.Name.ToString());
            Participant friendParticipant = await applicationContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (principalUser == null || friendParticipant == null)
                return NotFound();

            Friend friend = await applicationContext.Friends.FirstOrDefaultAsync(f => f.ParticipantId == friendParticipant.Id);
            Friend user = await applicationContext.Friends.FirstOrDefaultAsync(f => f.ParticipantId == principalUser.Id);

            if (!principalUser.FriendList.Contains(friend) || user.FriendStatus != FriendStatus.RequestFromMe ||
                !friendParticipant.FriendList.Contains(user) || friend.FriendStatus != FriendStatus.RequestToMe)
                return BadRequest();


            user.FriendStatus = FriendStatus.FriendList;
            friend.FriendStatus = FriendStatus.FriendList;
            await applicationContext.SaveChangesAsync();
            return Ok();
        }

        // GET: api/<ApiActionsController> 
        [HttpGet("add/{id}")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //// GET api/<ApiActionsController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<ApiActionsController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<ApiActionsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ApiActionsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
