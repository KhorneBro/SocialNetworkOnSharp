using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialNetworkOnSharp.Models;
using SocialNetworkOnSharp.Services;


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
            {
                return BadRequest();
            }

            Participant principalUser = await userService.FindByLoginAsync(User.Identity.Name.ToString());
            Participant friend = await applicationContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (principalUser == null || friend == null)
            {
                return NotFound();
            }
            if (principalUser.FriendRequestFromMe.Contains(await applicationContext.Friends.FirstOrDefaultAsync(f => f.FriendsId == friend.Id)) ||
                friend.FriendRequestToMe.Contains(await applicationContext.Friends.FirstOrDefaultAsync(f => f.FriendsId == principalUser.Id)) ||
                principalUser.FriendList.Contains(await applicationContext.Friends.FirstOrDefaultAsync(f => f.FriendsId == friend.Id)) ||
                friend.FriendList.Contains(await applicationContext.Friends.FirstOrDefaultAsync(f => f.FriendsId == principalUser.Id)))
            {
                return BadRequest();
            }

            principalUser.FriendRequestFromMe.Add(new Friend { FriendsId = friend.Id });
            friend.FriendRequestToMe.Add(new Friend { FriendsId = principalUser.Id });
            await applicationContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("deleteFriend/{id}")]
        public async Task<ActionResult> DeleteFriendFromFriendList(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            Participant principalUser = await userService.FindByLoginAsync(User.Identity.Name.ToString());
            Participant friend = await applicationContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (principalUser == null || friend == null)
            {
                return NotFound();
            }
            if (!principalUser.FriendList.Contains(await applicationContext.Friends.FirstOrDefaultAsync(f => f.FriendsId == friend.Id)) ||
               !friend.FriendList.Contains(await applicationContext.Friends.FirstOrDefaultAsync(f => f.FriendsId == principalUser.Id)))
            {
                return BadRequest();
            }

            principalUser.FriendList.Remove(await applicationContext.Friends.FirstOrDefaultAsync(u => u.FriendsId == friend.Id));
            friend.FriendList.Remove(await applicationContext.Friends.FirstOrDefaultAsync(u => u.FriendsId == principalUser.Id));
            await applicationContext.SaveChangesAsync();
            return NoContent();
        }

        //отметить собственный запрос
        [HttpPost("cancelRequest/{id}")]
        public async Task<ActionResult> CancelRequestFroAddingFriend(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            Participant principalUser = await userService.FindByLoginAsync(User.Identity.Name.ToString());
            Participant friend = await applicationContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (principalUser == null || friend == null)
            {
                return NotFound();
            }
            if (!principalUser.FriendRequestFromMe.Contains(await applicationContext.Friends.FirstOrDefaultAsync(f => f.FriendsId == friend.Id)) ||
                !friend.FriendRequestToMe.Contains(await applicationContext.Friends.FirstOrDefaultAsync(f => f.FriendsId == principalUser.Id)))
            {
                return BadRequest();
            }

            principalUser.FriendRequestFromMe.Remove(await applicationContext.Friends.FirstOrDefaultAsync(u => u.FriendsId == friend.Id));
            friend.FriendRequestToMe.Remove(await applicationContext.Friends.FirstOrDefaultAsync(u => u.FriendsId == principalUser.Id));
            await applicationContext.SaveChangesAsync();
            return Ok();
        }

        //отменить чужой запрос
        [HttpPost("declineRequest/{id}")]
        public async Task<ActionResult> DeclineRequestFroAddingFriend(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            Participant principalUser = await userService.FindByLoginAsync(User.Identity.Name.ToString());
            Participant friend = await applicationContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (principalUser == null || friend == null)
            {
                return NotFound();
            }
            if (!principalUser.FriendRequestToMe.Contains(await applicationContext.Friends.FirstOrDefaultAsync(f => f.FriendsId == friend.Id)) ||
                !friend.FriendRequestFromMe.Contains(await applicationContext.Friends.FirstOrDefaultAsync(f => f.FriendsId == principalUser.Id)))
            {
                return BadRequest();
            }

            principalUser.FriendRequestToMe.Remove(await applicationContext.Friends.FirstOrDefaultAsync(u => u.FriendsId == friend.Id));
            friend.FriendRequestFromMe.Remove(await applicationContext.Friends.FirstOrDefaultAsync(u => u.FriendsId == principalUser.Id));
            await applicationContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("acceptRequest/{id}")]
        public async Task<ActionResult> AcceptRequestFroAddingFriend(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            Participant principalUser = await userService.FindByLoginAsync(User.Identity.Name.ToString());
            Participant friend = await applicationContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (principalUser == null || friend == null)
            {
                return NotFound();
            }
            if (!principalUser.FriendList.Contains(await applicationContext.Friends.FirstOrDefaultAsync(f => f.FriendsId == friend.Id)) ||
                principalUser.FriendRequestFromMe.Contains(await applicationContext.Friends.FirstOrDefaultAsync(f => f.FriendsId == friend.Id)) ||
                !friend.FriendList.Contains(await applicationContext.Friends.FirstOrDefaultAsync(f => f.FriendsId == principalUser.Id)) ||
                friend.FriendRequestToMe.Contains(await applicationContext.Friends.FirstOrDefaultAsync(f => f.FriendsId == principalUser.Id)))
            {
                return BadRequest();
            }

            principalUser.FriendRequestFromMe.Remove(await applicationContext.Friends.FirstOrDefaultAsync(u => u.FriendsId == friend.Id));
            friend.FriendRequestToMe.Remove(await applicationContext.Friends.FirstOrDefaultAsync(u => u.FriendsId == principalUser.Id));
            principalUser.FriendList.Add(new Friend { FriendsId = friend.Id });
            friend.FriendList.Add(new Friend { FriendsId = principalUser.Id });
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
