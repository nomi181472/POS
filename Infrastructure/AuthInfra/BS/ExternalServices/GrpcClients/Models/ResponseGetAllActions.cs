using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.ExternalServices.GrpcClients.Models
{
    public class ResponseGetAllActions
    {
        public List<string> Routes { get; set; }
        public string ApiName { get; set; }
        public bool IsWorking { get; set; }
        public string Message { get; set; } 
    }
}
