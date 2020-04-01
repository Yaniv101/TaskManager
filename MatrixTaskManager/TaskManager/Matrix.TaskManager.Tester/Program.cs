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
			Console.WriteLine("Hello World!");

			TaskTester tester = new TaskTester();
			Console.ReadLine();
		}



	}

}
