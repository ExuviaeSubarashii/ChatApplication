using Chat.Domain.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
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
        [HttpGet]
        [Route("GetAll/{username}")]
        public ActionResult GetAll(string username)
        {
            var query2 = _CP.Users.Where(x => x.Username.TrimEnd() == username.TrimEnd()).ToList();
            return Ok(query2);
        }
        [HttpPost]
        [Route("CreateUser")]
        public ActionResult CreateUser([FromBody] User user)
        {
            var query=_CP.Users.Where(x=>x.Username==user.Username).Any();
            User newUser = new User()
            {
                Username = user.Username,
                Password = user.Password,
                HasPassword = user.HasPassword,
                Image = user.Image,
                Server = user.Server
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
        [HttpPut]
        [Route("AddServer")]
        public ActionResult AddServer([FromBody] User user)
        {
            var query = _CP.Users.Any(x => x.Username.TrimEnd() == user.Username.TrimEnd());
            var query2=_CP.Users.Where(x=>x.Username.TrimEnd()==user.Username.TrimEnd()).FirstOrDefault();
            if (query)
            {
                query2.Server = query2.Server.TrimEnd() + ", " + user.Server.TrimEnd();
                //_CP.Users.Add(newUser);
                _CP.SaveChanges();
            }
            
            return Ok();
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
