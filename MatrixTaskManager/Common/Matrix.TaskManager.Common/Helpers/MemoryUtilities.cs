using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Matrix.TaskManager.Common.Helpers
{
	public class MemoryUtilities
	{
		public static void CleanupWebClient(WebClient webClient)
		{
			webClient?.Headers.Clear();
			webClient?.Dispose();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
	}
}
