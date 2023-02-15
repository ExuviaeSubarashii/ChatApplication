using Chat.Domain.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Text;
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
        [HttpPut]
        [Route("DeleteChannel")]
        public ActionResult DeleteChannel([FromBody] Servers servers)
        {
            string[] sww = null;
            List<string> sw = new List<string>();
            var checkifserverexists = _CP.Servers.Where(x => x.ServerName.Trim() == servers.ServerName.Trim()).FirstOrDefault();
            if (checkifserverexists != null)
            {
                var query = _CP.Servers.Where(x => x.Channels.Contains(servers.Channels)).FirstOrDefault();
                var query2 = _CP.Servers.Where(x => x.ServerName.Trim() == servers.ServerName.Trim() && x.Channels.Contains(servers.Channels.Trim())).ToList();
                foreach (var item in query2)
                {
                    sww = item.Channels.Split(',');
                }
                List<String> list = sww.ToList();
                list.Remove(servers.Channels.TrimEnd());
                string[] columns = list.ToArray();
                var newchannelss = string.Join(",", columns);
                query.Channels = newchannelss;
                _CP.SaveChanges();
            }
            return Ok();
        }
        [HttpPut]
        [Route("DeleteServer")]
        public ActionResult DeleteServer([FromBody] User user)
        {
            string[] sww = null;
            List<string> sw = new List<string>();
            var query = _CP.Users.Where(x => x.Username.Trim() == user.Username.Trim()).FirstOrDefault();
            var query2 = _CP.Users.Where(x => x.Username.Trim() == user.Username.Trim() && x.Server.Contains(user.Server)).ToList();
            foreach (var item in query2)
            {
                sww = item.Server.Split(',');
            }

            List<String> list = sww.ToList();
            list.Remove(user.Server);
            string[] columns = list.ToArray();
            var newservers = string.Join(",", columns);
            query.Server = newservers;
            _CP.SaveChanges();
            return Ok();
        }
        [HttpGet]
        [Route("GetAll/{username}")]
        public ActionResult GetAll(string username)
        {
            var query2 = _CP.Users.Where(x => x.Username.TrimEnd() == username.TrimEnd()).ToList();
            return Ok(query2);
        }
        [HttpGet]
        [Route("GetAllChannels/{servername}")]
        public ActionResult GetAllChannels(string servername)
        {
            var query = _CP.Servers.Where(x => x.ServerName.Trim() == servername.Trim()).ToList();
            return Ok(query);
        }
        [HttpPost]
        [Route("CreateUser")]
        public ActionResult CreateUser([FromBody] User user)
        {
            var query = _CP.Users.Where(x => x.Username == user.Username).Any();
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
        [Route("AddChannel")]
        public ActionResult AddChannel([FromBody] Servers servers)
        {
            var query3 = _CP.Servers.Any(x => x.ServerName.Trim() == servers.ServerName.Trim());
            var query4 = _CP.Servers.Where(x => x.ServerName.TrimEnd() == servers.ServerName.TrimEnd()).FirstOrDefault();
            if (query3)
            {
                query4.Channels = query4.Channels.TrimEnd() + ", " + servers.Channels.TrimEnd();
            }
            _CP.SaveChanges();
            return Ok();
        }
        [HttpPost]
        [Route("AddNewServer")]
        public ActionResult AddToServer([FromBody] Servers servers)
        {
            Servers servers1 = new Servers()
            {
                UserNames = servers.UserNames,
                Channels = servers.Channels,
                ServerName = servers.ServerName,
            };
            var query = _CP.Servers.Any(x => x.ServerName.TrimEnd() == servers.ServerName.TrimEnd());
            if (query)
            {
                return NotFound();
            }
            _CP.Servers.Add(servers1);
            _CP.SaveChanges();
            return Ok();
        }
        [HttpPut]
        [Route("AddServer")]
        public ActionResult AddServer([FromBody] User user)
        {
            string[] sww = null;
            var query = _CP.Users.Any(x => x.Username.TrimEnd() == user.Username.TrimEnd());
            var query2 = _CP.Users.Where(x => x.Username.TrimEnd() == user.Username.TrimEnd()).FirstOrDefault();
            if (query)
            {
                query2.Server = query2.Server.TrimEnd() + ", " + user.Server.TrimEnd();
            }
            _CP.SaveChanges();
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
