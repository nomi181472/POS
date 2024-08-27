

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Models.RepoResultModels
{
    public class SetterResult
    {
        public bool Result { get; set; }
        public bool IsException { get; set; }
        public string Message { get; set; }
    }
}