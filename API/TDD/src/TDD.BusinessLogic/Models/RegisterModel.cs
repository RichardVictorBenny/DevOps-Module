﻿// File: LoginModel.cs
// Author: Richard Benny
// Purpose: Represents the data model for user login credentials.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDD.BusinessLogic.Models
{
    public class RegisterModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
