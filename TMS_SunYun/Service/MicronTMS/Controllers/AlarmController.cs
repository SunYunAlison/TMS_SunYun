using System.Web.Http;
using MicronTMS.BL.Implementation;
using MicronTMS.DLL.Entities;

namespace MicronTMS.Controllers
{
    [RoutePrefix("api/Alarm")]
    public class AlarmController : ApiController
    {
        private readonly AlarmBL _alarmBL;

        public AlarmController()
        {
            _alarmBL = new AlarmBL();
        }

        [HttpGet]
        [Route("GetAlarmList")]
        public IHttpActionResult GetAlarmList()
        {
            return Ok(_alarmBL.GetAlarmList());
        }

        [HttpPost]
        [Route("UpdateAlarmAction")]
        public IHttpActionResult UpdateAlarmAction(RTmsAlert alermComment)
        {
            return Ok(_alarmBL.UpdateAlarmAction(alermComment));
        }
    }
}