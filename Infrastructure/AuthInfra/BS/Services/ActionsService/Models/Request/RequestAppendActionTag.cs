﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.ActionsService.Models.Request
{
    public class RequestAppendActionTag
    {
        public string actionId {  get; set; }

        public string tagToAppend { get; set; }
    }
}
