using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.ActionsService.Models.Request
{
    public class RequestAddListOfActions
    {
        public List<RequestAddAction> Actions { get; set; }
    }
}
