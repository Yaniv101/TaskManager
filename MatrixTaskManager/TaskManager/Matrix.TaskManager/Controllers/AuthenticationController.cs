using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading;
using Matrix.TaskManager.Common.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using Matrix.TaskManager.Services;
using Matrix.TaskManager.Common.Models;
using Microsoft.Extensions.Options;
using Matrix.TaskManager.Common.Logging;
using Microsoft.Extensions.Logging;

namespace Matrix.TaskManager.Controllers
{
    [Authorize]
    [Route("api/authentication")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "authentication-svc")]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService _userService;
        private ILogger< AuthenticationController> _logger;
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
