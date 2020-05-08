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
        [Required]
        public int rentAmount { get; set; }
        public string property_id { get; set; }
        public int deposit { get; set; }
        public DateTime rentDueDate { get; set; }
    }
}
