using Matrix.TaskManager.Common.Models;
using Matrix.TaskManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Matrix.TaskManager.Controllers
{
    [Authorize]
    [Route("api/authentication")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "authentication-svc")]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService _userService;
        private ILogger<AuthenticationController> _logger;
        public AuthenticationController(IAuthenticationService userService,
            ILogger<AuthenticationController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
            {
                _logger.LogError("Authenticate: Username or password is incorrect");
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            return Ok(user);
        }

        [HttpGet("getusers")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }
    }
}
