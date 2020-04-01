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
            return Ok(await _dataRepository.AddUser(user));
        }

        [HttpPost("updateuser")]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody]UserInfo user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
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

            return Ok(await _dataRepository.GetUser(userid));
        }

        [HttpPost("addtask")]
        public async Task<IActionResult> AddTaskAsync(int userId ,TaskInfo task ,CancellationToken cancellationToken)
        {
            return Ok(await _dataRepository.AddTask(userId,task));
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
            var tasks = await _dataRepository.GetUserTasksByEmail(email);

            return Ok(await _emailRepository.SendEmail(
                _appSettings.Email.UserName, email, "My Tasks",
                FormatTasks(tasks))  );
        }

        private string ExtractJWTEmail()
        {
            var accessToken = _httpContextAccessor.HttpContext.
                        Request.Headers["Authorization"];
            var handler = new JwtSecurityTokenHandler();

            var res = handler.ReadJwtToken(accessToken.ToString().Replace("Bearer ", ""));
            var email = res.Claims.Where(p => p.Type == "email").First();
            return email.Value;
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
            return Ok(await _dataRepository.GetUserTasks(userId));
        }

        [HttpGet("getusertasksbyemail")]
        public async Task<IActionResult> GetUserTasksByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return Ok(await _dataRepository.GetUserTasksByEmail(email));
        }

    }
}