using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace housemon_API.Models
{
    public class Tenant : User
    {
        public string leaseId { get; set; }
        public string propertyId { get; set; }
        public int rentAmount { get; set; }
        public int deposit { get; set; }
        public DateTimeOffset rentDueDate { get; set; }
    }
}
