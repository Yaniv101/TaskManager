using Matrix.TaskManager.Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Matrix.TaskManager.Interfaces
{
	public interface ITaskManagerRepository
	{
		Task<IEnumerable<UserInfo>> GetAllUsers();
		UserInfo FindUser(string userName, string password);
		Task<UserInfo> GetUser(int id);
		Task<int> AddUser(UserInfo entity);
		Task<bool> UpdateUser(UserInfo user, UserInfo entity);
		Task<bool> DeleteUser(int userId);

		TaskInfo GetTask(int taskId);
		Task<TaskInfo> AddTask(int userId, TaskInfo task);
		Task<bool> DeleteTask(TaskInfo task);
		Task<bool> DeleteUserTasks(int userId);
		Task<bool> UpdateTask(TaskInfo task, TaskInfo taskToUpdate);
		Task<bool> ShereTasks(int userId, List<TaskInfo> tasksToShere, int userIdToShere);
		Task<IEnumerable<TaskInfo>> GetUserTasks(int userId);
		Task<IEnumerable<TaskInfo>> GetUserTasksByEmail(string email);
		Task<bool> UpdateTaskStatus(int taskId, enTaskStatus status);

	}
}
