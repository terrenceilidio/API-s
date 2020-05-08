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
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string propertyId { get; set; }
        public virtual ICollection<Manager> managers { get; set; }
        public virtual ICollection<Tenant> tenants { get; set; }
        public virtual ICollection<LandLordProperty> landLordProperties { get; set; } 
        public int rooms { get; set; }
        public Boolean isFull {get; set;}
        [Required]
        public int roomsAvailable { get; set; } 
        public int rentTotal { get; set; }
        public double rating { get; set; }
        [Required]
        [MaxLength(200)]
        public string address { get; set; }
        public string rules { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
