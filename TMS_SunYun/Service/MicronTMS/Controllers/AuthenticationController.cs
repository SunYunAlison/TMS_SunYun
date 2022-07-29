using System.Web.Http;
using MicronTMS.BL.Implementation;
using MicronTMS.DLL.Entities;

namespace MicronTMS.Controllers
{
    [RoutePrefix("api/Authentication")]
    public class AuthenticationController : ApiController
    {
        private readonly AuthenticationBL _authenticationBL;

        public AuthenticationController()
        {
            _authenticationBL = new AuthenticationBL();
        }

        [HttpPost]
        [Route("NewAccount")]
        public IHttpActionResult NewAccount(CUserInfo userInfo)
        {
            return Ok(_authenticationBL.NewAccount(userInfo));
        }

        [HttpPost]
        [Route("UpdateAccountInfo")]
        public IHttpActionResult UpdateAccountInfo(CUserInfo userInfo)
        {
            return Ok(_authenticationBL.UpdateAccountInfo(userInfo));
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IHttpActionResult GetAllUsers()
        {
            return Ok(_authenticationBL.GetAllUsers());
        }

        [HttpPost]
        [Route("DeleteAccount")]
        public IHttpActionResult DeleteAccount(CUserInfo userInfo)
        {
            return Ok(_authenticationBL.DeleteAccount(userInfo));
        }
    }
}