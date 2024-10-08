﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.AuthService.Models.Request
{
    public class RequestSignUp
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public List<string> RoleIds { get; set; } =new List<string>();
    }
}
