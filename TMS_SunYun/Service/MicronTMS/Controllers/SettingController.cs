using System.Web.Http;
using System.Collections.Generic;
using MicronTMS.BL.Implementation;
using MicronTMS.DLL.Models;

namespace MicronTMS.Controllers
{
    [RoutePrefix("api/Setting")]
    public class SettingController : ApiController
    {
        private readonly SettingBL _settingBL;

        public SettingController()
        {
            _settingBL = new SettingBL();
        }

        [HttpGet]
        [Route("GetRealTimeStatusAndOffset")]
        public IHttpActionResult GetRealTimeStatusAndOffset()
        {
            return Ok(_settingBL.GetRealTimeStatusAndOffset());
        }

        [HttpPost]
        [Route("UpdateStatusAndOffset")]
        public IHttpActionResult UpdateStatusAndOffset(List<StatusOffsetModel> statusOffsetList)
        {
            return Ok(_settingBL.UpdateStatusAndOffset(statusOffsetList));
        }
    }
}