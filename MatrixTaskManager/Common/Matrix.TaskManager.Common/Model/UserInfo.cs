using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Matrix.TaskManager.Common.Model
{
    //[DataContract(Name = "UserInfo")]
    public class UserInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  UserId{ get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
        
        public string Phone { get; set; }
        
        public string Email { get; set; }

        public enSex Sex { get; set; }

        public bool IsActive{ get; set; }

        public DateTime RegisterTS { get; set; }

        public string Token { get; set; }


  //      public virtual ICollection<UserTasks> UserTasks { get; set; }
        public UserInfo()
        {
//            this.UserTasks = new HashSet<UserTasks>();
        }
    }

    public enum enSex
    {
        Male=0,
        Female=1

    }
}