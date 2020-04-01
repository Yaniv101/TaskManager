using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Matrix.TaskManager.Common.Model
{

	public class TaskInfo
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int TaskId { get; set; }
		public string TaskName { get; set; }
		public DateTime DueDate { get; set; }
		public enPriority Priority { get; set; }
		public string CityName { get; set; }
		public string Address { get; set; }
		public enTaskStatus Status { get; set; }
		//public virtual ICollection<UserTasks> UserTasks { get; set; }

		public TaskInfo()
		{
			//this.UserTasks = new HashSet<UserTasks>();
		}

		public override string ToString()
		{
			return $"TaskId:{TaskId},TaskName:{TaskName},Priority:{Priority},DueDate:{DueDate}" +
				$",CityName:{CityName},Address:{Address},Status:{Status}";
		}
	}
	public enum enTaskStatus
	{
		New,
		InProgress,
		Done
	}
	public enum enPriority
	{
		Hige,
		Meduim,
		Low
	}
}
