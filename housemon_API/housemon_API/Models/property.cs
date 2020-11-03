using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace housemon_API.Models
{
    public class Property
    {
        public Property()
        {
            landLordProperties = new List<LandLordProperty>();
            managers = new List<Manager>();
            tenants = new List<Tenant>();
            Leases = new List<Lease>();
            Complaints = new List<Complaint>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string propertyId { get; set; }
        public virtual ICollection<Manager> managers { get; set; }
        public virtual ICollection<Tenant> tenants { get; set; }
        public virtual ICollection<LandLordProperty> landLordProperties { get; set; }
        public virtual ICollection<Lease> Leases { get; set; }
        public virtual ICollection<Complaint> Complaints{ get; set; }
        public int rooms { get; set; }
        [MaxLength(200)]
        public string address { get; set; }
        public string country { get; set; }
        public string rules { get; set; }
        public DateTimeOffset createdAt { get; set; } 
        public DateTimeOffset updatedAt { get; set; }
    }
}
