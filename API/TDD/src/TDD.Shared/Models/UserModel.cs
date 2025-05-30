// File: TaskModel.cs
// Author: Richard Benny
// Purpose: Represents the business logic model for a user's task.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDD.Shared.Models
{
    public class UserModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string GetFullName()
        {
            return $"{this.FirstName} {this.LastName}";
        }
    }

    
}
