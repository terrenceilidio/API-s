using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace housemon_API.Models
{
    public class ResponseModel<T>
    {
        public string message { get; set; }
        public Boolean status { get; set; }
        public T data { get; set; }
    }
}
