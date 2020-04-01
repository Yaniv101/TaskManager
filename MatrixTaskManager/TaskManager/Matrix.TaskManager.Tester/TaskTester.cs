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
			var token = await AuthenticateTokenAsync(
				string.Format(AuthUrl, "authenticate") ,"admin", "12345");

			var adminuserJson = await CallTask(string.Format(TaskUrl, "getuser?userid=1"), token,
				"", "GET");
			var adminUser= JsonConvert.DeserializeObject<UserInfo>(adminuserJson);

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


			var task1 = await CallTask(string.Format(TaskUrl, "addtask?userId=" + adminUser.UserId), token,
							JsonConvert.SerializeObject(
								 	new TaskInfo()
									 {
										 TaskName = "t1",
										 Address = "ad1",
										 CityName = "c1",
										 Status = enTaskStatus.New,
										 Priority = enPriority.Hige,
										 DueDate = DateTime.Now
									 }

								), "POST");

			var task2 = await CallTask(string.Format(TaskUrl, "addtask?userId=" + adminUser.UserId), token,
							JsonConvert.SerializeObject(
								 	new TaskInfo()
									 {
										 TaskName = "t2",
										 Address = "ad2",
										 CityName = "c2",
										 Status = enTaskStatus.New,
										 Priority = enPriority.Hige,
										 DueDate = DateTime.Now
									 }

								), "POST");

			var newTask1 = JsonConvert.DeserializeObject<TaskInfo>(task1);
			var newTask2 = JsonConvert.DeserializeObject<TaskInfo>(task2);
			newTask1.DueDate = DateTime.Now;
			var result = await CallTask(string.Format(TaskUrl, "updatetask"), token,
							JsonConvert.SerializeObject(newTask1), "POST");

			result = await CallTask(string.Format(TaskUrl, "updatetaskstatus?taskid= " + newTask1.TaskId + "&status=InProgress"), token,
				JsonConvert.SerializeObject(newTask1), "POST");


			result = await CallTask(string.Format(TaskUrl, "getusertasks?userid= " + adminUser.UserId), token,
						JsonConvert.SerializeObject(newTask1), "GET");
			var allTasks = JsonConvert.DeserializeObject<List<TaskInfo>>(result);

			result = await CallTask(string.Format(TaskUrl, "sheretasks?userid=" + adminUser .UserId+ "&userIdToShere= " + userid),
				token,
				JsonConvert.SerializeObject(
					new List<TaskInfo>() { newTask1, newTask2 }), "POST");

			var tasksByMailJson= 
				await CallTask(string.Format(TaskUrl,
				"getusertasksbyemail?email=" + "Yaniv101@gmail.com"), 
				token, "", "GET");
			var tasksByMail = JsonConvert.DeserializeObject<List<TaskInfo>>(tasksByMailJson);

			var allusersJson  = await CallTask(
				string.Format(AuthUrl, "getusers") ,
				token,
				"", "GET");
			var allUsers = JsonConvert.DeserializeObject<List<UserInfo>>(allusersJson);

			token = await AuthenticateTokenAsync(
				string.Format(AuthUrl, "authenticate"), "yaniv", "yaniv");

			result = await CallTask(string.Format(TaskUrl, "emailmytasks"),
				token,
				"", "POST");



			result = await CallTask(string.Format(TaskUrl, "deleteuser?userid= " + userid), token,
						"", "POST");

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
