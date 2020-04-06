using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Linq;
using System.Security.Authentication;
using Microsoft.AspNetCore.Server.Kestrel;
using Matrix.TaskManager.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Matrix.TaskManager
{
	public class Program
	{
		public static void Main(string[] args)
		{

			string currentVersion = typeof(Program).Assembly.GetName().Version.ToString();
			Console.WriteLine(Figgle.FiggleFonts.Standard.Render("MATRIX TASK MANAGER"));
			Console.WriteLine(Figgle.FiggleFonts.Standard.Render($"Version - {currentVersion}"));


			var host = CreateHostBuilder(args.Where(arg => arg != "--console").ToArray()).Build();

			using (var scope = host.Services.CreateScope()) 
			{
				using (var context = scope.ServiceProvider.GetService<TaskManagerContext>())
				{
					context.Database.EnsureCreated();
				}
			}
			host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
//					 .UseUrls("http://localhost:4000");
				})
				.ConfigureLogging((hostingContext, logging) =>
				{
					logging.AddSerilog();
				});
	}
}
