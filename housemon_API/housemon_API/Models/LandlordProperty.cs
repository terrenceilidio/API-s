using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace housemon_API.Models
{
    public class LandLordProperty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public Landlord LandLord { get; set; }
        public Property Property { get; set; }
        public DateTime LastVisit { get; set; }
    }
}
