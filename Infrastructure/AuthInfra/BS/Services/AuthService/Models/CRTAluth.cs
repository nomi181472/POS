﻿
using BS.Services.AuthService.Models.Request;
using DM.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceServices.Services.AllowanceService.Models
{
    public static class CRTAuth
    {
        public static User ToDomain(this RequestSignUp request)
        {
            return new User
            {
                
                Name = request.Name,
                Email=request.Email,

            };
        }
        
    }
}