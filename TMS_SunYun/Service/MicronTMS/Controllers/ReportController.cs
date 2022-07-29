using System.Web.Http;
using MicronTMS.BL.Implementation;
using MicronTMS.DLL.Entities;

namespace MicronTMS.Controllers
{
    [RoutePrefix("api/Report")]
    public class ReportController : ApiController
    {
        private readonly ReportBL _reportBL;

        public ReportController()
        {
            _reportBL = new ReportBL();
        }

        [HttpGet]
        [Route("GetFridgeList")]
        public IHttpActionResult GetFridgeList()
        {
            return Ok(_reportBL.GetFridgeList());
        }

        [HttpGet]
        [Route("GetTempbyMin")]
        public IHttpActionResult GetTempbyMin(string fridgeId, string startTime, string endTime)
        {
            return Ok(_reportBL.GetTempbyMin(fridgeId, startTime, endTime));
        }

        [HttpGet]
        [Route("GetTempbyHour")]
        public IHttpActionResult GetTempbyHour(string fridgeId, string startTime, string endTime)
        {
            return Ok(_reportBL.GetTempbyHour(fridgeId, startTime, endTime));
        }

        [HttpGet]
        [Route("DownloadTempbyMin")]
        public IHttpActionResult DownloadTempbyMin(string fridgeId, string startTime, string endTime)
        {
            return Ok(_reportBL.DownloadTempbyMin(fridgeId, startTime, endTime));
        }

        [HttpGet]
        [Route("DownloadTempbyHour")]
        public IHttpActionResult DownloadTempbyHour(string fridgeId, string startTime, string endTime)
        {
            return Ok(_reportBL.DownloadTempbyHour(fridgeId, startTime, endTime));
        }
    }
}