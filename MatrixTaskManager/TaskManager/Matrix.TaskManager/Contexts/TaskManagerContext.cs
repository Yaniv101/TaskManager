using Matrix.TaskManager.Common.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Matrix.TaskManager.Contexts
{
	//Add-Migration Matrix.TaskManager.Repository.TaskManagerContext
	//Add-Migration Matrix.TaskManager.Repository.TaskManagerContextSeed

	//update-database
	public class TaskManagerContext : DbContext
	{
		public TaskManagerContext(DbContextOptions options)
			: base(options)
		{
		}

		public DbSet<UserInfo> Users { get; set; }

		public DbSet<TaskInfo> Tasks { get; set; }
		public DbSet<UserTasks> UserTasks { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserInfo>().HasData(new UserInfo
			{
				UserId = 1,
				FirstName = "admin",
				LastName = "admin",
				UserName = "admin",
				Password = "12345",
				Email = "admin@gmail.com",
				IsActive = true,
				RegisterTS = DateTime.Now,
				Sex = enSex.Male,
				Phone = "",
			});

		}

	}
}
