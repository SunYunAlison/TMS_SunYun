using System.Web.Http;
using System.Collections.Generic;
using MicronTMS.BL.Implementation;
using MicronTMS.DLL.Entities;

namespace MicronTMS.Controllers
{
    [RoutePrefix("api/Dashboard")]
    public class DashboardController : ApiController
    {
        private readonly DashboardBL _dashboardBL;

        public DashboardController()
        {
            _dashboardBL = new DashboardBL();
        }

        [HttpGet]
        [Route("GetRealTimeTemp")]
        public IHttpActionResult GetRealTimeTemp()
        {
            return Ok(_dashboardBL.GetRealTimeTemp());
        }

        [HttpPost]
        [Route("UpdateTempbyLimit")]
        public IHttpActionResult UpdateTempbyLimit(List<CTmsEqpConfig> fridgeInfoList)
        {
            return Ok(_dashboardBL.UpdateTempbyLimit(fridgeInfoList));
        }
    }
}