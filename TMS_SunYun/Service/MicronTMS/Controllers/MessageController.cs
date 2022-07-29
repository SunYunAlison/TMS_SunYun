using System.Web.Http;
using MicronTMS.BL.Implementation;
using MicronTMS.DLL.Models;


namespace MicronTMS.Controllers
{
    [RoutePrefix("api/Message")]
    public class MessageController : ApiController
    {
        private readonly MessageBL _messageBL;

        public MessageController()
        {
            _messageBL = new MessageBL();
        }

        [HttpGet]
        [Route("GetCMCCurrentVersion")]
        public IHttpActionResult GetCMCCurrentVersion()
        {
            var result = _messageBL.GetCMCCurrentVersion();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetMessageRecords")]
        public IHttpActionResult GetMessageRecords()
        {
            var result = _messageBL.GetMessageRecords();
            return Ok(result);
        }

        [HttpPost]
        [Route("sendMessages")]
        public IHttpActionResult sendMessages(SendWebAPIModel messageData)
        {
            return Ok(_messageBL.sendMessages(messageData));
        }
    }
}