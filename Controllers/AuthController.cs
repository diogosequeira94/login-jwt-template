using loginjwt.Business;
using loginjwt.Model.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace loginjwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ILoginBusiness _loginBusiness;

        public AuthController(ILoginBusiness loginBusiness)
        {
            _loginBusiness = loginBusiness;
        }

        [HttpPost]
        [Route("signin")]
        public IActionResult SignIn([FromBody] UserVO user)
        {
            if (user == null) return BadRequest("Invalid client request");
            var token = _loginBusiness.ValidateCredentials(user);
            if (token == null) return Unauthorized();
            return Ok(token);
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh([FromBody] TokenVO tokenVo)
        {
            if (tokenVo is null) return BadRequest("Invalid client request");
            var token = _loginBusiness.ValidateCredentials(tokenVo);
            if (token == null) return BadRequest();
            return Ok(token);
        }

        [HttpGet]
        [Route("revoke")]
        // We need to be authenticated
        [Authorize("Bearer")]
        public IActionResult Revoke()
        {
            if (User.Identity != null)
            {
                var username = User.Identity.Name;
                var result = _loginBusiness.RevokeToken(username);

                if (!result) return BadRequest("Invalid client request");
            }

            return NoContent();
        }
    }
}