using Matrix.TaskManager.Common.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Matrix.TaskManager.Contexts;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using Matrix.TaskManager.Interfaces;

namespace Matrix.TaskManager.Repository
{

	public class TaskManagerRepository : ITaskManagerRepository
	{
		readonly TaskManagerContext _taskContext;
		private readonly ILogger<TaskManagerRepository> _logger;
		private IHttpContextAccessor _httpContextAccessor;


		public TaskManagerRepository(
			TaskManagerContext context, 
			ILogger<TaskManagerRepository> logger,
			IHttpContextAccessor httpContextAccessor)
		{
			_taskContext = context;
			_logger = logger;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<IEnumerable<UserInfo>> GetAllUsers()
		{
			return await _taskContext.Users.ToListAsync();
		}

		public async Task<UserInfo> GetUser(int id)
		{
			return await _taskContext.Users.FirstOrDefaultAsync(e => e.UserId == id);
		}

		public UserInfo FindUser(string userName, string password)
		{
			return _taskContext.Users.FirstOrDefault(e => e.UserName == userName && e.Password == password);
		}

		public async Task<TaskInfo> AddTask(int userId, TaskInfo task)
		{
			try
			{
				await _taskContext.Tasks.AddAsync(task);
				await _taskContext.SaveChangesAsync();
				var user = await GetUser(userId);
				if (user == null)
				{
					throw new Exception($"Couldn't find user id{userId}");
				}

				_taskContext.UserTasks.Add(new UserTasks()
				{
					TaskId = task.TaskId,
					UserId = user.UserId,
				});
				await _taskContext.SaveChangesAsync();
				return task;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task<bool> DeleteUserTasks(int userId)
		{
			try
			{
				var userTasks = await _taskContext.UserTasks.Where(p => p.UserId== userId).ToListAsync();
				_taskContext.UserTasks.RemoveRange(userTasks);
				await _taskContext.SaveChangesAsync();
				var taskUnAttached = await _taskContext.UserTasks.
						Where(p => userTasks.Select(x=>x.TaskId).Contains( p.TaskId)).ToListAsync();
				if (taskUnAttached.Count == 0)
				{
					_taskContext.Tasks.RemoveRange(await
						_taskContext.Tasks.Where(p => userTasks.Select(x => x.TaskId).Contains(p.TaskId)).ToListAsync());
					await _taskContext.SaveChangesAsync();
				}
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("DeleteUserTasks: " + ex.Message);
				return false;
			}
		}
		public async Task<bool> DeleteTask(TaskInfo task)
		{
			try
			{

				var userTasks = await _taskContext.UserTasks.Where(p => p.TaskId == task.TaskId).ToListAsync();
				_taskContext.UserTasks.RemoveRange(userTasks);

				_taskContext.Tasks.Remove(task);

				await _taskContext.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public TaskInfo GetTask(int taskId)
		{
			return _taskContext.Tasks.FirstOrDefault(e => e.TaskId == taskId);
		}

		public async Task<IEnumerable<TaskInfo>> GetUserTasks(int userId)
		{
			return
				await (from x in _taskContext.UserTasks
					   join y in _taskContext.Tasks on x.TaskId equals y.TaskId
					   where x.UserId == userId
					   select y).ToListAsync();
		}
		public async Task<IEnumerable<TaskInfo>> GetUserTasksByEmail(string email)
		{
			return await (from x in _taskContext.UserTasks
						  join y in _taskContext.Tasks on x.TaskId equals y.TaskId
						  join u in _taskContext.Users on x.UserId equals u.UserId
						  where u.Email == email.Trim()
						  select y).ToListAsync();
		}

		public async Task<bool> UpdateTask(TaskInfo task, TaskInfo taskToUpdate)
		{
			try
			{
				task.TaskName = taskToUpdate.TaskName;
				task.Priority = taskToUpdate.Priority;
				task.DueDate = taskToUpdate.DueDate;
				task.CityName = taskToUpdate.CityName;
				task.Address = taskToUpdate.Address;
				await _taskContext.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		public async Task<bool> UpdateTaskStatus(int taskId, enTaskStatus status)
		{
			try
			{
				var task = GetTask(taskId);
				if (task != null)
				{
					task.Status = status;
					await _taskContext.SaveChangesAsync();
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{

				return false;
			}
		}

		public void AddTaskToUser(int userId, int taskId)
		{
			_taskContext.UserTasks.Add(new UserTasks()
			{
				UserId = userId,
				TaskId = taskId
			});
		}
		public async Task<bool> ShereTasks(int userId, List<TaskInfo> tasksToShere, int userIdToShere)
		{
			try
			{
				var userTasks = await _taskContext.UserTasks.Where(p => p.UserId == userIdToShere).ToListAsync();

				foreach (var task in tasksToShere)
				{
					if (userTasks.Where(p => p.TaskId == task.TaskId).Count() == 0)
					{
						AddTaskToUser(userIdToShere, task.TaskId);
					}
				}

				await _taskContext.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		public async Task<int> AddUser(UserInfo entity)
		{
			try
			{
				await _taskContext.Users.AddAsync(entity);
				await _taskContext.SaveChangesAsync();
				return entity.UserId;
			}
			catch (Exception ex)
			{
				return 0;
			}
		}

		public async Task<bool> UpdateUser(UserInfo user, UserInfo entity)
		{
			try
			{
				user.FirstName = entity.FirstName;
				user.LastName = entity.LastName;
				user.Email = entity.Email;
				user.Password = entity.Password;
				user.Phone = entity.Phone;
				user.Sex = entity.Sex;
				user.UserName = entity.UserName;

				await _taskContext.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async Task<bool> DeleteUser(int userId)
		{
			try
			{
				_taskContext.UserTasks.RemoveRange(_taskContext.UserTasks.Where(p => p.UserId == userId));
				_taskContext.Users.Remove(_taskContext.Users.Where(p => p.UserId == userId).FirstOrDefault());
				await _taskContext.SaveChangesAsync();

				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}
}
