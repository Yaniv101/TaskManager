using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Matrix.TaskManager.Common.Model
{
	public class UserTasks
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int UserTaskId { get; set; }
		public int UserId { get; set; }
		public int TaskId { get; set; }

	}
}
