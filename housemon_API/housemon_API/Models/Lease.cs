using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace housemon_API.Models
{
    public class Lease
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string leaseId { get; set; }
        public string propertyId { get; set; }
        public DateTimeOffset createdAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset startDate {get; set;}
        public DateTimeOffset endDate { get; set; }
        public string data { get; set; }
        public virtual ICollection<Tenant> Tenants { get; set; }
    }
}
