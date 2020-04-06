using System;

namespace Matrix.TaskManager.Tester
{
	class Program
	{

		static void Main(string[] args)
		{

			string currentVersion = typeof(Program).Assembly.GetName().Version.ToString();
			Console.WriteLine(Figgle.FiggleFonts.Standard.Render("MATRIX TASK MANAGER"));
			Console.WriteLine(Figgle.FiggleFonts.Standard.Render($"Version - {currentVersion}"));
			Console.WriteLine(($"Press any key to start the task manager test..."));

			Console.ReadLine();

			TaskTester tester = new TaskTester();
			tester.RunTest();
			Console.ReadLine();
		}



	}

}
