using Matrix.TaskManager.Common.Model;
using Matrix.TaskManager.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Options;
using Matrix.TaskManager.Common.Configuration;
using Matrix.TaskManager.Interfaces;

namespace Matrix.TaskManager.Controllers
{
    [Authorize]
    [Route("api/taskmanager")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "task-svc")]
    public class TaskManagerController : ControllerBase
    {
        private readonly ILogger<TaskManagerController> _logger;
        private readonly ITaskManagerRepository _dataRepository;
        private readonly IEmailRepository _emailRepository;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _appSettings;

        public TaskManagerController(ILogger<TaskManagerController> logger,
            ITaskManagerRepository dataRepository,
            IEmailRepository  emailRepository,
            IOptions<AppSettings> appSettings,
            IHttpContextAccessor httpContextAccessor)

        {
            _logger = logger;
            _dataRepository = dataRepository;
            _emailRepository = emailRepository;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("createuser")]
        public async Task<IActionResult> AddUserAsync(UserInfo user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                _logger.LogDebug($"AddUserAsync: failed to create user");
                return BadRequest("User is null.");
            }

            var result  = await _dataRepository.AddUser(user);
            if (result == 0)
            {
                _logger.LogDebug($"failed to create user {user.Email}");
            }
            return Ok(result);
        }

        [HttpPost("updateuser")]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody]UserInfo user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                _logger.LogDebug($"UpdateUserAsync: failed to update user {id}");
                return BadRequest("User is null.");
            }

            var userToUpdate = await _dataRepository.GetUser(id);
            if (userToUpdate == null)
            {
                return NotFound("The User record couldn't be found.");
            }

            return Ok(await _dataRepository.UpdateUser(user,userToUpdate));
        }

        [HttpPost("deleteuser")]
        public async Task<IActionResult> DeleteUserAsync(int userId, CancellationToken cancellationToken)
        {
            return Ok(await _dataRepository.DeleteUser(userId));
        }

        [HttpGet("getuser")]
        public async Task<IActionResult> GetUserAsync(int userid,  CancellationToken cancellationToken)
        {
            var user = await _dataRepository.GetUser(userid);
            if (user == null)
            {
                _logger.LogDebug($"GetUserAsync: failed to retrive user {userid}");
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("addtask")]
        public async Task<IActionResult> AddTaskAsync(int userId ,TaskInfo task ,CancellationToken cancellationToken)
        {
            var newTask= await _dataRepository.AddTask(userId, task);

            if (newTask == null)
            {
                _logger.LogDebug($"AddTaskAsync: failed to create task for user {userId}");
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost("updatetask")]
        public async Task<IActionResult> UpdateTaskAsync(TaskInfo task, CancellationToken cancellationToken)
        {
            if (task== null)
            {
                return BadRequest("task is null.");
            }

            var taskToUpdate = _dataRepository.GetTask(task.TaskId);
            if (taskToUpdate == null)
            {
                return NotFound("The task record couldn't be found.");
            }

            return Ok(await _dataRepository.UpdateTask(task, taskToUpdate));
        }

        [HttpPost("updatetaskstatus")]
        public async Task<IActionResult> UpdateTaskStatusAsync(int taskId,enTaskStatus status, CancellationToken cancellationToken)
        {

            return Ok(await _dataRepository.UpdateTaskStatus(taskId,status));
        }

        [HttpPost("deletetask")]
        public async Task<IActionResult> DeleteTaskAsync(TaskInfo task, CancellationToken cancellationToken)
        {
            return Ok(await _dataRepository.DeleteTask(task));
        }

        [HttpPost("deleteusertasks")]
        public async Task<IActionResult> DeleteUserTasksAsync(int userId, CancellationToken cancellationToken)
        {
            return Ok(await _dataRepository.DeleteUserTasks(userId));
        }


        [HttpPost("sheretasks")]
        public async Task<IActionResult> ShereTasksAsync(int userId, int userIdToShere,
            List<TaskInfo> tasksToShere,   CancellationToken cancellationToken)
        {
            var result = await _dataRepository.ShereTasks(userId, tasksToShere, userIdToShere);
            //send email to user with new tasks
            var user =await _dataRepository.GetUser(userId);
            var userTo =await _dataRepository.GetUser(userIdToShere);
            var tasks = await _dataRepository.GetUserTasks(userId);
            await _emailRepository.SendEmail(user.Email, userTo.Email, "New Tasks for you", FormatTasks(tasks));
            return Ok(result);
        }

        [HttpPost("emailmytasks")]
        public async Task<IActionResult> EmailMyTasksAsync(CancellationToken cancellationToken)
        {
            var email  = ExtractJWTEmail();
            if (!string.IsNullOrEmpty(email))
            {
                var tasks = await _dataRepository.GetUserTasksByEmail(email);

                return Ok(await _emailRepository.SendEmail(
                    _appSettings.Email.UserName, email, "My Tasks",
                    FormatTasks(tasks)));
            }
            else
            {
                return NotFound("failed to extract user data");
            }
        }

        private string ExtractJWTEmail()
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.
             Request.Headers["Authorization"];
                var handler = new JwtSecurityTokenHandler();

                var res = handler.ReadJwtToken(accessToken.ToString().Replace("Bearer ", ""));
                var email = res.Claims.Where(p => p.Type == "email").First();
                return email.Value;

            }
            catch (Exception ex)
            {
                _logger.LogError("ExtractJWTEmail: failed to extract email address from jwt - " + ex.Message);
                return string.Empty;
            }
        }

        private string FormatTasks(IEnumerable<TaskInfo>  tasks)
        {
            StringBuilder sb = new StringBuilder();

            foreach (TaskInfo item in tasks)
            {
                sb.AppendLine(item.ToString());
            }

            return sb.ToString();
        }

        [HttpGet("getusertasks")]
        public async Task<IActionResult > GetUserTasksAsync(int userId ,CancellationToken cancellationToken)
        {
            var tasks = await _dataRepository.GetUserTasks(userId);
            if (tasks == null)
            {
                _logger.LogError($"failed to retrive user {userId} tasks");
                return NotFound();
            }

            return Ok(tasks);
        }

        [HttpGet("getusertasksbyemail")]
        public async Task<IActionResult> GetUserTasksByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var tasks  = await _dataRepository.GetUserTasksByEmail(email);

            if (tasks == null)
            {
                _logger.LogError($"failed to retrive user tasks by email {email}");
                return NotFound();
            }
            return Ok(tasks);
        }

    }
}