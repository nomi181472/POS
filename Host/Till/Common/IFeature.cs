using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Till.Common
{
    public interface IFeature
    {
        static abstract void Map(IEndpointRouteBuilder app);
    }
}