using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Linq;
using System.Security.Authentication;
using Microsoft.AspNetCore.Server.Kestrel;

namespace Matrix.TaskManager
{
	public class Program
	{
		public static void Main(string[] args)
		{

			string currentVersion = typeof(Program).Assembly.GetName().Version.ToString();
			Console.WriteLine(Figgle.FiggleFonts.Standard.Render("MATRIX TASK MANAGER"));
			Console.WriteLine(Figgle.FiggleFonts.Standard.Render($"Version - {currentVersion}"));


			CreateHostBuilder(args.Where(arg => arg != "--console").ToArray()).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>()
					 .UseUrls("http://localhost:4000");
				})
				.ConfigureLogging((hostingContext, logging) =>
				{
					logging.AddSerilog();
				});
				//.UseKestrel((context, serverOptions) =>
				//{
				//	serverOptions.Configure(context.Configuration.GetSection("Kestrel"))
				//		.Endpoint("HTTP", listenOptions =>
				//		{
				//			listenOptions.HttpsOptions.SslProtocols = SslProtocols.None;
				//		})
				//		.Endpoint("HTTPS", listenOptions =>
				//		{
				//			listenOptions.HttpsOptions.SslProtocols = SslProtocols.Tls12;
				//		});
				//});


	}
}
