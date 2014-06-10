using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XiyouLibApi.Exceptions
{
    public class ParamExistedException : System.Exception
    {
        public ParamExistedException() : base("Param already existed.") { }
    }
}