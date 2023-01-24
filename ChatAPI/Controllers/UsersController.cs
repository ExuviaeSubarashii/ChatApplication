using Chat.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ChatContext _CP;
        public UsersController(ChatContext CP) 
        {
            _CP = CP;
        }
        [HttpPost]
        [Route("CreateUser")]
        public ActionResult CreateUser([FromBody] User user)
        {
            var query=_CP.Users.Where(x=>x.Username==user.Username).Any();
            User newUser = new User()
            {
                Username= user.Username,
                Password=user.Password,
                HasPassword=user.HasPassword
            };
            if (query == true)
            {
                return StatusCode(500);
            }
            else if (user.Username == null || user.Username == "" || string.IsNullOrEmpty(user.Username))
            {
                return StatusCode(406);
            }
            else
            {
                _CP.Users.Add(newUser);
                _CP.SaveChanges();
                return Ok();
            }
        }
        [HttpPost]
        [Route("Login")]
        public ActionResult Login([FromBody] User user)
        {
            if (user.Username != null && user.Password != null)
            {
                var query = _CP.Users.Any(x => x.Username.TrimEnd() == user.Username.TrimEnd() & x.Password.TrimEnd() == user.Password.TrimEnd());

                if (query)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            return Ok();
        }
    }
}
