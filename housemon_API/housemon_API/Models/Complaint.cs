using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace housemon_API.Models
{
    public class Complaint
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string complaintId { get; set; }
        public string userId { get; set; }
        public string propertyId { get; set; }
        public DateTimeOffset dateMade { get; set; } = DateTimeOffset.Now;
        public Boolean isResolved { get; set; }
        public DateTimeOffset dateResolved { get; set; }
        public string data  { get; set; }
        
    }
}
