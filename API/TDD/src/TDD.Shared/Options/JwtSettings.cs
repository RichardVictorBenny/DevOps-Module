// File: JwtSettings.cs
// Author: Richard Benny
// Purpose: Represents configuration settings for JWT authentication, including key, issuer, and audience.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDD.Shared.Options
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
