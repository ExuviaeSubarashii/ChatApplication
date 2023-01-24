using Chat.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [Route("SendMessage")]
        public ActionResult SendMessage([FromBody] Message msg)
        {
            Message newmsg=new Message()
            {
                Message1=msg.Message1,
                SenderName=msg.SenderName,
                SenderTime=msg.SenderTime,
            };
            _CP.Messages.Add(newmsg);
            _CP.SaveChanges();
            return Ok();
        }
    }
}
