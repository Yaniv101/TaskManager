using Matrix.TaskManager.Common.Extensions;
using Matrix.TaskManager.Common.Helpers;
using Matrix.TaskManager.Common.Model;
using Matrix.TaskManager.Common.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Matrix.TaskManager.Tester
{
	class Program
	{

		static void Main(string[] args)
		{

			string currentVersion = typeof(Program).Assembly.GetName().Version.ToString();
			Console.WriteLine(Figgle.FiggleFonts.Standard.Render("MATRIX TASK MANAGER"));
			Console.WriteLine(Figgle.FiggleFonts.Standard.Render($"Version - {currentVersion}"));

			Console.ReadLine();

			TaskTester tester = new TaskTester();
			Console.ReadLine();
		}



	}

}
