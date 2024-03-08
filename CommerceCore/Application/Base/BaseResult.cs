using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCore.Application.Base
{
    public class BaseResult
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = "";
        public string CodeSign { get; set; } = "";
    }
}
