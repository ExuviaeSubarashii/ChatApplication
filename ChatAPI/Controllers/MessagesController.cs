using Chat.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ChatContext _CP;
        public MessagesController(ChatContext CP) 
        {
            _CP= CP;
        }
        [HttpPost]
        [Route("GetAll")]
        public ActionResult GetAll([FromBody] Message message)
        {
            var query= _CP.Messages.Where(x => x.Server==message.Server&&x.Channel==message.Channel && x.ImageDir != "Null").ToList();
            return Ok(query.ToList());
        }
        [HttpPost]
        [Route("SendMessage")]
        public ActionResult SendMessage([FromBody] Message msg)
        {
            Message newmsg=new Message()
            {
                Message1=msg.Message1,
                SenderName=msg.SenderName,
                SenderTime=msg.SenderTime,
                Server=msg.Server,
                Channel=msg.Channel,
                ImageDir=msg.ImageDir,
            };
            _CP.Messages.Add(newmsg);
            _CP.SaveChanges();
            return Ok();
        }
    }
}
