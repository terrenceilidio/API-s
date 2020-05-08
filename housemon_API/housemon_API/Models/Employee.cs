using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace housemon_API.Models
{
    public class Employee : User
    {
        public string propertyId { get; set; }
        public int salary { get; set; }
    }
}
