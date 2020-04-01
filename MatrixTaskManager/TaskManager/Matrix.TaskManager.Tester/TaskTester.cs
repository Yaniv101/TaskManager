using Matrix.TaskManager.Common.Extensions;
using Matrix.TaskManager.Common.Helpers;
using Matrix.TaskManager.Common.Model;
using Matrix.TaskManager.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Matrix.TaskManager.Tester
{
	public class TaskTester
	{
		private string AuthUrl = "http://localhost:4000/api/authentication/{0}";
		private string TaskUrl = "http://localhost:4000/api/taskmanager/{0}";
		public TaskTester()
		{
			RunTest();
		}

		private async void RunTest()
		{
			var token = await Login("admin", "12345");

			var adminUser =await GetUser(1,token);//get admin user

			var userid = await CreateUser(token);

			var allUsers = await GetAllUsers(token);

			var newTask1 = await AddTask("1", adminUser.UserId, token);

			var newTask2 = await AddTask("2", adminUser.UserId, token);

			await UpdateTask(newTask1, token);

			await UpdateTaskStatus(newTask1, enTaskStatus.InProgress, token);

			var allTasks = GetUserTasks(adminUser.UserId, token);

			await ShereTasks(adminUser.UserId, userid, new List<TaskInfo>() { newTask1, newTask2 }, token);

			var tasksByMail =await GetTasksByEmail("Yaniv101@gmail.com", token );


			var yanivToken = await Login( "yaniv", "yaniv");
			await EmailMyTasks(yanivToken);

			await DeleteUser(userid, yanivToken);
			await DeleteUserTasks(adminUser.UserId, token);

		}
		private async Task<string> DeleteUserTasks(int userid, string token)
		{
			return await CallTask(string.Format(TaskUrl, "deleteusertasks?userid= " + userid), token,
					"", "POST");
		}

		private async Task<string> DeleteUser(int userid, string token)
		{
			return await CallTask(string.Format(TaskUrl, "deleteuser?userid= " + userid), token,
					"", "POST");
		}

		private async Task<string> EmailMyTasks(string token)
		{
			return await CallTask(string.Format(TaskUrl, "emailmytasks"),
				token, "", "POST");
		}

		private async Task<string> Login(string user, string pass)
		{
			var token =  await AuthenticateTokenAsync( string.Format(AuthUrl, 
												"authenticate"), user,pass);

			return token;
		}

		private async Task<List<UserInfo>> GetAllUsers(string token)
		{
			var allusersJson = await CallTask(
				string.Format(AuthUrl, "getusers"), token,
				"", "GET");
			return JsonConvert.DeserializeObject<List<UserInfo>>(allusersJson);
		}

		private async Task<List<TaskInfo>> GetTasksByEmail(string email, string token)
		{
			var tasksByMailJson =
				await CallTask(string.Format(TaskUrl, "getusertasksbyemail?email=" + email),
				token, "", "GET");
			return JsonConvert.DeserializeObject<List<TaskInfo>>(tasksByMailJson);
		}

		private async Task<string> ShereTasks(int userFromId, int userToid, List<TaskInfo> tasks, string token)
		{
			return await CallTask(string.Format(TaskUrl, 
							"sheretasks?userid=" + userFromId+ "&userIdToShere= " + userToid),token,
							JsonConvert.SerializeObject(tasks), "POST");
		}

		private async Task<List<TaskInfo>> GetUserTasks(int userId, string token)
		{
			var result = await CallTask(string.Format(TaskUrl, "getusertasks?userid= " + userId), token,
						"", "GET");
			return JsonConvert.DeserializeObject<List<TaskInfo>>(result);
		}

		private async Task<string> UpdateTaskStatus(TaskInfo newTask, enTaskStatus status, string token)
		{
			return await CallTask(string.Format(TaskUrl, "updatetaskstatus?taskid= " + 
										newTask.TaskId + "&status=" + status.ToString()), token,
				JsonConvert.SerializeObject(newTask), "POST");
		}

		private async Task<string> UpdateTask(TaskInfo newTask, string token)
		{
			newTask.DueDate = DateTime.Now;

			return await CallTask(string.Format(TaskUrl, "updatetask"), token,
							JsonConvert.SerializeObject(newTask), "POST");

		}

		private async Task<TaskInfo> AddTask(string tinx, int userId, string token)
		{
			var taskJson =  await CallTask(string.Format(TaskUrl, "addtask?userId=" + userId), token,
							JsonConvert.SerializeObject(
								 	new TaskInfo()
									 {
										 TaskName = "t"+ tinx,
										 Address = "ad" + tinx,
										 CityName = "c" + tinx,
										 Status = enTaskStatus.New,
										 Priority = enPriority.Hige,
										 DueDate = DateTime.Now
									 }

								), "POST");
			return  JsonConvert.DeserializeObject<TaskInfo>(taskJson);

		}

		private async Task<UserInfo > GetUser(int userId,string token)
		{
			var userJson = await CallTask(string.Format(TaskUrl, "getuser?userid=" + userId), token,
				"", "GET");
			var user = JsonConvert.DeserializeObject<UserInfo>(userJson);
			return user;
		}

		private async Task<int> CreateUser(string token)
		{
			var userid = await CallTask(string.Format(TaskUrl, "createuser"), token,
				JsonConvert.SerializeObject(new UserInfo()
				{
					FirstName = "Yaniv",
					LastName = "Aviel",
					UserName = "yaniv",
					Password = "yaniv",
					Email = "Yaniv101@gmail.com",
					IsActive = true,
					Phone = "052-5524099",
					Sex = enSex.Male,
					RegisterTS = DateTime.Now,
				}), "POST");

			return CastHelper.CastValueToInt( userid);
		}

		public async Task<string> CallTask(string url, string token, string data, string cmdType)
		{
			Console.WriteLine(url + " - " + data);
			Uri serviceAccountTokenUri = new Uri(url);

			string requestJson = data;
			using (var webClient = NetExtensions.WebClient())
			{
				try
				{
					ConfigureRequestHeaders(webClient, token);
					string response = "";
					if (cmdType == "POST")
					{
						response = await webClient.UploadStringTaskAsync(serviceAccountTokenUri, requestJson);
					}
					else
					{
						response = await webClient.DownloadStringTaskAsync(serviceAccountTokenUri);
					}

					//_logger.LogDebug($"Response:\n{response}");
					return response;
				}
				catch (Exception ex)
				{
					//_logger.LogCritical(exception.ToString());
				}
				finally
				{
					MemoryUtilities.CleanupWebClient(webClient);
				}
			}


			return null;
		}

		public async Task<string> AuthenticateTokenAsync(
			string url,string userName, string password )
		{
			Uri serviceAccountTokenUri = new Uri(url);
			AuthenticateModel request = new AuthenticateModel()
			{ Username = userName, Password = password };
			string requestJson = JsonConvert.SerializeObject(request);
			using (var webClient = NetExtensions.WebClient())
			{
				//_logger.LogDebug($"Request:\n{requestJson}");

				try
				{
					webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
					string response = await webClient.UploadStringTaskAsync(serviceAccountTokenUri, requestJson);
					//_logger.LogDebug($"Response:\n{response}");
					var responseTokenInfo = JsonConvert.DeserializeObject<UserInfo>(response);
					return responseTokenInfo.Token;
				}
				catch (Exception ex)
				{
					//_logger.LogCritical(exception.ToString());
				}
				finally
				{
					MemoryUtilities.CleanupWebClient(webClient);
				}
			}


			return null;
		}



		private static void ConfigureRequestHeaders(
			WebClient webClient, string accessToken)
		{
			string bearerValue = $"Bearer {accessToken}";
			webClient.Headers.Add(HttpRequestHeader.Authorization, bearerValue);
			webClient.Headers.Add(HttpRequestHeader.ContentType, @"application/json");
			webClient.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
		}

	}
}
