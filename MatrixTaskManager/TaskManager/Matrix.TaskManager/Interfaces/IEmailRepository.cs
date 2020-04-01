using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Matrix.TaskManager.Interfaces
{
	public interface IEmailRepository
	{
		Task<bool> SendEmail(string emailFrom, string emailTo, string title, string data);
	}
}
