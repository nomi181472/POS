
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Models.RepoResultModels
{
    public class GetterResult<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; } = String.Empty;
        public T Data { get; set; }
    }
}
