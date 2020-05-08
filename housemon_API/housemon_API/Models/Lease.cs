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
        public string userId { get; set; }
        public DateTime createdAt { get; set; } = new DateTime();
        public DateTime updatedAt { get; set; }
        public byte[] signature { get; set; }
        public DateTime startDate {get; set;}
        public string houseId { get; set; }
        public string leaseText { get; set; }
    }
}
